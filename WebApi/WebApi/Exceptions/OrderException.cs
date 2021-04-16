using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Exceptions
{
    public class OrderException : Exception
    {
        public OrderException(string message):base(message)
        {
        }
    }
}