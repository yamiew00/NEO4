using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonRequest
{
    /// <summary>
    /// 雙元運算子網路請求
    /// </summary>
    public class RequestBinaryJson
    {
        /// <summary>
        /// 雙元運算子名稱
        /// </summary>
        public char BinaryName;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="binaryName">雙元運算子名稱</param>
        public RequestBinaryJson(char binaryName)
        {
            BinaryName = binaryName;
        }
    }
}
