using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.NewThings
{
    public class Expression
    {
        public decimal? Number { get; set; }
        public string BinaryOperatorMark { get; set; }
        public bool LeftBracket { get; set; }
        public bool RightBracket { get; set; }

        public Expression(decimal? number, string binaryOperatorMark, bool leftBracket, bool rightBracket)
        {
            
            Number = number;
            BinaryOperatorMark = binaryOperatorMark;
            LeftBracket = leftBracket;
            RightBracket = rightBracket;
        }
    }
}
