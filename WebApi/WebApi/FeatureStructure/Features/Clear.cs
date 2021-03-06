using System;
using System.Collections.Generic;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 清除鍵:Concrete IFeature物件
    /// </summary>
    public class Clear : Feature
    {
        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        private static readonly HashSet<Type> CLEAR_PREVIOUS_TYPE = new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(RightBracket), typeof(ClearError), typeof(BackSpace), typeof(Unary) };

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        private static readonly HashSet<Type> CLEAR_AFTERWARD_TYPE = new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(Unary) };

        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        public override HashSet<Type> PreviousType => CLEAR_PREVIOUS_TYPE;

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        public override HashSet<Type> AfterWardType => CLEAR_AFTERWARD_TYPE;

        /// <summary>
        /// 空建構子。反射用的
        /// </summary>
        public Clear()
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
            //全資訊初始化
            boardObject.Init();

            //完整運算式的刷新
            boardObject.CompleteExpression = string.Empty;

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
            computeObject.Init();
            return new FrameUpdate(FrameUpdate.REMOVE_ALL, string.Empty);
        }
    }
}