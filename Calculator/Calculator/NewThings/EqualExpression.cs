using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.NewThings
{
    public class EqualExpression
    {
        public decimal? Number { get; set; }
        public bool RightBracket { get; set; }

        public EqualExpression(decimal? number, bool rightBracket)
        {
            Number = number;
            RightBracket = rightBracket;
        }
    }
}
