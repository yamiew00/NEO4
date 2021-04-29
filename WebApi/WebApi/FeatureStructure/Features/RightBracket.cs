using System;
using System.Collections.Generic;
using WebApi.Evaluate.Tree;
using WebApi.Exceptions;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;
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
        /// 依功能回傳畫面物件
        /// </summary>
        /// <param name="boardObject">面板物件</param>
        /// <param name="frameUpdate">畫面更新</param>
        /// <returns>畫面物件</returns>
        public override FrameObject GetFrameObject(BoardObject boardObject, FrameUpdate frameUpdate)
        {
            //完整運算式的刷新
            boardObject.CompleteExpression = frameUpdate.Refresh(boardObject.CompleteExpression);

            //panel, subpanel設定。
            boardObject.FrameObject.SubPanel = boardObject.CompleteExpression;
            boardObject.FrameObject.Panel = frameUpdate.Answer;

            return boardObject.FrameObject;
        }

        /// <summary>
        /// 依計畫內容回傳畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        public override FrameUpdate Compute(ComputeObject computeObject)
        {
            string updateString = string.Empty;

            if (computeObject.NumberField != null)
            {
                computeObject.TreeStack.Peek().Add(computeObject.NumberField.Number.Value);
            }
            else if (computeObject.NumberField == null)
            {
                computeObject.TreeStack.Peek().Add(computeObject.CurrentAnswer);
                updateString += computeObject.CurrentAnswer.ToString();
            }

            //右括號處理
            if (computeObject.TreeStack.Count <= 1)
            {
                throw new BracketException("右括號數量過多");
            }
            var subTree = computeObject.TreeStack.Pop();

            computeObject.TreeStack.Peek().Add(subTree);

            updateString += ")";

            computeObject.NumberField = null;

            computeObject.CurrentAnswer = GetTmpResult(computeObject);

            return new FrameUpdate(computeObject.CurrentAnswer.ToString(), removeLength: 0, updateString: updateString);
        }
    }
}