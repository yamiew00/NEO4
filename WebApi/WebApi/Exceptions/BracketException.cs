using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Exceptions
{
    /// <summary>
    /// 括號數量不正確
    /// </summary>
    public class BracketException : Exception
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="msg">message</param>
        public BracketException(string msg) : base(msg)
        {
        }
    }
}