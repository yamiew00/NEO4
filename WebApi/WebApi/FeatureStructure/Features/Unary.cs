using System;
using System.Collections.Generic;
using WebApi.Evaluate.Operators;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 單元運算規則
    /// </summary>
    public enum UnaryRule
    {
        UNARY_SINGLE,
        UNARY_UNARY_COMBO
    }

    /// <summary>
    /// 單元運算子:Concrete IFeature物件
    /// </summary>
    public class Unary : Feature
    {
        /// <summary>
        /// 單元運算規則
        /// </summary>
        private UnaryRule UnaryRule;

        /// <summary>
        /// 單元運算字典
        /// </summary>
        private static Dictionary<char, UnaryOperator> UnaryDic = new Dictionary<char, UnaryOperator>()
        {
            {'±', new UnaryOperator((num) => -1 * num, '±') },
            {'√', new UnaryOperator((num) => (decimal)Math.Pow((double) num, 0.5), '√')}
        };

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userId">用戶id</param>
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
        /// 根據OrderingDealer方法的回傳值，製造畫面物件。
        /// </summary>
        /// <returns>畫面面件</returns>
        protected override FrameObject CreateFrameObject()
        {
            //OrderingChecker會回傳FrameUpdate
            FrameUpdate frameUpdate = OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = frameUpdate.Answer;

            return FrameObject;
        }

        /// <summary>
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate;

            //如果單元連按會有迭代的表現方式
            if (LastFeature == typeof(Unary))
            {
                UnaryRule = UnaryRule.UNARY_UNARY_COMBO;
            }
            else
            {
                //非迭代的case
                UnaryRule = UnaryRule.UNARY_SINGLE;
            }

            frameUpdate = Tree();
            return frameUpdate;
        }

        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate Tree()
        {
            if (UnaryRule == UnaryRule.UNARY_SINGLE)
            {
                return UnarySingle();
            }
            else if (UnaryRule == UnaryRule.UNARY_UNARY_COMBO)
            {
                return UnaryCombo();
            }
            else
            {
                throw new Exception("UnaryRule錯誤");
            }
        }

        /// <summary>
        /// 對單一數字，做第一次的單元運算
        /// </summary>
        /// <returns>畫面更新</returns>
        private FrameUpdate UnarySingle()
        {
            //拿到單元運算的公式
            UnaryOperator unaryOperator = UnaryDic[Content];
            var formula = unaryOperator.Formula;

            int removeLength = 0;
            if (NumberField == null)
            {
                return new FrameUpdate("0", removeLength: removeLength, updateString: string.Empty);
            }

            removeLength = NumberField.Number.Value.ToString().Length;
            if (NumberField.IsEndWithPoint())
            {
                removeLength += 1;
            }

            //暫存
            var OriginNumberString = NumberField.Number.ToString();
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
            NumberField.Number = formula(NumberField.Number.Value);

            //記住這次的結果
            CurrentUnaryString = updateString;
            return new FrameUpdate(NumberField.Number.ToString(), removeLength, updateString);
        }

        /// <summary>
        /// 連續做單元運算
        /// </summary>
        /// <returns>畫面更新</returns>
        private FrameUpdate UnaryCombo()
        {
            //取得單元運算子
            UnaryOperator unaryOperator = UnaryDic[Content];
            var formula = unaryOperator.Formula;

            int removeLength = CurrentUnaryString.Length;

            //必須case by case處理
            if (Content == '√')
            {
                CurrentUnaryString = $"√({CurrentUnaryString})";
                if (NumberField.Number.HasValue && NumberField.Number.Value <= 0)
                {
                    throw new SquareRootException("無效的輸入");
                }
            }
            else if (Content == '±')
            {
                CurrentUnaryString = $"negate({CurrentUnaryString})";
            }
            else
            {
                throw new Exception("無此運算元");
            }

            //計算後將結果存起來
            NumberField.Number = formula(NumberField.Number.Value);

            return new FrameUpdate(NumberField.Number.ToString(), removeLength, CurrentUnaryString);
        }

        /// <summary>
        /// 回傳此功能前面可以接的功能集
        /// </summary>
        /// <returns>前面可以接的功能集</returns>
        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace), typeof(Unary) };
        }

        /// <summary>
        /// 回傳此功能後面可以接的功能集
        /// </summary>
        /// <returns>後面可以接的功能集</returns>
        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(Unary) };
        }

        /// <summary>
        /// 回傳新增物件的方法
        /// </summary>
        /// <returns>委派</returns>
        public override Func<char, Feature> Create()
        {
            return (content) => new Unary(content);
        }
    }
}