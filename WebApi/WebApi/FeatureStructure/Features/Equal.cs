using System;
using System.Collections.Generic;
using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public enum EqualRule
    {
        EQUAL_SINGLE, 
        EQUAL_EQUAL_COMBO,
        BINARY_EQUAL_COMBO
    }

    public class Equal: IFeature
    {
        private EqualRule EqualRule;
        
        public Equal(int userid) : base(userid)
        {
            FeatureSet 
                = new HashSet<Feature>() { Feature.Null, Feature.NUMBER, Feature.BINARY, Feature.EQUAL, Feature.RIGHT_BRACKET, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY };
        }

        protected override FrameObject CreateFrameObject()
        {
            FrameUpdate frameUpdate = OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = frameUpdate.Answer;

            return FrameObject;
        }

        protected override FrameUpdate OrderingDealer()
        {
            Feature CurrentFeature = Feature.EQUAL;
            
            FrameUpdate frameUpdate;
            if (PreviousFeature == Feature.Null)
            {
                return new FrameUpdate("0", removeLength: 0, updateString: "0=");
            }
            else if (PreviousFeature == Feature.EQUAL)
            {
                //等號連按
                EqualRule = EqualRule.EQUAL_EQUAL_COMBO;
                frameUpdate = Tree();

                //執行成功時記錄下這次的Cast
                PreviousFeature = CurrentFeature;
                return frameUpdate;
            }
            else if (PreviousFeature == Feature.BINARY)
            {
                //運算符配等號
                EqualRule = EqualRule.BINARY_EQUAL_COMBO;
                frameUpdate = Tree();

                //執行成功時記錄下這次的Cast
                PreviousFeature = CurrentFeature;
                return frameUpdate;
            }
            EqualRule = EqualRule.EQUAL_SINGLE;
            frameUpdate = Tree();

            //執行成功時記錄下這次的Cast
            PreviousFeature = CurrentFeature;
            return frameUpdate;
        }

        protected override FrameUpdate Tree()
        {
            if (EqualRule == EqualRule.EQUAL_SINGLE)
            {
                return EqualSingle();
            }
            else if (EqualRule == EqualRule.EQUAL_EQUAL_COMBO)
            {
                return EqualCombo();
            }
            else if (EqualRule == EqualRule.BINARY_EQUAL_COMBO)
            {
                return BinaryEqualCombo();
            }
            else
            {
                throw new Exception("EqualRule錯誤");
            }
        }

        private FrameUpdate EqualSingle()
        {
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

            //暫存答案
            CurrentAnswer = ans;
            return new FrameUpdate(ans.ToString(),removeLength: 0, updateString: updateString);
        }

        private FrameUpdate EqualCombo()
        {
            string updateString = CurrentAnswer.ToString();

            //更新計算答案
            ExpressionTreeManager.ReplaceLeft(CurrentAnswer);
            var op = ExpressionTreeManager.GetTop().NodeValue.Operator.Name;
            var rightResult = ExpressionTreeManager.GetRightHalf();
            updateString += $"{op}{rightResult}=";
            CurrentAnswer = ExpressionTreeManager.TryGetResult().Answer;
            return new FrameUpdate(CurrentAnswer.ToString(), removeLength: FrameUpdate.REMOVE_ALL, updateString: updateString);
        }

        private FrameUpdate BinaryEqualCombo()
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

            return new FrameUpdate(ans.ToString(), removeLength: 0, updateString: updateString);
        }
    }
}