using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonRequest
{
    /// <summary>
    /// 單元運算子請求
    /// </summary>
    public class RequestUnaryJson
    {
        /// <summary>
        /// 單元運算子名稱
        /// </summary>
        public char UnaryName;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="unaryName">單元運算子</param>
        public RequestUnaryJson(char unaryName)
        {
            UnaryName = unaryName;
        }
    }
}
