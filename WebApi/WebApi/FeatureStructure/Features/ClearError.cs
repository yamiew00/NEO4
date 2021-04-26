using System.Collections.Generic;
using WebApi.Evaluate.Tree;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public class ClearError : IFeature
    {
        public ClearError(int userid) : base(userid)
        {
            FeatureSet
            = new HashSet<Feature>() {  Feature.NUMBER, Feature.CLEAR_ERROR};
        }

        protected override FrameObject CreateFrameObject()
        {
            FrameUpdate expUpdate = OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = expUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定。
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = "0";

            return FrameObject;
        }

        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate = Tree();
            //執行成功時記錄下這次的Cast
            PreviousFeature = Feature.CLEAR_ERROR;
            return frameUpdate;
        }

        protected override FrameUpdate Tree()
        {
            if (NumberField == null)
            {
                return new FrameUpdate(removeLength: 0, updateString: string.Empty);
            }
            int length = NumberField.Number.Value.ToString().Length;
            NumberField = new NumberField();

            return new FrameUpdate(removeLength: length, updateString: "0");
        }
    }
}