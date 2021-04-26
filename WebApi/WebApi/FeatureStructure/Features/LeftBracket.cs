using System.Collections.Generic;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public class LeftBracket : IFeature
    {

        public LeftBracket(int userid) : base(userid)
        {
            FeatureSet = new HashSet<Feature>() { Feature.Null, Feature.BINARY, Feature.EQUAL, Feature.LEFT_BRACKET, Feature.CLEAR, Feature.CLEAR_ERROR, Feature.BACKSPACE};
        }

        protected override FrameObject CreateFrameObject()
        {
            FrameUpdate frameUpdate = OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定。subpanel強制給0
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = "0";

            return FrameObject;
        }

        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate = Tree();

            if (PreviousFeature == Feature.EQUAL)
            {
                frameUpdate.RemoveLength = FrameUpdate.REMOVE_ALL;
            }

            //執行成功時記錄下這次的Cast
            PreviousFeature = Feature.LEFT_BRACKET;
            return frameUpdate;
        }

        protected override FrameUpdate Tree()
        {
            ExpressionTreeManager.LeftBracket();
            return new FrameUpdate(removeLength: 0, updateString: "(");
        }
    }
}