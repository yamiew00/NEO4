using WebApi.DataBase;
using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 功能種類
    /// </summary>
    public enum Feature
    {
        Null,
        NUMBER,
        BINARY,
        EQUAL,
        LEFT_BRACKET,
        RIGHT_BRACKET,
        CLEAR,
        CLEAR_ERROR,
        BACKSPACE,
        UNARY
    }

    public abstract class FeatureObject
    {
        protected int UserId { get; set; }

        protected char Content { get; set; }

        private UserInfo UserInfo;

        //User-Related。
        protected string CompleteExpression
        {
            get
            {
                return UserInfo.CompleteExpression;
            }

            set
            {
                UserInfo.CompleteExpression = value;
            }
        }

        protected FrameObject FrameObject
        {
            get
            {
                return UserInfo.FrameObject;
            }

            set
            {
                UserInfo.FrameObject = value;
            }
        }

        //User-Related
        protected Feature PreviousFeature
        {
            get
            {
                return UserInfo.PreviousFeature;
            }

            set
            {
                UserInfo.PreviousFeature = value;
            }
        }

        //User-Related。
        protected NumberField NumberField
        {
            get
            {
                return UserInfo.NumberField;
            }

            set
            {
                UserInfo.NumberField = value;
            }
        }

        //User-Related。
        protected decimal CurrentAnswer
        {
            get
            {
                return UserInfo.CurrentAnswer;
            }

            set
            {
                UserInfo.CurrentAnswer = value;
            }
        }

        //User-Related。
        protected ExpressionTreeManager ExpressionTreeManager
        {
            get
            {
                return UserInfo.ExpressionTreeManager;
            }

            set
            {
                UserInfo.ExpressionTreeManager = value;
            }
        }

        protected string CurrentUnaryString
        {
            get
            {
                return UserInfo.CurrentUnaryString;
            }

            set
            {
                UserInfo.CurrentUnaryString = value;
            }
        }

        protected FeatureObject(int userId)
        {
            UserId = userId;
            UserInfo = Users.GetUserInfoDic(userId);
        }

        protected FeatureObject(int userId, char content) : this(userId)
        {
            Content = content;
        }

        protected void InfoInit()
        {
            UserInfo.Init();
        }
    }
}