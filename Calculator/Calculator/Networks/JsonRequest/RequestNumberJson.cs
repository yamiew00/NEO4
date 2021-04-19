using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonRequest
{
    /// <summary>
    /// 數字請求
    /// </summary>
    public class RequestNumberJson
    {
        /// <summary>
        /// 數字
        /// </summary>
        public char Number;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="number">數字</param>
        public RequestNumberJson(char number)
        {
            Number = number;
        }
    }
}
