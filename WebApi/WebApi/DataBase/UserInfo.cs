using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public class UserInfo
    {
        //User-Related。
        public string CompleteExpression { get; set; }

        //User-Related
        public FrameObject FrameObject { get; set; }

        //User-Related
        public Feature PreviousFeature { get; set; }

        //User-Related。
        public NumberField NumberField { get; set; }

        //User-Related。
        public decimal CurrentAnswer { get; set; }

        //User-Related。
        public ExpressionTreeManager ExpressionTreeManager { get; set; }

        public string CurrentUnaryString { get; set; }

        public UserInfo()
        {
            Init();
        }

        public void Init()
        {
            CompleteExpression = string.Empty;
            FrameObject = new FrameObject();
            PreviousFeature = Feature.Null;
            NumberField = new NumberField();
            CurrentAnswer = 0;
            ExpressionTreeManager = new ExpressionTreeManager();
            CurrentUnaryString = string.Empty;
        }
    }
}