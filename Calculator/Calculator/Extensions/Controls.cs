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

        public static void AppendText(this TextBox textBox, string str)
        {
            textBox.Text = textBox.Text + str;
        }

        public static string Last(this TextBox textBox, string str)
        {
            return textBox.Text.Last().ToString();
        }

        public static void RemoveLast(this TextBox textBox, int length)
        {
            textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - length);
        }
    }
}
