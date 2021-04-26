using System;
using System.Collections.Generic;
using WebApi.Evaluate;
using WebApi.Evaluate.Operators;
using WebApi.Evaluate.Tree;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public enum BinaryRule
    {
        ADD,
        MODIFY
    }

    public class Binary : IFeature
    {
        private BinaryRule BinaryRule;
        
        public Binary(int userid, char content) : base(userid, content)
        {
            FeatureSet
            = new HashSet<Feature>() { Feature.Null, Feature.NUMBER, Feature.BINARY,Feature.RIGHT_BRACKET, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY };
        }

        protected override FrameObject CreateFrameObject()
        {
            //OrderingChecker會回傳Update與要給Panel的numberString
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
            Feature CurrentFeature = Feature.BINARY;
            
            FrameUpdate frameUpdate;

            if (PreviousFeature == Feature.Null)
            {
                BinaryRule = BinaryRule.ADD;
                frameUpdate = Tree();
                //要補0
                frameUpdate.UpdateString = $"0{frameUpdate.UpdateString}";
            }
            else if (PreviousFeature == Feature.BINARY)
            {
                BinaryRule = BinaryRule.MODIFY;
                frameUpdate = Tree();
            }
            else
            {
                BinaryRule = BinaryRule.ADD;
                frameUpdate = Tree();
            }

            //執行成功時記錄下這次的Cast
            PreviousFeature = CurrentFeature;
            return frameUpdate;
        }

        protected override FrameUpdate Tree()
        {
            if (BinaryRule == BinaryRule.ADD)
            {
                BinaryOperator binaryOperator = Setting.Operators.GetBinary(Content);

                //若沒有輸入過值，則視為輸入了0
                ExpressionTreeManager.Add((NumberField == null) ? 0 : NumberField.Number.Value);

                //數字歸零
                NumberField = null;

                //算出一個暫時的結果並存下
                CurrentAnswer = ExpressionTreeManager.TryGetTmpResult();

                ExpressionTreeManager.Add(binaryOperator);

                return new FrameUpdate(CurrentAnswer.ToString(), removeLength: 0, updateString: Content.ToString());
            }
            else if (BinaryRule == BinaryRule.MODIFY)
            {
                BinaryOperator binaryOperator = Setting.Operators.GetBinary(Content);
                if (NumberField != null)
                {
                    throw new Exception("ModifyBinary時Number不能有值");
                }
                ExpressionTreeManager.Modify(binaryOperator);
                return new FrameUpdate(CurrentAnswer.ToString(),removeLength: 1, updateString: Content.ToString());
            }
            else
            {
                throw new Exception("BinaryRule錯誤");
            }
        }
    }
}