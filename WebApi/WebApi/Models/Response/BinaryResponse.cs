using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// 雙元運算子的網路回應
    /// </summary>
    public class BinaryResponse : StatusMessage
    {
        /// <summary>
        /// 網路回應
        /// </summary>
        public Updates Update { get; private set; }

        /// <summary>
        /// 建構子(為了外部泛型方法必須存在)
        /// </summary>
        public BinaryResponse()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="update">更新內容</param>
        public BinaryResponse(Updates update)
        {
            Update = update;
        }
    }
}