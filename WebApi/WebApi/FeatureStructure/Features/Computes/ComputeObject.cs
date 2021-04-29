using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Evaluate.Tree;

namespace WebApi.FeatureStructure.Computes
{
    /// <summary>
    /// 計算物件
    /// </summary>
    public class ComputeObject
    {
        /// <summary>
        /// 前次執行功能
        /// </summary>
        public Type LastFeature { get; set; }

        /// <summary>
        /// 數字
        /// </summary>
        public NumberField NumberField { get; set; }

        /// <summary>
        /// 樹堆疊
        /// </summary>
        public Stack<ExpressionTree> TreeStack { get; set; }

        /// <summary>
        /// 當前答案
        /// </summary>
        public decimal CurrentAnswer { get; set; }

        /// <summary>
        /// 當前單元運算字串
        /// </summary>
        public string CurrentUnaryString { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public ComputeObject()
        {
            Init();
        }

        /// <summary>
        /// 全體初始化
        /// </summary>
        public void Init()
        {
            LastFeature = typeof(Clear);
            NumberField = null;
            TreeStack = new Stack<ExpressionTree>();
            TreeStack.Push(new ExpressionTree());
            CurrentAnswer = 0;
            CurrentUnaryString = string.Empty;
        }
    }
}