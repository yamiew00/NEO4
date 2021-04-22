using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Evaluate.Tree
{
    /// <summary>
    /// 計算結果，以及
    /// </summary>
    public class Result
    {
        /// <summary>
        /// (樹的)計算結果
        /// </summary>
        public decimal Answer { get; private set; }

        /// <summary>
        /// 補上的右括號數量
        /// </summary>
        public int ExtraRightBracketCount { get; private set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="result">計算結果</param>
        /// <param name="extraRightBracket">補上的右括號數量</param>
        public Result(decimal result, int extraRightBracket)
        {
            this.Answer = result;
            this.ExtraRightBracketCount = extraRightBracket;
        }
    }
}