using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// ClearError的網路回應
    /// </summary>
    public class ClearErrorResponse : StatusMessage
    {
        /// <summary>
        /// 更新內容
        /// </summary>
        public Updates Update { get; private set; }

        /// <summary>
        /// 建構子(為了外部泛型方法必須存在)
        /// </summary>
        public ClearErrorResponse()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="update">更新內容</param>
        public ClearErrorResponse(Updates update)
        {
            Update = update;
        }
    }
}