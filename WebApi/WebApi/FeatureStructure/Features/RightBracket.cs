using System;
using System.Collections.Generic;
using WebApi.Evaluate.Tree;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 右括號鍵:Concrete IFeature物件
    /// </summary>
    public class RightBracket : Feature
    {
        /// <summary>
        /// 空建構子。反射用的
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

        /// <summary>
        /// 回傳此功能後面可以接的功能集
        /// </summary>
        /// <returns>後面可以接的功能集</returns>
        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(Unary) };
        }

        /// <summary>
        /// 回傳此功能前面可以接的功能集
        /// </summary>
        /// <returns>前面可以接的功能集</returns>
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
            return frameUpdate;
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

        /// <summary>
        /// 回傳新增物件的方法
        /// </summary>
        /// <returns>委派</returns>
        public override Func<char, Feature> Create()
        {
            return (content) => new RightBracket();
        }

    }
}