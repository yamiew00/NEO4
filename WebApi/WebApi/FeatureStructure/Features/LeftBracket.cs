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
    public class LeftBracket : Feature
    {

        /// <summary>
        /// 空建構子。反射用的
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

        /// <summary>
        /// 回傳此功能後面可以接的功能集
        /// </summary>
        /// <returns>後面可以接的功能集</returns>
        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(LeftBracket), typeof(RightBracket), typeof(Clear)};
        }

        /// <summary>
        /// 回傳此功能前面可以接的功能集
        /// </summary>
        /// <returns>前面可以接的功能集</returns>
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

            if (LastFeature == typeof(Equal))
            {
                frameUpdate.RemoveLength = FrameUpdate.REMOVE_ALL;
            }
            
            return frameUpdate;
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

        /// <summary>
        /// 回傳新增物件的方法
        /// </summary>
        /// <returns>委派</returns>
        public override Func<char, Feature> Create()
        {
            return (content) => new LeftBracket();
        }
    }
}