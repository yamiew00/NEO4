using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.CalculateObjects;

namespace Calculator.Setting
{
    public static class Operators
    {
        public static Dictionary<string, BinaryOperator> BinaryDic = new Dictionary<string, BinaryOperator>
        {
            { "+", new BinaryOperator(1, (number1, number2) => number1 + number2)},
            { "-", new BinaryOperator(1, (number1, number2) => number1 - number2)},
            { "x", new BinaryOperator(2, (number1, number2) => number1 * number2)},
            { "÷", new BinaryOperator(2, (number1, number2) => number1 / number2)},
        };

        public static Dictionary<string, UnaryOperator> UnaryDic = new Dictionary<string, UnaryOperator>()
        {
            { "±", new UnaryOperator(1, (number) => -1 * number) },
            { "√", new UnaryOperator(3, (number) => (decimal) Math.Pow((double) number, 0.5))}
        };

        public static Dictionary<string, WildSymbol> WildDic = new Dictionary<string, WildSymbol>()
        {
            { "(", new WildSymbol(-99)},
            { ")", new WildSymbol(-99)}
        };

        //更新字典的方法
        public static void UpdateBinary(string mark, BinaryOperator binaryOperator)
        {
            BinaryDic.Add(mark, binaryOperator);
        }

        public static void UpdateBinary(string mark, int priority, Func<decimal, decimal, decimal> func)
        {
            BinaryDic.Add(mark, new BinaryOperator(priority, func));
        }
    }
}
