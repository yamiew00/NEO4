using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Controllers;
using Calculator.Extensions;

namespace Calculator.Tools
{
    /// <summary>
    /// 運算器
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// 後序轉成計算結果
        /// </summary>
        /// <param name="postfix">後序</param>
        /// <returns>計算結果</returns>
        public static decimal PostfixToAns(List<string> postfix)
        {
            Stack<decimal> stack = new Stack<decimal>();

            ComputeIterative(postfix, stack);

            //此時stack剩下的唯一一個數字就是答案
            if (stack.Count == 1)
            {
                return stack.Pop();
            }
            else
            {
                throw new Exception("後序有誤");
            }
        }

        /// <summary>
        /// 後序轉結果的演算過程
        /// </summary>
        /// <param name="postfix">後序</param>
        /// <param name="stack">堆疊</param>
        private static void ComputeIterative(List<string> postfix, Stack<decimal> stack)
        {
            foreach (var item in postfix)
            {
                //判斷是不是單元或雙元運算子
                bool isBinary = OperatorController.GetInstance().GetBinaryMarks().Contains(item);

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
        }

        /// <summary>
        /// 判斷是否為數字
        /// </summary>
        /// <param name="str">字串</param>
        /// <returns>「是數字」的布林值</returns>
        private static bool IsNumber(string str)
        {
            if (decimal.TryParse(str, out decimal i))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 二元運算
        /// </summary>
        /// <param name="op">運算規則的名稱</param>
        /// <param name="number1">數字1</param>
        /// <param name="number2">數字2</param>
        /// <returns>運算結果</returns>
        private static decimal BinaryCompute(string op, decimal number1, decimal number2)
        {
            var formula = OperatorController.GetInstance().GetBinaryFormula(op);

            return formula(number1, number2);
        }

        /// <summary>
        /// 單元運算
        /// </summary>
        /// <param name="op">運算規則的名稱</param>
        /// <param name="number">數字</param>
        /// <returns>運算結果</returns>
        private static decimal UnaryCompute(string op, decimal number)
        {
            var formula = OperatorController.GetInstance().GetUnaryFormula(op);

            return formula(number);
        }
    }
}
