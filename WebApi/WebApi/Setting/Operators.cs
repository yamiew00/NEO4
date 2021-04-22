using System;
using System.Collections.Generic;
using WebApi.Evaluate.Operators;

namespace WebApi.Setting
{
    /// <summary>
    /// 運算符號
    /// </summary>
    public static class Operators
    {
        /// <summary>
        /// 二元運算字典
        /// </summary>
        private static Dictionary<char, BinaryOperator> BinaryDic = new Dictionary<char, BinaryOperator>()
        {
            { '+', new BinaryOperator(1, (num1, num2) => num1 + num2, '+')},
            { '-', new BinaryOperator(1, (num1, num2) => num1 - num2, '-')},
            { 'x', new BinaryOperator(2, (num1, num2) => num1 * num2, 'x')},
            { '÷', new BinaryOperator(2, (num1, num2) => num1 / num2, '÷')},
        };

        /// <summary>
        /// 單元運算字典
        /// </summary>
        private static Dictionary<char, UnaryOperator> UnaryDic = new Dictionary<char, UnaryOperator>()
        {
            {'±', new UnaryOperator((num) => -1 * num, '±') },
            {'√', new UnaryOperator((num) => (decimal)Math.Pow((double) num, 0.5), '√')}
        };

        /// <summary>
        /// 取得雙元運算子
        /// </summary>
        /// <param name="name">雙元運算符號</param>
        /// <returns>雙元運算</returns>
        public static BinaryOperator GetBinary(char name)
        {
            return BinaryDic[name];
        }

        /// <summary>
        /// 取得單元運算子
        /// </summary>
        /// <param name="name">單元運算符號</param>
        /// <returns>單元運算</returns>
        public static UnaryOperator GetUnary(char name)
        {
            return UnaryDic[name];
        }
    }
}