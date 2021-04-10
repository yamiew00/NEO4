using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Extensions
{
    public static class Strings
    {
        public static string AddString(this string str, string newStr)
        {
            return new StringBuilder().Append(str)
                                      .Append(newStr)
                                      .ToString();
        }
        public static string RemoveLast(this string str, int length)
        {
            return str.Substring(0, str.Length - length);
        }

        public static void test(this string str, string newStr)
        {
            str =  new StringBuilder().Append(str)
                                      .Append(newStr)
                                      .ToString();
        }
    }
}
