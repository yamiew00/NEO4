using System.Collections.Generic;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public class BackSpace : IFeature
    {
        public BackSpace(int userid) : base(userid)
        {
            FeatureSet
            = new HashSet<Feature>() { Feature.NUMBER, Feature.BACKSPACE};
        }

        protected override FrameObject CreateFrameObject()
        {
            FrameUpdate frameUpdate = OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定。
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = frameUpdate.Refresh(FrameObject.Panel);

            return FrameObject;
        }

        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate = Tree();
            //執行成功時記錄下這次的Cast
            PreviousFeature = Feature.BACKSPACE;
            return frameUpdate;
        }

        protected override FrameUpdate Tree()
        {
            int RemoveLength = 0;
            if (NumberField == null)
            {
                return new FrameUpdate(removeLength: RemoveLength, updateString: string.Empty);
            }

            RemoveLength = NumberField.Number.Value.ToString().Length;
            if (NumberField.IsEndWithPoint())
            {
                RemoveLength += 1;
            }
            NumberField.ClearOneDigit();
            string updateString = NumberField.Number.ToString();
            if (NumberField.IsEndWithPoint())
            {
                updateString += ".";
            }
            return new FrameUpdate(RemoveLength, updateString);
        }
    }
}