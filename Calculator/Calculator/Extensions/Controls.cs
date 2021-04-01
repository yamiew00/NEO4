using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator.Extensions
{
    public static class Controls
    {
        public static void ShowText(this TextBox textBox, String str)
        {
            textBox.Text = str;
        }

        //Debug用
        public static string Print(this List<string> list)
        {
            string result = string.Empty;
            list.ForEach(x => result += $"{x}, ");
            return result;
        }
    }
}
