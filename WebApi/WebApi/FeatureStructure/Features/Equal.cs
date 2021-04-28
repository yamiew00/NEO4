using System;
using System.Collections.Generic;
using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Evaluate.Utils;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 等號規則
    /// </summary>
    public enum EqualRule
    {
        EQUAL_SINGLE, 
        EQUAL_EQUAL_COMBO,
        BINARY_EQUAL_COMBO
    }

    /// <summary>
    /// 等號鍵:Concrete IFeature物件
    /// </summary>
    public class Equal : IFeature
    {
        /// <summary>
        /// 等號規則
        /// </summary>
        private EqualRule EqualRule;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userid">用戶id</param>
        public Equal(int userid) : base(userid)
        {
        }

        /// <summary>
        /// 空建構子(必要)
        /// </summary>
        public Equal()
        {

        }

        /// <summary>
        /// 根據OrderingDealer方法的回傳值，製造畫面物件。
        /// </summary>
        /// <returns>畫面面件</returns>
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

        /// <summary>
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
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

        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
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

        /// <summary>
        /// 單等號(前次功能不是等號或者是運算符)
        /// </summary>
        /// <returns>畫面更新</returns>
        private FrameUpdate EqualSingle()
        {
            if (NumberField != null)
            {
                TreeStack.Peek().Add(NumberField.Number.Value);
                NumberField = null;
            }

            //result處理
            var result = GetResult();
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
            return new FrameUpdate(ans.ToString(), removeLength: 0, updateString: updateString);
        }

        /// <summary>
        /// 等號連擊
        /// </summary>
        /// <returns>畫面更新</returns>
        private FrameUpdate EqualCombo()
        {
            string updateString = CurrentAnswer.ToString();

            //更新計算答案
            ReplaceLeft(CurrentAnswer);
            var op = TreeStack.Peek().GetTop().NodeValue.Operator.Name;
            var rightResult = GetRightHalf();

            updateString += $"{op}{rightResult}=";
            CurrentAnswer = GetResult().Answer;
            return new FrameUpdate(CurrentAnswer.ToString(), removeLength: FrameUpdate.REMOVE_ALL, updateString: updateString);
        }

        /// <summary>
        /// 以一個數字替換Tree的左半部
        /// </summary>
        /// <param name="newNumber">要更新的數字</param>
        private void ReplaceLeft(decimal newNumber)
        {
            //在這個方法被呼叫之前，TreeStack已經收束成一棵完整的樹了，也就是TreeStack.Count = 1
            var tree = TreeStack.Peek();
            var top = tree.GetTop();
            
            //新的左節點
            Node newLeftNode = Node.Build()
                       .SetParentNode(top)
                       .SetNumber(newNumber)
                       .Exec();
            top.LeftNode = newLeftNode;
            tree.Root = newLeftNode;
        }

        /// <summary>
        /// 回傳Tree右半部的計算結果
        /// </summary>
        /// <returns>右半的計算結果</returns>
        private decimal GetRightHalf()
        {
            var top = TreeStack.Peek().GetTop();
            if (top.RightNode == null)
            {
                return 0;
            }

            return new Evaluator().PackNode(top.RightNode);
        }

        /// <summary>
        /// 運算符接等號
        /// </summary>
        /// <returns>畫面更新</returns>
        private FrameUpdate BinaryEqualCombo()
        {
            //CurrentAnswer已經被Binary算好了
            TreeStack.Peek().Add(CurrentAnswer);

            //result處理
            var result = GetResult();
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

        /// <summary>
        /// 設定實體物件的合法順序規則
        /// </summary>
        /// <returns>合法順序集</returns>
        protected override HashSet<Feature> SetPreviousFeatures()
        {
            return new HashSet<Feature>() { Feature.Null, Feature.NUMBER, Feature.BINARY, Feature.EQUAL, Feature.RIGHT_BRACKET, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY };
        }

        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Equal), typeof(LeftBracket), typeof(Clear), typeof(Unary) };
        }

        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(RightBracket),typeof(Clear), typeof(ClearError), typeof(BackSpace), typeof(Unary) };
        }


    }
}