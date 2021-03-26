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
    }
}
