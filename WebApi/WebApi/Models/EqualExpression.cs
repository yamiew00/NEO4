using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// EqualExpression的傳輸種類
    /// </summary>
    public enum EqualType
    {
        /// <summary>
        /// 數字→等號
        /// </summary>
        NUM_EQUAL,

        /// <summary>
        /// 數字→右括號→等號
        /// </summary>
        NUM_RB_EQUAL
    }

    /// <summary>
    /// 等號表達式
    /// </summary>
    public class EqualExpression
    {
        /// <summary>
        /// 數字
        /// </summary>
        public decimal? Number;

        /// <summary>
        /// 右括號
        /// </summary>
        public bool RightBracket;

        /// <summary>
        /// 單元運算列
        /// </summary>
        public List<char> UnaryList;

        /// <summary>
        /// 回傳等號表達式的表達型態
        /// </summary>
        /// <returns>EqualType</returns>
        public EqualType Type()
        {
            if (Number.HasValue && RightBracket == false )
            {
                return EqualType.NUM_EQUAL;
            }
            else if (Number.HasValue && RightBracket == true)
            {
                return EqualType.NUM_RB_EQUAL;
            }
            else
            {
                throw new Exception("EqualType錯誤");
            }
        }
    }
}