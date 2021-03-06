using System;
using System.Collections.Generic;
using WebApi.Evaluate.Tree;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 清除錯誤鍵:Concrete IFeature物件
    /// </summary>
    public class ClearError : Feature
    {
        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        private static readonly HashSet<Type> CLEARERROR_PREVIOUS_TYPE = new HashSet<Type>() { typeof(Number), typeof(ClearError) };

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        private static readonly HashSet<Type> CLEARERROR_AFTERWARD_TYPE = new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(RightBracket), typeof(Clear), typeof(ClearError), typeof(Unary) };

        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        public override HashSet<Type> PreviousType => CLEARERROR_PREVIOUS_TYPE;

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        public override HashSet<Type> AfterWardType => CLEARERROR_AFTERWARD_TYPE;

        /// <summary>
        /// 空建構子。反射用的
        /// </summary>
        public ClearError()
        {
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
            boardObject.FrameObject.Panel = "0";

            return boardObject.FrameObject;
        }

        /// <summary>
        /// 依計畫內容回傳畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        public override FrameUpdate Compute(ComputeObject computeObject)
        {
            if (computeObject.NumberField == null)
            {
                return new FrameUpdate(removeLength: 0, updateString: string.Empty);
            }
            int length = computeObject.NumberField.Number.Value.ToString().Length;
            computeObject.NumberField = new NumberField();
            return new FrameUpdate(removeLength: length, updateString: "0");
        }
    }
}