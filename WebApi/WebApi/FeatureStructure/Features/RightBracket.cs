using System.Collections.Generic;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public class RightBracket : IFeature
    {
        public RightBracket(int userid) : base(userid)
        {
            FeatureSet = new HashSet<Feature>() { Feature.NUMBER, Feature.BINARY, Feature.LEFT_BRACKET, Feature.RIGHT_BRACKET, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY };
        }

        protected override FrameObject CreateFrameObject()
        {
            FrameUpdate frameUpdate = OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定。
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = frameUpdate.Answer;

            return FrameObject;
        }

        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate = Tree();
            //執行成功時記錄下這次的Cast
            PreviousFeature = Feature.RIGHT_BRACKET;
            return frameUpdate;
        }

        protected override FrameUpdate Tree()
        {
            string updateString = string.Empty;

            if (NumberField != null)
            {
                ExpressionTreeManager.Add(NumberField.Number.Value);
            }
            else if (NumberField == null)
            {
                ExpressionTreeManager.Add(CurrentAnswer);
                updateString += CurrentAnswer.ToString();
            }

            ExpressionTreeManager.RightBracket();
            updateString += ")";

            NumberField = null;

            CurrentAnswer = ExpressionTreeManager.TryGetTmpResult();

            return new FrameUpdate(CurrentAnswer.ToString(), removeLength: 0, updateString: updateString);
        }
    }
}