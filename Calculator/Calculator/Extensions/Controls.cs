using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator.Extensions
{
    /// <summary>
    /// Control的擴充方法
    /// </summary>
    public static class Controls
    {
        /// <summary>
        /// 顯示指定文字
        /// </summary>
        /// <param name="textBox">指定的textBox</param>
        /// <param name="str">指定文字</param>
        public static void ShowText(this TextBox textBox, string str)
        {
            textBox.Text = str;
        }

        /// <summary>
        /// Debug用的
        /// </summary>
        /// <param name="list">stringList</param>
        /// <returns>string</returns>
        public static string Print(this List<string> list)
        {
            string result = string.Empty;
            list.ForEach(x => result += $"{x}, ");
            return result;
        }
    }
}
