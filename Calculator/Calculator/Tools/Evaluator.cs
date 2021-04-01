using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Controllers;
using Calculator.Extensions;

namespace Calculator.Tools
{
    class Evaluator
    {
        //後序轉成計算結果
        public static decimal TransformToAns(List<string> postfix)
        {
            //套用中的運算規則

            Stack<decimal> stack = new Stack<decimal>();

            foreach(var item in postfix)
            {
                bool isBinary = OperatorController.GetInstance().GetBinaryMarks().Contains(item);

                Console.WriteLine($"OperatorController.GetInstance().GetBinaryMarks() = {OperatorController.GetInstance().GetBinaryMarks().Print()}");


                bool isUnary = OperatorController.GetInstance().GetUnaryMarks().Contains(item);

               if (IsNumber(item))
                {
                    stack.Push(decimal.Parse(item));
                }
               else if (isBinary)
                {
                    //防呆
                    if (stack.Count < 2)
                    {
                        throw new Exception("後序有誤");
                    }

                    //計算
                    decimal number1 = stack.Pop();
                    decimal number2 = stack.Pop();
                    stack.Push(BinaryCompute(item, number2, number1));
                }
               else if (isUnary)
                {
                    //防呆
                    if (stack.Count < 1)
                    {
                        throw new Exception("後序有誤");
                    }

                    //計算
                    decimal number = stack.Pop();
                    stack.Push(UnaryCompute(item, number));
                }
                else
                {
                    Console.WriteLine($"item = {item}");
                    throw new Exception("無法識別的符號");
                }
            }

            if (stack.Count == 1)
            {
                return stack.Pop();
            }
            else
            {
                throw new Exception("後序有誤");
            }
        }

        //重複囉
        //判斷是否為數字
        private static bool IsNumber(string str)
        {
            if (decimal.TryParse(str, out decimal i))
            {
                return true;
            }
            return false;
        }

        private static decimal BinaryCompute(string op, decimal number1, decimal number2)
        {
            var formula = OperatorController.GetInstance().GetBinaryFormula(op);

            return formula(number1, number2);
        }

        private static decimal UnaryCompute(string op, decimal number)
        {
            var formula = OperatorController.GetInstance().GetUnaryFormula(op);

            return formula(number);
        }
    }
}
