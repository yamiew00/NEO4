using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Exceptions
{
    /// <summary>
    /// 輸入位數長度不能超過限制
    /// </summary>
    public class TooLongDigitException : Exception
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="msg">message</param>
        public TooLongDigitException(string msg) : base(msg)
        {
        }
    }
}