using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Objects
{
    public class EqualExpression
    {
        public decimal? Number { get; set; }
        public bool RightBracket { get; set; }

        public List<string> UnaryList { get; set; }

        public EqualExpression(decimal? number, bool rightBracket, List<string> unaryList)
        {
            Number = number;
            RightBracket = rightBracket;
            UnaryList = unaryList;
        }
    }
}
