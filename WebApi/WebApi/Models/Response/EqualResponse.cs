using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// 等號的網路回應
    /// </summary>
    public class EqualResponse : StatusMessage
    {
        /// <summary>
        /// 更新內容
        /// </summary>
        public Updates Update { get; private set; }

        /// <summary>
        /// 計算結果(不一定有結果)
        /// </summary>
        public decimal? Answer { get; private set; }

        /// <summary>
        /// 建構子(為了外部泛型方法必須存在)
        /// </summary>
        public EqualResponse()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="update">更新內容</param>
        /// <param name="answer">計算結果</param>
        public EqualResponse(Updates update, decimal? answer)
        {
            Update = update;
            Answer = answer;
        }
    }
}