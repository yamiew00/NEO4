using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Exceptions
{
    public class TooLongDigitException : Exception
    {
        public TooLongDigitException(string msg) : base(msg)
        {
        }
    }
}