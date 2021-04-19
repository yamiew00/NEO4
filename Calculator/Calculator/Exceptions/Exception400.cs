using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Exceptions
{
    /// <summary>
    /// 自訂例外狀況
    /// </summary>
    class Exception400 : Exception
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="msg">例外狀況訊息</param>
        public Exception400(string msg) : base(msg)
        {

        }
    }
}
