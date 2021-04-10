using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public enum EqualType
    {
        NUM_EQUAL,
        NUM_RB_EQUAL
    }

    public class EqualExpression
    {
        public decimal? Number;
        public bool RightBracket;
        public List<char> UnaryList;

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