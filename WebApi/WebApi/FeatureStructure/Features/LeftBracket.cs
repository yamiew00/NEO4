using System;
using System.Collections.Generic;
using WebApi.Evaluate.Tree;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 左括號鍵:Concrete IFeature物件
    /// </summary>
    public class LeftBracket : IFeature
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userid">用戶id</param>
        public LeftBracket(int userid) : base(userid)
        {
        }

        /// <summary>
        /// 空建構子(必要)
        /// </summary>
        public LeftBracket()
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

            //panel, subpanel設定。subpanel強制給0
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = "0";

            return FrameObject;
        }

        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(LeftBracket), typeof(RightBracket), typeof(Clear)};
        }

        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace) };
        }

        /// <summary>
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate = Tree();

            if (PreviousFeature == Feature.EQUAL)
            {
                frameUpdate.RemoveLength = FrameUpdate.REMOVE_ALL;
            }

            //執行成功時記錄下這次的Cast
            PreviousFeature = Feature.LEFT_BRACKET;
            return frameUpdate;
        }

        /// <summary>
        /// 設定實體物件的合法順序規則
        /// </summary>
        /// <returns>合法順序集</returns>
        protected override HashSet<Feature> SetPreviousFeatures()
        {
            return new HashSet<Feature>() { Feature.Null, Feature.BINARY, Feature.EQUAL, Feature.LEFT_BRACKET, Feature.CLEAR, Feature.CLEAR_ERROR, Feature.BACKSPACE };
        }

        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate Tree()
        {
            TreeStack.Push(new ExpressionTree());
            return new FrameUpdate(removeLength: 0, updateString: "(");
        }
    }
}