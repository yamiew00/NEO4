﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public enum ExpType
    {
        NUM_OP,
        LB_NUM_OP,
        NUM_RB_OP
    }

    public class Expression
    {
        public char? Operator;
        public decimal? Number;
        public bool LeftBracket;
        public bool RightBracket;

        public ExpType Type()
        {
            if (Operator.HasValue && Number.HasValue && LeftBracket == false && RightBracket == false)
            {
                return ExpType.NUM_OP;
            }
            else if (Operator.HasValue && Number.HasValue && LeftBracket == true && RightBracket == false)
            {
                return ExpType.LB_NUM_OP;
            }
            else if (Operator.HasValue && Number.HasValue && LeftBracket == false && RightBracket == true)
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