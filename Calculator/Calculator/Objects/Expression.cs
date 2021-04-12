using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Objects
{
    public class Expression
    {
        public decimal? Number { get; set; }
        public string BinaryOperatorMark { get; set; }
        public List<string> UnaryList { get; set; }
        public bool LeftBracket { get; set; }
        public bool RightBracket { get; set; }

        public Expression(decimal? number, string binaryOperatorMark, bool leftBracket, bool rightBracket, List<string> unaryList)
        {
            Number = number;
            BinaryOperatorMark = binaryOperatorMark;
            LeftBracket = leftBracket;
            RightBracket = rightBracket;
            UnaryList = unaryList;
        }
    }
}
