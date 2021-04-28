using System;
using System.Collections.Generic;
using WebApi.Evaluate.Tree;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 清除錯誤鍵:Concrete IFeature物件
    /// </summary>
    public class ClearError : IFeature
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userid">用戶id</param>
        public ClearError(int userid) : base(userid)
        {
        }

        /// <summary>
        /// 空建構子(必要)
        /// </summary>
        public ClearError()
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
            FrameObject.Panel = "0";

            return FrameObject;
        }

        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal),typeof(LeftBracket), typeof(RightBracket), typeof(Clear), typeof(ClearError), typeof(Unary) };
        }

        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(ClearError)};
        }

        /// <summary>
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate = Tree();
            //執行成功時記錄下這次的Cast
            PreviousFeature = Feature.CLEAR_ERROR;
            return frameUpdate;
        }

        /// <summary>
        /// 設定實體物件的合法順序規則
        /// </summary>
        /// <returns>合法順序集</returns>
        protected override HashSet<Feature> SetPreviousFeatures()
        {
            return new HashSet<Feature>() { Feature.NUMBER, Feature.CLEAR_ERROR };
        }

        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate Tree()
        {
            if (NumberField == null)
            {
                return new FrameUpdate(removeLength: 0, updateString: string.Empty);
            }
            int length = NumberField.Number.Value.ToString().Length;
            NumberField = new NumberField();
            return new FrameUpdate(removeLength: length, updateString: "0");
        }
    }
}