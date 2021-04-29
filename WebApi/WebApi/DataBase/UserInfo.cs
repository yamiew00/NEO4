using System;
using System.Collections;
using System.Collections.Generic;
using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 用戶資訊(計算、畫面)
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 完整表達式
        /// </summary>
        public string CompleteExpression { get; set; }

        /// <summary>
        /// 畫面物件
        /// </summary>
        public FrameObject FrameObject { get; set; }

        /// <summary>
        /// 前次執行功能
        /// </summary>
        public Type LastFeature { get; set; }

        /// <summary>
        /// 數字
        /// </summary>
        public NumberField NumberField { get; set; }
        
        /// <summary>
        /// 運算樹
        /// </summary>
        public Stack<ExpressionTree> TreeStack { get; set; }

        /// <summary>
        /// 當前答案
        /// </summary>
        public decimal CurrentAnswer { get; set; }

        /// <summary>
        /// 單元運算字串
        /// </summary>
        public string CurrentUnaryString { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public UserInfo()
        {
            Init();
        }

        /// <summary>
        /// 用戶資訊的初始化
        /// </summary>
        public void Init()
        {
            CompleteExpression = string.Empty;
            FrameObject = new FrameObject();
            //LastFeature初始預設為Clear
            LastFeature = typeof(Clear); 
            //NumberField預設為null(待填入)
            NumberField = null;
            CurrentAnswer = 0;
            TreeStack = new Stack<ExpressionTree>();
            TreeStack.Push(new ExpressionTree());
            CurrentUnaryString = string.Empty;
        }
    }
}