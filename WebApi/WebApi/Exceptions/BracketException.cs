using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Exceptions
{
    public class BracketException : Exception
    {
        public BracketException(string msg) : base(msg)
        {

        }
    }
}