using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// 左括號的網路回應
    /// </summary>
    public class LeftBracketResponse : StatusMessage
    {
        /// <summary>
        /// 更新內容
        /// </summary>
        public Updates Update { get; private set; }

        /// <summary>
        /// 建構子(為了外部泛型方法必須存在)
        /// </summary>
        public LeftBracketResponse()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="update">更新內容</param>
        public LeftBracketResponse(Updates update)
        {
            Update = update;
        }
    }
}