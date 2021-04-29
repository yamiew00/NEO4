using System;
using System.Collections.Generic;
using WebApi.DataBase;
using WebApi.Evaluate;
using WebApi.Evaluate.Operators;
using WebApi.Evaluate.Tree;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 雙元運算子:Concrete IFeature物件
    /// </summary>
    public class Binary : Feature
    {
        /// <summary>
        /// 二元運算字典
        /// </summary>
        private static Dictionary<char, BinaryOperator> BinaryDic = new Dictionary<char, BinaryOperator>()
        {
            { '+', new BinaryOperator(1, (num1, num2) => num1 + num2, '+')},
            { '-', new BinaryOperator(1, (num1, num2) => num1 - num2, '-')},
            { 'x', new BinaryOperator(2, (num1, num2) => num1 * num2, 'x')},
            { '÷', new BinaryOperator(2, (num1, num2) => num1 / num2, '÷')},
        };
        
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="content">功能內容</param>
        public Binary(char content) : base(content)
        {
        }

        /// <summary>
        /// 空建構子。反射用的
        /// </summary>
        public Binary()
        {
        }
        
        /// <summary>
        /// 回傳此功能後面可以接的功能集
        /// </summary>
        /// <returns>後面可以接的功能集</returns>
        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(RightBracket), typeof(Clear), typeof(Unary) };
        }

        /// <summary>
        /// 回傳此功能前面可以接的功能集
        /// </summary>
        /// <returns>前面可以接的功能集</returns>
        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(RightBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace), typeof(Unary) };
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
            if (computeObject.LastFeature == typeof(Clear))
            {
                var frameUpdate = GetUpdateDefault(computeObject);
                //要補0
                frameUpdate.UpdateString = $"0{frameUpdate.UpdateString}";
                return frameUpdate;
            }
            else if (computeObject.LastFeature == typeof(Binary))
            {
                return GetUpdateAfterBinary(computeObject);
            }
            else
            {
                return GetUpdateDefault(computeObject);
            }
        }

        /// <summary>
        /// 回傳預設雙元運算子事件的畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        private FrameUpdate GetUpdateDefault(ComputeObject computeObject)
        {
            BinaryOperator binaryOperator = BinaryDic[Content];

            //若沒有輸入過值，則視為輸入了0
            computeObject.TreeStack.Peek().Add((computeObject.NumberField == null) ? 0 : computeObject.NumberField.Number.Value);

            //數字歸零
            computeObject.NumberField = null;

            //算出一個暫時的結果並存下
            computeObject.CurrentAnswer = GetTmpResult(computeObject);

            computeObject.TreeStack.Peek().Add(binaryOperator);

            return new FrameUpdate(computeObject.CurrentAnswer.ToString(), removeLength: 0, updateString: Content.ToString());
        }

        /// <summary>
        /// 回傳連續輸入雙元運算子的畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        private FrameUpdate GetUpdateAfterBinary(ComputeObject computeObject)
        {
            BinaryOperator binaryOperator = BinaryDic[Content];
            if (computeObject.NumberField != null)
            {
                throw new Exception("ModifyBinary時Number不能有值");
            }
            computeObject.TreeStack.Peek().ModifyOperator(binaryOperator);
            return new FrameUpdate(computeObject.CurrentAnswer.ToString(), removeLength: 1, updateString: Content.ToString());
        }
    }
}