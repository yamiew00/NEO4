using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Extensions;
using Calculator.Controllers;

namespace Calculator.Tools
{
    //負責前中後序
    public class ExpressionTree
    {
        public string Expression { get; set; }

        //輸入運算式(可能一次輸入多個)
        public void Input(string element)
        {
            Expression += element;   
        }

        //Infix，很多防呆可以做，正常使用沒問題
        public List<string> Infix()
        {
            List<string> result = new List<string>();
            string unit = string.Empty;

            for (int i = 0; i < Expression.Length; i++)
            {
                char character = Expression[i];
                //小數點
                if (char.IsNumber(character) || character.Equals('.') )
                {
                    unit += character.ToString();
                    if (i == Expression.Length - 1)
                    {
                        result.Add(unit);
                    }
                }
                else
                {
                    //unit處理，若為小數點結尾，放一個零在結尾
                    if (unit.Length == 0)
                    {
                    }
                    else
                    {
                        if (unit.EndsWith("."))
                        {
                            unit += "0";
                        }
                        result.Add(unit);
                        unit = string.Empty;
                    }

                    result.Add(character.ToString());
                }
            }
            
            return result;
        }

        //中序轉後序
        public static List<string> InfixToPostFix(List<string> expression)
        {
            List<string> result = new List<string>();

            //empty stack
            Stack<string> stack = new Stack<string>();

            //製造stack與result
            foreach(var item in expression)
            {
                if (IsNumber(item))
                {
                    result.Add(item);
                }
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
                //這裡還要再做處理，只能限定成既有的運算符號(做了)
                else
                {
                    while ( stack.Count > 0
                        && OperatorController.GetInstance().GetPriority(item) <= OperatorController.GetInstance().GetPriority(stack.Peek()))
                    {
                        result.Add(stack.Pop());
                    }
                    stack.Push(item);
                }
                

            }

            //最後把operators都pop出來
            while(stack.Count > 0)
            {
                result.Add(stack.Pop());
            }

            Console.WriteLine(result.Print());

            return result;
        }
       
        //有點類似
        private static bool IsNumber(string str)
        {
            if (decimal.TryParse(str, out decimal i))
            {
                return true;
            }
            return false;
        }    

        //拿計算結果
        public decimal Result()
        {
            Console.WriteLine("count = " + InfixToPostFix(Infix()).Count());
            return Evaluator.TransformToAns(InfixToPostFix(Infix()));
        }
        
        //Clear
        public void Clear()
        {
            Expression = string.Empty;
        }

        //Clear Error，清掉最後一個數字
        public void ClearError()
        {

                int lastIndex = Expression.Length - 1;
                char element = Expression[lastIndex];


                for (int index = Expression.Length - 1; index >=0; index--)
                {
                    if (char.IsNumber(Expression[index]) || Expression[index].Equals('.'))
                    {
                        //移除最後一個元素
                        Expression = Expression.Substring(0, Expression.Length - 1);
                    }
                    else
                    {
                        break;
                    }
                }
        }

        public void BackSpace()
        {
            Expression = Expression.Substring(0, Expression.Length - 1);
        }
    }
}
