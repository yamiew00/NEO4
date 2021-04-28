using System;
using System.Collections.Generic;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 右括號鍵:Concrete IFeature物件
    /// </summary>
    public class RightBracket : IFeature
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userid">用戶id</param>
        public RightBracket(int userid) : base(userid)
        {
        }

        /// <summary>
        /// 空建構子(必要)
        /// </summary>
        public RightBracket()
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

            //panel, subpanel設定。
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = frameUpdate.Answer;

            return FrameObject;
        }

        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(Unary) };
        }

        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(LeftBracket), typeof(RightBracket), typeof(ClearError), typeof(BackSpace), typeof(Unary) };
        }

        /// <summary>
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate = Tree();

            //執行成功時記錄下這次的Cast
            PreviousFeature = Feature.RIGHT_BRACKET;
            return frameUpdate;
        }

        /// <summary>
        /// 設定實體物件的合法順序規則
        /// </summary>
        /// <returns>合法順序集</returns>
        protected override HashSet<Feature> SetPreviousFeatures()
        {
            return new HashSet<Feature>() { Feature.NUMBER, Feature.BINARY, Feature.LEFT_BRACKET, Feature.RIGHT_BRACKET, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY };
        }

        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate Tree()
        {
            string updateString = string.Empty;

            if (NumberField != null)
            {
                TreeStack.Peek().Add(NumberField.Number.Value);
            }
            else if (NumberField == null)
            {
                TreeStack.Peek().Add(CurrentAnswer);
                updateString += CurrentAnswer.ToString();
            }
            
            //右括號處理
            if (TreeStack.Count <= 1)
            {
                throw new BracketException("右括號數量過多");
            }
            var subTree = TreeStack.Pop();

            TreeStack.Peek().Add(subTree);

            updateString += ")";

            NumberField = null;
            
            CurrentAnswer = GetTmpResult();

            return new FrameUpdate(CurrentAnswer.ToString(), removeLength: 0, updateString: updateString);
        }
    }
}