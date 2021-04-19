using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Exceptions
{
    /// <summary>
    /// 輸入次序的例外狀況
    /// </summary>
    public class OrderException : Exception
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="message">錯誤訊息</param>
        public OrderException(string message):base(message)
        {
        }
    }
}