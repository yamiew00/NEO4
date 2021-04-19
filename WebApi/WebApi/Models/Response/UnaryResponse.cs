using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// 單元運算子功能的回應
    /// </summary>
    public class UnaryResponse : StatusMessage
    {
        /// <summary>
        /// Update內容
        /// </summary>
        public Updates Update;

        /// <summary>
        /// 建構子(為了外部泛型方法必須存在)
        /// </summary>
        public UnaryResponse()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="update">Update內容</param>
        public UnaryResponse(Updates update)
        {
            Update = update;
        }   
    }
}