using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Objects;

namespace WebApi.Setting
{
    public static class Operators
    {
        public static Dictionary<char, BinaryOperator> Dic = new Dictionary<char, BinaryOperator>()
        {
            { '+', new BinaryOperator(1, '+', (num1, num2) => num1 + num2)},
            { '-', new BinaryOperator(1, '-', (num1, num2) => num1 - num2)},
            { 'x', new BinaryOperator(2, 'x', (num1, num2) => num1 * num2)},
            { '÷', new BinaryOperator(2, '÷', (num1, num2) => num1 / num2)},
        };

    }
}