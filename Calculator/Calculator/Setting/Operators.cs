using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.CalculateObjects;

namespace Calculator.Setting
{
    /// <summary>
    /// 運算符(包含雙元、單元及括號)
    /// </summary>
    public static class Operators
    {
        /// <summary>
        /// 雙元運算符字典
        /// </summary>
        public static Dictionary<string, BinaryOperator> BinaryDic { get;  private set; } 
            = new Dictionary<string, BinaryOperator>
        {
            { "+", new BinaryOperator(1, (number1, number2) => number1 + number2)},
            { "-", new BinaryOperator(1, (number1, number2) => number1 - number2)},
            { "x", new BinaryOperator(2, (number1, number2) => number1 * number2)},
            { "÷", new BinaryOperator(2, (number1, number2) => number1 / number2)}
        };

        /// <summary>
        /// 單元運算符字典
        /// </summary>
        public static Dictionary<string, UnaryOperator> UnaryDic { get; private set; }
            = new Dictionary<string, UnaryOperator>()
        {
            { "±", new UnaryOperator(1, (number) => -1 * number) },
            { "√", new UnaryOperator(3, (number) => (decimal) Math.Pow((double) number, 0.5))}
        };

        /// <summary>
        /// 特殊符號
        /// </summary>
        public static Dictionary<string, WildSymbol> WildDic { get; private set; }
            = new Dictionary<string, WildSymbol>()
        {
            { "(", new WildSymbol(-99)},
            { ")", new WildSymbol(-99)}
        };

        /// <summary>
        /// 更新字典的方法
        /// </summary>
        /// <param name="mark">符號</param>
        /// <param name="binaryOperator">雙元運算子</param>
        public static void UpdateBinary(string mark, BinaryOperator binaryOperator)
        {
            BinaryDic.Add(mark, binaryOperator);
        }

        /// <summary>
        /// 更新字典的方法
        /// </summary>
        /// <param name="mark">符號</param>
        /// <param name="priority">優先度</param>
        /// <param name="func">委派</param>
        public static void UpdateBinary(string mark, int priority, Func<decimal, decimal, decimal> func)
        {
            BinaryDic.Add(mark, new BinaryOperator(priority, func));
        }
    }
}
