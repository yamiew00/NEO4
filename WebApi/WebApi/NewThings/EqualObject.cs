using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DataBase;
using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;
using WebApi.Setting;
using static WebApi.Setting.FeatureRule;

namespace WebApi.NewThings
{
    public class EqualObject
    {
        public char Content;

        public int UserId;

        private HashSet<Feature> FeatureSet
            = new HashSet<Feature>() { Feature.Null, Feature.NUMBER, Feature.BINARY, Feature.EQUAL, Feature.RIGHT_BRACKET, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY };

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

        public EqualObject(int userid, char content)
        {
            Content = content;
            UserId = userid;
        }

        //factory層
        public FrameObject Layer1()
        {
            FrameUpdate frameUpdate = Layer2();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定
            var subPanel = CompleteExpression;
            var panel = frameUpdate.Answer;

            return new FrameObject(subPanel, panel);
        }

        //order層
        public FrameUpdate Layer2()
        {
            Feature CurrentFeature = Feature.EQUAL;

            if (FeatureSet.Contains(PreviousFeature))
            {
                FrameUpdate frameUpdate;
                if (PreviousFeature == Feature.Null)
                {
                    return new FrameUpdate("0", new ExpUpdate(removeLength: 0, updateString: "0="));
                }
                else if (PreviousFeature == Feature.EQUAL)
                {
                    //這個怎麼辦
                    frameUpdate = Layer3_equalCombo();
                    //執行成功時記錄下這次的Cast
                    PreviousFeature = CurrentFeature;
                    return frameUpdate;
                }
                else if (PreviousFeature == Feature.BINARY)
                {
                    frameUpdate = Layer3_binaryAndEqualCombo();
                    //執行成功時記錄下這次的Cast
                    PreviousFeature = CurrentFeature;
                    return frameUpdate;
                }
                frameUpdate = Layer3_equal();

                //執行成功時記錄下這次的Cast
                PreviousFeature = CurrentFeature;
                return frameUpdate;
            }
            else
            {
                throw new OrderException(FeatureRule.ORDER_EXCEPTION_MSG);
            }
        }

        public FrameUpdate Layer3_equal()
        {
            //這裡的case應該還有更多
            if (NumberField != null)
            {
                ExpressionTreeManager.Add(NumberField.Number.Value);
                NumberField = null;
            }

            //result處理
            var result = ExpressionTreeManager.TryGetResult();
            var updateString = string.Empty;
            for (int i = 0; i < result.ExtraRightBracketCount; i++)
            {
                updateString += ")";
            }

            //送出結果
            updateString += "=";
            var ans = result.Answer;
            CurrentAnswer = ans;
            return new FrameUpdate(ans.ToString(), new ExpUpdate(removeLength: 0, updateString: updateString));
        }

        public FrameUpdate Layer3_equalCombo()
        {
            string updateString = CurrentAnswer.ToString();

            //更新計算答案
            CurrentAnswer = ExpressionTreeManager.GetLastTreeReplaceLeftResult(CurrentAnswer);
            var op = ExpressionTreeManager.GetLastTreeTopOperator();
            updateString += op;

            var rightNumber = ExpressionTreeManager.GetLastTreeRightResult();
            updateString += rightNumber.ToString();

            updateString += "=";

            return new FrameUpdate(CurrentAnswer.ToString(), new ExpUpdate(removeLength: ExpUpdate.REMOVE_ALL, updateString: updateString));
        }

        public FrameUpdate Layer3_binaryAndEqualCombo()
        {
            //CurrentAnswer已經被Binary算好了
            ExpressionTreeManager.Add(CurrentAnswer);

            //result處理
            var result = ExpressionTreeManager.TryGetResult();
            var ans = result.Answer;

            //做更新字串
            var updateString = CurrentAnswer.ToString();
            for (int i = 0; i < result.ExtraRightBracketCount; i++)
            {
                updateString += ")";
            }

            //送出結果
            updateString += "=";

            return new FrameUpdate(ans.ToString(), new ExpUpdate(removeLength: 0, updateString: updateString));
        }
    }
}