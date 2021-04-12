using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Extensions
{
    /// <summary>
    /// 字串擴充方法
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// 移除最後幾個字元
        /// </summary>
        /// <param name="str">字串</param>
        /// <param name="length">要移除的長度</param>
        /// <returns>新字串</returns>
        public static string RemoveLast(this string str, int length)
        {
            return str.Substring(0, str.Length - length);
        }
    }
}
