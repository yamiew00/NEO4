using System;
using System.Collections.Generic;
using WebApi.DataBase;
using WebApi.Evaluate;
using WebApi.Evaluate.Operators;
using WebApi.Evaluate.Tree;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 雙元規則
    /// </summary>
    public enum BinaryRule
    {
        ADD_BINARY,
        MODIFY_BINARY
    }

    /// <summary>
    /// 雙元運算子:Concrete IFeature物件
    /// </summary>
    public class Binary : Feature
    {
        /// <summary>
        /// 雙元規則
        /// </summary>
        private BinaryRule BinaryRule;

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
        /// <param name="userid">用戶ID</param>
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
        /// 根據OrderingDealer方法的回傳值，製造畫面物件。
        /// </summary>
        /// <returns>畫面面件</returns>
        protected override FrameObject CreateFrameObject()
        {
            //OrderingChecker會回傳Update與要給Panel的numberString
            FrameUpdate frameUpdate = OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = frameUpdate.Answer;

            return FrameObject;
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
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate OrderingDealer()
        {            
            FrameUpdate frameUpdate;

            if (LastFeature == typeof(Clear))
            {
                BinaryRule = BinaryRule.ADD_BINARY;
                frameUpdate = Tree();
                //要補0
                frameUpdate.UpdateString = $"0{frameUpdate.UpdateString}";
            }
            else if (LastFeature == typeof(Binary))
            {
                BinaryRule = BinaryRule.MODIFY_BINARY;
                frameUpdate = Tree();
            }
            else
            {
                BinaryRule = BinaryRule.ADD_BINARY;
                frameUpdate = Tree();
            }
            
            return frameUpdate;
        }
        
        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate Tree()
        {
            if (BinaryRule == BinaryRule.ADD_BINARY)
            {
                BinaryOperator binaryOperator = BinaryDic[Content];

                //若沒有輸入過值，則視為輸入了0
                TreeStack.Peek().Add((NumberField == null) ? 0 : NumberField.Number.Value);

                //數字歸零
                NumberField = null;

                //算出一個暫時的結果並存下
                CurrentAnswer = GetTmpResult();
                
                TreeStack.Peek().Add(binaryOperator);

                return new FrameUpdate(CurrentAnswer.ToString(), removeLength: 0, updateString: Content.ToString());
            }
            else if (BinaryRule == BinaryRule.MODIFY_BINARY)
            {
                BinaryOperator binaryOperator = BinaryDic[Content];
                if (NumberField != null)
                {
                    throw new Exception("ModifyBinary時Number不能有值");
                }
                TreeStack.Peek().ModifyOperator(binaryOperator);
                return new FrameUpdate(CurrentAnswer.ToString(), removeLength: 1, updateString: Content.ToString());
            }
            else
            {
                throw new Exception("BinaryRule錯誤");
            }
        }

        /// <summary>
        /// 回傳新增物件的方法
        /// </summary>
        /// <returns>委派</returns>
        public override Func<char, Feature> Create()
        {
            return (content) => new Binary(content);
        }
    }
}