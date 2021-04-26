using System.Collections.Generic;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public class Clear : IFeature
    {
        public Clear(int userid) : base(userid)
        {
            FeatureSet = new HashSet<Feature>()
            {
               Feature.NUMBER, Feature.BINARY, Feature.EQUAL, Feature.LEFT_BRACKET, Feature.RIGHT_BRACKET, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY
            };
        }

        protected override FrameObject CreateFrameObject()
        {
            OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = string.Empty;

            //panel, subpanel設定。
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = "0";

            return FrameObject;
        }

        protected override FrameUpdate OrderingDealer()
        {
            var frameUpdate = Tree();
            //執行成功時記錄下這次的Cast
            PreviousFeature = Feature.Null;
            return frameUpdate;
        }

        protected override FrameUpdate Tree()
        {
            //全資訊初始化
            InfoInit();
            return new FrameUpdate(FrameUpdate.REMOVE_ALL, string.Empty);
        }
    }
}