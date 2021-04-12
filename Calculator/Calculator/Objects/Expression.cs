using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Objects
{
    /// <summary>
    /// 運算表達式
    /// </summary>
    public class OperatorExpression
    {
        /// <summary>
        /// 數字
        /// </summary>
        public decimal? Number { get; set; }

        /// <summary>
        /// 雙元運算符
        /// </summary>
        public string BinaryOperatorMark { get; set; }

        /// <summary>
        /// 單元運算列
        /// </summary>
        public List<string> UnaryList { get; set; }

        /// <summary>
        /// 左括號
        /// </summary>
        public bool LeftBracket { get; set; }

        /// <summary>
        /// 右括號
        /// </summary>
        public bool RightBracket { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="number">數字</param>
        /// <param name="binaryOperatorMark">雙元運算符</param>
        /// <param name="leftBracket">左括號</param>
        /// <param name="rightBracket">右括號</param>
        /// <param name="unaryList">單元運算列</param>
        public OperatorExpression(decimal? number, string binaryOperatorMark, bool leftBracket, bool rightBracket, List<string> unaryList)
        {
            Number = number;
            BinaryOperatorMark = binaryOperatorMark;
            LeftBracket = leftBracket;
            RightBracket = rightBracket;
            UnaryList = unaryList;
        }
    }
}
