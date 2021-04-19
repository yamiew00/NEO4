using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// 數字響應
    /// </summary>
    public class NumberResponse : StatusMessage
    {
        /// <summary>
        /// 更新內容
        /// </summary>
        public Updates Update { get; private set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="update">更新內容</param>
        public NumberResponse(Updates update)
        {
            Update = update;
        }

        /// <summary>
        /// 建構子(為了外部泛型方法必須存在)
        /// </summary>
        public NumberResponse()
        {
        }


    }
}