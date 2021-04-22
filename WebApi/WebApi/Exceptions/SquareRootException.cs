using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Exceptions
{
    /// <summary>
    /// 根號內不可以有負數
    /// </summary>
    public class SquareRootException : Exception
    {
        /// <summary>
        /// 建樣子
        /// </summary>
        /// <param name="msg">message</param>
        public SquareRootException(string msg) : base(msg)
        {
        }
    }
}