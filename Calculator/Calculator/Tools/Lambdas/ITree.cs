using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Extensions;

namespace Calculator.Tools.Lambdas
{
    public class ITree
    {
        //運算符
        private string[] operators = new string[] { "+", "-", "*", "/" };

        //轉中序Infix，很多防呆可以做，正常使用沒問題
        public List<string> ToInfix(string str)
        {
            List<string> result = new List<string>();
            string unit = string.Empty;

            //之前的

            //新的
            for (int i = 0; i < str.Length; i++)
            {
                string character = str[i].ToString();

                if (character.Equals(" "))
                {
                    continue;
                }
                else if (operators.Contains(character) || character.Equals("(") || character.Equals(")"))
                {
                    //只要遇到運算符，必定要分隔
                    if (!unit.Equals(string.Empty))
                    {
                        result.Add(unit);
                    }
                    result.Add(character);
                    unit = string.Empty;
                }
                else
                {
                    //unit更新，其他各種奇形怪狀全部加進去
                    if (unit.Equals(string.Empty))
                    {
                        unit = character;
                    }
                    else
                    {
                        unit += character;
                    }
                    //最後一個要印出來
                    if (i == str.Length - 1)
                    {
                        result.Add(unit);
                    }
                }
                
            }
            
            return result;
        }


        //中序轉後序
        public List<string> InfixToPostFix(List<string> infix)
        {
            List<string> result = new List<string>();

            //empty stack
            Stack<string> stack = new Stack<string>();
            

            //製造stack與result
            foreach (var item in infix)
            {

                //不是operators就視為一個數字
                if ( !operators.Contains(item) && !item.Equals("(") && !item.Equals(")"))
                {
                    result.Add(item);
                }
                //括號目前用不上
                else if (item.Equals("("))
                {
                    stack.Push(item);
                }
                else if (item.Equals(")"))
                {
                    while (stack.Count > 0
                        && !stack.Peek().Equals("("))
                    {
                        result.Add(stack.Pop());
                    }
                    //無效運算式
                    if (stack.Count > 0
                        && !stack.Peek().Equals("("))
                    {
                        throw new Exception("無效運算式");
                    }
                    else
                    {
                        stack.Pop();
                    }
                }
                //既有運算符號
                else
                {
                    while (stack.Count > 0
                        && GetPriority(item) <= GetPriority(stack.Peek()))
                    {
                        result.Add(stack.Pop());
                    }
                    stack.Push(item);
                }
            }

            //最後把operators都pop出來
            while (stack.Count > 0)
            {
                result.Add(stack.Pop());
            }

            //Console.WriteLine($"Itree result.Print() = {result.Print()}");
            return result;
        }

        public int GetPriority(string op)
        {
            switch (op)
            {
                case "+":
                    return 1;
                case "-":
                    return 1;
                case "*":
                    return 2;
                case "/":
                    return 2;
                default:
                    return -99;
            }
        }


        public List<string> GetPostFix(string str)
        {
            return InfixToPostFix(ToInfix(str));
        }
    }
}
