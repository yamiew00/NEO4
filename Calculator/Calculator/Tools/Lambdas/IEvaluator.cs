using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tools
{
    class IEvaluator
    {
        //寫死成兩個Parameter
        private ParameterExpression a = Expression.Parameter(typeof(decimal), "a");
        private ParameterExpression b = Expression.Parameter(typeof(decimal), "b");

        //Dic
        private Dictionary<string, ParameterExpression> Dic = new Dictionary<string, ParameterExpression>();

        //運算符
        private string[] operators = new string[] {"+", "-", "*", "/"};

        //先處理雙元
        private string[] array = new string[2];

        //後序轉delegate
        public Func<decimal, decimal, decimal> InfixToDelegate(List<string> postfix)
        {
            //套用中的運算規則
            Stack<BinaryExpression> stack = new Stack<BinaryExpression>();

            foreach (var item in postfix)
            {
                if (IsNumber(item))
                {
                    stack.Push(Expression.Add(Expression.Constant(decimal.Parse(item)), Expression.Constant(0M)));
                }
                else if (IsParam(item))
                {
                    stack.Push(Expression.Add(Dic[item], Expression.Constant(0M)));
                }
                else if (operators.Contains(item))
                {
                    //防呆
                    if (stack.Count < 2)
                    {
                        throw new Exception("後序有誤");
                    }

                    //計算
                    var number1 = stack.Pop();
                    var number2 = stack.Pop();
                    stack.Push(BinaryCompute(item, number2, number1));
                }
                else
                {
                    Console.WriteLine($"無法識別 = {item}");
                    throw new Exception("無法識別的符號");
                }
            }

            if (stack.Count == 1)
            {
                return Expression.Lambda<Func<decimal, decimal, decimal>>(stack.Pop(), a, b).Compile();
            }
            else
            {
                throw new Exception("後序有誤");
            }
        }


        //判斷是否為「數字」
        private bool IsParam(string str)
        {
            if (Dic.Keys.Contains(str))
            {
                return true;
            }
            else if (operators.Contains(str))
            {
                //忽略運算符
                return false;
            }
            else if (!Dic.Keys.Contains(str) && Dic.Keys.Count() == 0)
            {
                Dic.Add(str, a);
                return true;
            }
            else if (!Dic.Keys.Contains(str) && Dic.Keys.Count() == 1)
            {
                Dic.Add(str, b);
                return true;
            }
            return false;
        }

        //判斷是否為數字
        private static bool IsNumber(string str)
        {
            if (decimal.TryParse(str, out decimal i))
            {
                return true;
            }
            return false;
        }

        //雙元運算
        private BinaryExpression BinaryCompute(string oper, BinaryExpression number1, BinaryExpression number2)
        {
            
            switch (oper)
            {
                case "+":
                    return Expression.Add(number1, number2);
                case "-":
                    return Expression.Subtract(number1, number2);
                case "*":
                    return Expression.Multiply(number1, number2);
                case "/":
                    return Expression.Divide(number1, number2);
                default:
                    return null;
            }
        }


        
    }
}
