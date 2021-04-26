using System;
using System.Collections.Generic;
using WebApi.Evaluate.Operators;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public enum UnaryRule
    {
        UNARY_SINGLE,
        UNARY_UNARY_COMBO
    }

    public class Unary : IFeature
    {
        private UnaryRule UnaryRule;

        public Unary(int userId, char content) : base(userId, content)
        {
            FeatureSet 
                = new HashSet<Feature>() { Feature.Null, Feature.NUMBER, Feature.BINARY, Feature.EQUAL, Feature.RIGHT_BRACKET, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY };
        }

        protected override FrameObject CreateFrameObject()
        {
            //OrderingChecker會回傳FrameUpdate
            FrameUpdate frameUpdate = OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定
            var subPanel = CompleteExpression;
            var panel = frameUpdate.Answer;

            return new FrameObject(subPanel, panel);
        }

        protected override FrameUpdate OrderingDealer()
        {
            Feature CurrentFeature = Feature.UNARY;
            
            FrameUpdate frameUpdate;

            //如果單元連按會有迭代的表現方式
            if (PreviousFeature == Feature.UNARY)
            {
                UnaryRule = UnaryRule.UNARY_UNARY_COMBO;
            }
            else
            {
                //非迭代的case
                UnaryRule = UnaryRule.UNARY_SINGLE;
            }

            frameUpdate = Tree();

            //執行成功時記錄下這次的Cast
            PreviousFeature = CurrentFeature;
            return frameUpdate;
        }

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

        private FrameUpdate UnarySingle()
        {
            //拿到單元運算的公式
            UnaryOperator unaryOperator = Setting.Operators.GetUnary(Content);
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
            return new FrameUpdate(NumberField.Number.ToString(),removeLength, updateString);
        }

        private FrameUpdate UnaryCombo()
        {
            //取得單元運算子
            UnaryOperator unaryOperator = Setting.Operators.GetUnary(Content);
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

    }
}