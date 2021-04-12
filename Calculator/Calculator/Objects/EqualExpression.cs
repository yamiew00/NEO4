using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Objects
{
    /// <summary>
    /// 等號表達式
    /// </summary>
    public class EqualExpression
    {
        /// <summary>
        /// 數字
        /// </summary>
        public decimal? Number { get; set; }

        /// <summary>
        /// 右括號
        /// </summary>
        public bool RightBracket { get; set; }

        /// <summary>
        /// 單元運算列
        /// </summary>
        public List<string> UnaryList { get; set; }

        /// <summary>
        /// 等號表達式
        /// </summary>
        /// <param name="number">數字</param>
        /// <param name="rightBracket">右括號</param>
        /// <param name="unaryList">單元運算列</param>
        public EqualExpression(decimal? number, bool rightBracket, List<string> unaryList)
        {
            Number = number;
            RightBracket = rightBracket;
            UnaryList = unaryList;
        }
    }
}
