using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Evaluate.Tree;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 數字鍵:Concrete IFeature物件
    /// </summary>
    public class Number : IFeature
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userid">用戶id</param>
        /// <param name="content">功能內容</param>
        public Number(int userid, char content) : base(userid, content)
        {
        }

        /// <summary>
        /// 空建構子(必要)
        /// </summary>
        public Number()
        {

        }

        /// <summary>
        /// 根據OrderingDealer方法的回傳值，製造畫面物件。
        /// </summary>
        /// <returns>畫面面件</returns>
        protected override FrameObject CreateFrameObject()
        {
            FrameUpdate frameUpdate = OrderingDealer();
            string answer = frameUpdate.Answer;

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定
            FrameObject.SubPanel = CompleteExpression.Substring(0, CompleteExpression.Length - answer.Length);
            FrameObject.Panel = answer;
            return FrameObject;
        }

        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace), typeof(Unary) };
        }

        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace) };
        }

        /// <summary>
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate OrderingDealer()
        {
            Feature CurrentFeature = Feature.NUMBER;
            
            FrameUpdate frameUpdate;

            //等號後輸入數字
            if (PreviousFeature == Feature.EQUAL)
            {
                //必須要把之前的運算全部清空
                InfoInit();
                frameUpdate = Tree();

                frameUpdate.RemoveLength = FrameUpdate.REMOVE_ALL;

                //執行成功時記錄下這次的Cast
                PreviousFeature = CurrentFeature;
                return frameUpdate;
            }

            //backspace或clearerror之後的數字處理
            if ((PreviousFeature == Feature.BACKSPACE || PreviousFeature == Feature.CLEAR_ERROR) && NumberField.Number == 0)
            {
                frameUpdate = Tree();
                frameUpdate.RemoveLength += 1;

                //執行成功時記錄下這次的Cast
                PreviousFeature = CurrentFeature;
                return frameUpdate;
            }

            frameUpdate = Tree();
            //執行成功時記錄下這次的Cast
            PreviousFeature = CurrentFeature;
            return frameUpdate;
        }

        /// <summary>
        /// 設定實體物件的合法順序規則
        /// </summary>
        /// <returns>合法順序集</returns>
        protected override HashSet<Feature> SetPreviousFeatures()
        {
            return new HashSet<Feature>() { Feature.Null, Feature.NUMBER, Feature.BINARY, Feature.EQUAL, Feature.LEFT_BRACKET, Feature.CLEAR, Feature.CLEAR_ERROR, Feature.BACKSPACE };
        }

        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate Tree()
        {
            //小數點case
            if (Content.Equals('.'))
            {
                //若這時沒有值→視為已經輸入零
                if (NumberField == null)
                {
                    NumberField = new NumberField();
                    NumberField.AddDigit(Content);
                    return new FrameUpdate("0.", removeLength: 0, updateString: "0.");
                }
                //若有值→判斷是否已為小數
                if (NumberField.IsNumeric)
                {
                    //已經是小數→不回傳值
                    return new FrameUpdate(NumberField.Number.ToString() + ".", removeLength: 0, updateString: string.Empty);
                }
                else
                {
                    //不是小數→更新小數點
                    NumberField.AddDigit(Content);
                    return new FrameUpdate(NumberField.Number.ToString() + ".", removeLength: 0, updateString: ".");
                }
            }

            //防呆
            if (!char.IsNumber(Content))
            {
                throw new Exception("該輸入數字");
            }

            //正常的數字case
            if (NumberField == null)
            {
                NumberField = new NumberField(Content);
            }
            else
            {
                NumberField.AddDigit(Content);
            }
            return new FrameUpdate(NumberField.Number.ToString(), removeLength: 0, updateString: Content.ToString());
        }
        
    }
}