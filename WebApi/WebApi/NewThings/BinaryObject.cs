using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DataBase;
using WebApi.Evaluate;
using WebApi.Evaluate.Operators;
using WebApi.Evaluate.Tree;
using WebApi.Exceptions;
using WebApi.Frames;
using WebApi.Models;
using WebApi.Models.Request;
using WebApi.Models.Response;
using WebApi.Setting;
using static WebApi.Setting.FeatureRule;

namespace WebApi.NewThings
{
    public class BinaryObject
    {
        public char Content;

        public int UserId;

        private HashSet<Feature> FeatureSet
            = new HashSet<Feature>() { Feature.Null, Feature.NUMBER, Feature.BINARY, Feature.LEFT_BRACKET, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY };

        //User-Related。
        public string CompleteExpression
        {
            get
            {
                return Users.GetCompleteExpression(UserId);
            }

            set
            {
                Users.SetCompleteExpression(UserId, value);
            }
        }

        //User-Related
        public Feature PreviousFeature
        {
            get
            {
                return Users.GetPreviousFeature(UserId);
            }
            set
            {
                Users.SetPreviousFeature(UserId, value);
            }
        }

        //User-Related。
        public NumberField NumberField
        {
            get
            {
                return Users.GetNumberField(UserId);
            }

            set
            {
                Users.SetNumberField(UserId, value);
            }
        }

        //User-Related。
        public decimal CurrentAnswer
        {
            get
            {
                return Users.GetCurrentAnswer(UserId);
            }

            set
            {
                Users.SetCurrentAnswer(UserId, value);
            }
        }

        //User-Related。
        public ExpressionTreeManager ExpressionTreeManager
        {
            get
            {
                return Users.GetExpressionTreeManager(UserId);
            }
        }

        public BinaryObject(int userid, char content)
        {
            Content = content;
            UserId = userid;
        }

        public FrameObject Layer1()
        {
            //OrderingChecker會回傳Update與要給Panel的numberString
            //FrameUpdate frameUpdate = OrderingChecker.AddBinary(Content);
            FrameUpdate frameUpdate = Layer2();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定
            var subPanel = CompleteExpression;
            var panel = frameUpdate.Answer;

            return new FrameObject(subPanel, panel);
        }

        //order層, throw exception要處理
        public FrameUpdate Layer2()
        {
            Feature CurrentFeature = Feature.BINARY;

            if (FeatureSet.Contains(PreviousFeature))
            {
                FrameUpdate frameUpdate;

                if (PreviousFeature == Feature.Null)
                {
                    frameUpdate = Layer3_add();
                    //要補0
                    frameUpdate.UpdateString = $"0{frameUpdate.UpdateString}";
                }
                else if (PreviousFeature == Feature.BINARY)
                {
                    frameUpdate = Layer3_modify();
                }
                else
                {
                    frameUpdate = Layer3_add();
                }

                //執行成功時記錄下這次的Cast
                PreviousFeature = CurrentFeature;
                return frameUpdate;
            }
            else
            {
                throw new OrderException(FeatureRule.ORDER_EXCEPTION_MSG);
            }
        }

        public FrameUpdate Layer3_add()
        {
            BinaryOperator binaryOperator = Setting.Operators.GetBinary(Content);
            
            //若沒有輸入過值，則視為輸入了0
            ExpressionTreeManager.Add((NumberField == null) ? 0 : NumberField.Number.Value);
                
            //數字歸零
            NumberField = null;

            //算出一個暫時的結果並存下
            CurrentAnswer = ExpressionTreeManager.TryGetTmpResult();

            ExpressionTreeManager.Add(binaryOperator);
            
            return new FrameUpdate(CurrentAnswer.ToString(), new ExpUpdate(removeLength: 0, updateString: Content.ToString()));
        }

        public FrameUpdate Layer3_modify()
        {
            BinaryOperator binaryOperator = Setting.Operators.GetBinary(Content);
            if (NumberField != null)
            {
                throw new Exception("ModifyBinary時Number不能有值");
            }
            ExpressionTreeManager.Modify(binaryOperator);
            return new FrameUpdate(CurrentAnswer.ToString(), new ExpUpdate(removeLength: 1, updateString: Content.ToString()));
        }
    }
}