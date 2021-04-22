using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Exceptions
{
    public class SquareRootException : Exception
    {
        public SquareRootException(string msg) : base(msg)
        {
        }
    }
}