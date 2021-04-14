using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// OperatorExprssion的傳輸種類
    /// </summary>
    public enum ExpType
    {
        /// <summary>
        /// 只有運算符
        /// </summary>
        OP,

        /// <summary>
        /// 數字→運算符
        /// </summary>
        NUM_OP,

        /// <summary>
        /// 左括號→數字→運算符
        /// </summary>
        LB_NUM_OP,

        /// <summary>
        /// 數字→右括號→運算符
        /// </summary>
        NUM_RB_OP
    }

    /// <summary>
    /// 運算表達式
    /// </summary>
    public class OperatorExpression
    {
        /// <summary>
        /// 雙元運算子
        /// </summary>
        public char? BinaryOperator;

        /// <summary>
        /// 數字
        /// </summary>
        public decimal? Number;

        /// <summary>
        /// 左括號
        /// </summary>
        public bool LeftBracket;

        /// <summary>
        /// 右括號
        /// </summary>
        public bool RightBracket;

        /// <summary>
        /// 單元運算列
        /// </summary>
        public List<char> UnaryList;

        /// <summary>
        /// 回傳運算表達式的表達型態
        /// </summary>
        /// <returns>ExpType</returns>
        public ExpType Type()
        {
            if (BinaryOperator.HasValue && !Number.HasValue && LeftBracket == false && RightBracket == false)
            {
                return ExpType.OP;
            }
            else if (BinaryOperator.HasValue && Number.HasValue && LeftBracket == false && RightBracket == false)
            {
                return ExpType.NUM_OP;
            }
            else if (BinaryOperator.HasValue && Number.HasValue && LeftBracket == true && RightBracket == false)
            {
                return ExpType.LB_NUM_OP;
            }
            else if (BinaryOperator.HasValue && Number.HasValue && LeftBracket == false && RightBracket == true)
            {
                return ExpType.NUM_RB_OP;
            }
            else
            {
                throw new Exception("ExpressionType錯誤");
            }
        }
    }
}