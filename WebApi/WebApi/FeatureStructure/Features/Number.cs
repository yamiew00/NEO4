using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Evaluate.Tree;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 數字鍵:Concrete IFeature物件
    /// </summary>
    public class Number : Feature
    {
        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        private static readonly HashSet<Type> NUMBER_PREVIOUS_TYPE = new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace) };

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        private static readonly HashSet<Type> NUMBER_AFTERWARD_TYPE = new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace), typeof(Unary) };

        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        public override HashSet<Type> PreviousType => NUMBER_PREVIOUS_TYPE;

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        public override HashSet<Type> AfterWardType => NUMBER_AFTERWARD_TYPE;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="content">功能內容</param>
        public Number(char content) : base(content)
        {
        }

        /// <summary>
        /// 空建構子(給反射用的)
        /// </summary>
        public Number()
        {
        }

        /// <summary>
        /// 依功能回傳畫面物件
        /// </summary>
        /// <param name="boardObject">面板物件</param>
        /// <param name="frameUpdate">畫面更新</param>
        /// <returns>畫面物件</returns>
        public override FrameObject GetFrameObject(BoardObject boardObject, FrameUpdate frameUpdate)
        {
            string answer = frameUpdate.Answer;

            //完整運算式的刷新
            boardObject.CompleteExpression = frameUpdate.Refresh(boardObject.CompleteExpression);

            //panel, subpanel設定
            boardObject.FrameObject.SubPanel = boardObject.CompleteExpression.Substring(0, boardObject.CompleteExpression.Length - answer.Length);
            boardObject.FrameObject.Panel = answer;
            return boardObject.FrameObject;
        }

        /// <summary>
        /// 依計畫內容回傳畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        public override FrameUpdate Compute(ComputeObject computeObject)
        {
            FrameUpdate frameUpdate;

            //等號後輸入數字
            if (computeObject.LastFeature == typeof(Equal))
            {
                //特殊情況，boardObject已在父類清空
                computeObject.Init();

                frameUpdate = GetUpdateDefault(computeObject);
                frameUpdate.RemoveLength = FrameUpdate.REMOVE_ALL;
                return frameUpdate;
            }

            //backspace或clearerror之後的數字處理
            if ((computeObject.LastFeature == typeof(BackSpace) || computeObject.LastFeature == typeof(ClearError)) && computeObject.NumberField.Number == 0)
            {
                frameUpdate = GetUpdateDefault(computeObject);
                frameUpdate.RemoveLength += 1;

                return frameUpdate;
            }

            frameUpdate = GetUpdateDefault(computeObject);
            return frameUpdate;
        }

        /// <summary>
        /// 回傳數字鍵事件的畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        private FrameUpdate GetUpdateDefault(ComputeObject computeObject)
        {
            //小數點case
            if (Content.Equals('.'))
            {
                //若這時沒有值→視為已經輸入零
                if (computeObject.NumberField == null)
                {
                    computeObject.NumberField = new NumberField();
                    computeObject.NumberField.AddDigit(Content);
                    return new FrameUpdate("0.", removeLength: 0, updateString: "0.");
                }
                //若有值→判斷是否已為小數
                if (computeObject.NumberField.IsNumeric)
                {
                    //已經是小數→不回傳值
                    return new FrameUpdate(computeObject.NumberField.Number.ToString() + ".", removeLength: 0, updateString: string.Empty);
                }
                else
                {
                    //不是小數→更新小數點
                    computeObject.NumberField.AddDigit(Content);
                    return new FrameUpdate(computeObject.NumberField.Number.ToString() + ".", removeLength: 0, updateString: ".");
                }
            }

            //防呆
            if (!char.IsNumber(Content))
            {
                throw new Exception("該輸入數字");
            }

            //正常的數字case
            if (computeObject.NumberField == null)
            {
                computeObject.NumberField = new NumberField(Content);
            }
            else
            {
                computeObject.NumberField.AddDigit(Content);
            }
            return new FrameUpdate(computeObject.NumberField.Number.ToString(), removeLength: 0, updateString: Content.ToString());
        }
    }
}