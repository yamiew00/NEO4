using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Evaluate
{
    public class Result
    {
        public decimal answer { get; private set; }
        public int extraRightBracket { get; private set; }

        public Result(decimal result, int extraRightBracket)
        {
            this.answer = result;
            this.extraRightBracket = extraRightBracket;
        }
    }
}