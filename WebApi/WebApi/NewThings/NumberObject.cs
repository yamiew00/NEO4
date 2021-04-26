using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DataBase;
using WebApi.Evaluate;
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
    public class NumberObject
    {
        public char Content;

        public int UserId;

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


        public NumberObject(int userid, char content)
        {
            Content = content;
            UserId = userid;
        }

        //factory層
        public FrameObject Layer1()
        {
            //OrderingChecker會回傳FrameUpdate
            //FrameUpdate frameUpdate = OrderingChecker.AddNumber(Content);
            FrameUpdate frameUpdate = Layer2();
            string answer = frameUpdate.Answer;

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定
            var subPanel = CompleteExpression.Substring(0, CompleteExpression.Length - answer.Length);
            var panel = answer;
            return new FrameObject(subPanel, panel);
        }

        private HashSet<Feature> FeatureSet 
            = new HashSet<Feature>() { Feature.Null,  Feature.NUMBER, Feature.BINARY, Feature.EQUAL, Feature.LEFT_BRACKET, Feature.CLEAR, Feature.CLEAR_ERROR, Feature.BACKSPACE };

        //order層
        public FrameUpdate Layer2()
        {
            Feature CurrentFeature = Feature.NUMBER;

            if (FeatureSet.Contains(PreviousFeature))
            {
                //FrameUpdate frameUpdate = NumberMachine.AddNumber(Content);
                FrameUpdate frameUpdate = Layer3();

                //等號後輸入數字
                if (PreviousFeature == Feature.EQUAL)
                {
                    frameUpdate.RemoveLength = ExpUpdate.REMOVE_ALL;

                    //執行成功時記錄下這次的Cast
                    PreviousFeature = CurrentFeature;
                    return frameUpdate;
                }

                //backspace或clearerror之後的數字處理
                if ((PreviousFeature == Feature.BACKSPACE || PreviousFeature == Feature.CLEAR_ERROR) && NumberField.Number == 0)
                {
                    frameUpdate.RemoveLength += 1;

                    //執行成功時記錄下這次的Cast
                    PreviousFeature = CurrentFeature;
                    return frameUpdate;
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

        //numbermachine層
        public FrameUpdate Layer3()
        {
            //小數點case
            if (Content.Equals('.'))
            {
                //若這時沒有值→視為已經輸入零
                if (NumberField == null)
                {
                    NumberField = new NumberField();
                    NumberField.AddDigit(Content);
                    return new FrameUpdate("0.", new ExpUpdate(removeLength: 0, updateString: "0."));
                }
                //若有值→判斷是否已為小數
                if (NumberField.IsNumeric)
                {
                    //已經是小數→不回傳值
                    return new FrameUpdate(NumberField.Number.ToString() + ".", new ExpUpdate(removeLength: 0, updateString: string.Empty));
                }
                else
                {
                    //不是小數→更新小數點
                    NumberField.AddDigit(Content);
                    return new FrameUpdate(NumberField.Number.ToString() + ".", new ExpUpdate(removeLength: 0, updateString: "."));
                }
            }

            //防呆
            if (!char.IsNumber(Content))
            {
                throw new Exception("該輸入數字");
            }

            //正常的數字case
            if (NumberField == null)
            {
                NumberField = new NumberField(Content);
            }
            else
            {
                NumberField.AddDigit(Content);
            }
            return new FrameUpdate(NumberField.Number.ToString(), new ExpUpdate(removeLength: 0, updateString: Content.ToString()));
        }
        
    }
}