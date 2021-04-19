using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonResponse
{
    /// <summary>
    /// 等號回應
    /// </summary>
    public class ResponseEqual
    {
        /// <summary>
        /// 更新內容
        /// </summary>
        public Update Update;

        /// <summary>
        /// 狀態碼
        /// </summary>
        public Status Status;

        /// <summary>
        /// 計算結果
        /// </summary>
        public decimal? answer;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="update">更新內容</param>
        /// <param name="answer">計算結果</param>
        public ResponseEqual(Update update, decimal? answer)
        {
            Update = update;
            this.answer = answer;
        }
    }
}
