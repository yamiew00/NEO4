using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// Status訊息
    /// </summary>
    public class StatusMessage
    {
        /// <summary>
        /// Status物件，狀態碼。
        /// </summary>
        public Status Status { get; private set; }
        
        /// <summary>
        /// 設定StatusCode
        /// </summary>
        /// <param name="code">狀態碼</param>
        public void SetStatus(int code)
        {
            Status = new Status(code);
        }
    }
}