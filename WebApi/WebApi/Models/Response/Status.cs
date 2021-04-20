using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// 網路響應的狀態
    /// </summary>
    public class Status
    {
        /// <summary>
        /// 狀態碼 400...
        /// </summary>
        public int Code { get; private set; }
        
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="code">狀態碼</param>
        public Status(int code)
        {
            Code = code;
        }
    }
}