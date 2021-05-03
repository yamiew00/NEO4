using System;
using System.Collections.Generic;
using WebApi.Evaluate.Operators;
using WebApi.Exceptions;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 單元運算子:Concrete IFeature物件
    /// </summary>
    public class Unary : Feature
    {
        /// <summary>
        /// 單元運算字典
        /// </summary>
        private static Dictionary<char, UnaryOperator> UnaryDic = new Dictionary<char, UnaryOperator>()
        {
            {'±', new UnaryOperator((num) => -1 * num, '±') },
            {'√', new UnaryOperator((num) => (decimal)Math.Pow((double) num, 0.5), '√')}
        };

        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        private static readonly HashSet<Type> UNARY_PREVIOUS_TYPE = new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace), typeof(Unary) };

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        private static readonly HashSet<Type> UNARY_AFTERWARD_TYPE = new HashSet<Type>() { typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(Unary) };

        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        public override HashSet<Type> PreviousType => UNARY_PREVIOUS_TYPE;

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        public override HashSet<Type> AfterWardType => UNARY_AFTERWARD_TYPE;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="content">功能內容</param>
        public Unary(char content) : base(content)
        {
        }

        /// <summary>
        /// 空建構子。反射用的
        /// </summary>
        public Unary()
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
            //完整運算式的刷新
            boardObject.CompleteExpression = frameUpdate.Refresh(boardObject.CompleteExpression);

            //panel, subpanel設定
            boardObject.FrameObject.SubPanel = boardObject.CompleteExpression;
            boardObject.FrameObject.Panel = frameUpdate.Answer;

            return boardObject.FrameObject;
        }

        /// <summary>
        /// 依計畫內容回傳畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        public override FrameUpdate Compute(ComputeObject computeObject)
        {
            //如果單元連按會有迭代的表現方式
            if (computeObject.LastFeature == typeof(Unary))
            {
                return GetUpdateAfterUnary(computeObject);
            }
            else
            {
                //非迭代的case
                return GetUpdateDefault(computeObject);
            }
        }

        /// <summary>
        /// 連按單元運算子的畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        private FrameUpdate GetUpdateAfterUnary(ComputeObject computeObject)
        {
            //取得單元運算子
            UnaryOperator unaryOperator = UnaryDic[Content];
            var formula = unaryOperator.Formula;

            int removeLength = computeObject.CurrentUnaryString.Length;

            //必須case by case處理
            if (Content == '√')
            {
                computeObject.CurrentUnaryString = $"√({computeObject.CurrentUnaryString})";
                if (computeObject.NumberField.Number.HasValue && computeObject.NumberField.Number.Value <= 0)
                {
                    throw new SquareRootException("無效的輸入");
                }
            }
            else if (Content == '±')
            {
                computeObject.CurrentUnaryString = $"negate({computeObject.CurrentUnaryString})";
            }
            else
            {
                throw new Exception("無此運算元");
            }

            //計算後將結果存起來
            computeObject.NumberField.Number = formula(computeObject.NumberField.Number.Value);

            return new FrameUpdate(computeObject.NumberField.Number.ToString(), removeLength, computeObject.CurrentUnaryString);
        }

        /// <summary>
        /// 回傳預設的單元運算子事件的畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        private FrameUpdate GetUpdateDefault(ComputeObject computeObject)
        {
            //拿到單元運算的公式
            UnaryOperator unaryOperator = UnaryDic[Content];
            var formula = unaryOperator.Formula;

            int removeLength = 0;
            if (computeObject.NumberField == null)
            {
                return new FrameUpdate("0", removeLength: removeLength, updateString: string.Empty);
            }

            removeLength = computeObject.NumberField.Number.Value.ToString().Length;
            if (computeObject.NumberField.IsEndWithPoint())
            {
                removeLength += 1;
            }

            //暫存
            var OriginNumberString = computeObject.NumberField.Number.ToString();
            string updateString = string.Empty;

            //必須case by case處理
            if (Content == '√')
            {
                updateString = $"√({OriginNumberString})";
            }
            else if (Content == '±')
            {
                updateString = $"negate({OriginNumberString})";
            }
            else
            {
                throw new Exception("無此運算元");
            }

            //計算後將結果存起來
            computeObject.NumberField.Number = formula(computeObject.NumberField.Number.Value);

            //記住這次的結果
            computeObject.CurrentUnaryString = updateString;
            return new FrameUpdate(computeObject.NumberField.Number.ToString(), removeLength, updateString);
        }
    }
}