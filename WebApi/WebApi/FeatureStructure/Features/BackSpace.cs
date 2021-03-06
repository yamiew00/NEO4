using System;
using System.Collections.Generic;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 返回鍵:Concrete IFeature物件
    /// </summary>
    public class BackSpace : Feature
    {
        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        private static readonly HashSet<Type> BACKSPACE_PREVIOUS_TYPE = new HashSet<Type>() { typeof(Number), typeof(BackSpace) };

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        private static readonly HashSet<Type> BACKSPACE_AFTERWARD_TYPE = new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(RightBracket), typeof(Clear), typeof(BackSpace), typeof(Unary) };

        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        public override HashSet<Type> PreviousType => BACKSPACE_PREVIOUS_TYPE;

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        public override HashSet<Type> AfterWardType => BACKSPACE_AFTERWARD_TYPE;

        /// <summary>
        /// 空建構子。反射用的
        /// </summary>
        public BackSpace()
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
            boardObject.FrameObject.Panel = frameUpdate.Refresh(boardObject.FrameObject.Panel);

            return boardObject.FrameObject;
        }

        /// <summary>
        /// 依計畫內容回傳畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        public override FrameUpdate Compute(ComputeObject computeObject)
        {
            int RemoveLength = 0;
            if (computeObject.NumberField == null)
            {
                return new FrameUpdate(removeLength: RemoveLength, updateString: string.Empty);
            }

            RemoveLength = computeObject.NumberField.Number.Value.ToString().Length;
            if (computeObject.NumberField.IsEndWithPoint())
            {
                RemoveLength += 1;
            }
            computeObject.NumberField.ClearOneDigit();
            string updateString = computeObject.NumberField.Number.ToString();
            if (computeObject.NumberField.IsEndWithPoint())
            {
                updateString += ".";
            }
            return new FrameUpdate(RemoveLength, updateString);
        }
    }
}