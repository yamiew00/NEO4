using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Extensions;
using Calculator.Controllers;

namespace Calculator.Tools
{
    /// <summary>
    /// 處理中後序的運算樹
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        /// 運算式
        /// </summary>
        public string Expression { get; private set; }

        /// <summary>
        /// 輸入運算式
        /// </summary>
        /// <param name="element">輸入字串</param>
        public void Input(string element)
        {
            Expression += element;   
        }

        /// <summary>
        /// 將運算樹轉換成中序。很多防呆可以做，正常使用沒問題
        /// </summary>
        /// <returns>中序</returns>
        public List<string> ToInfix()
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

        /// <summary>
        /// 中序轉後序
        /// </summary>
        /// <param name="infix">中序</param>
        /// <returns>後序</returns>
        public List<string> InfixToPostFix(List<string> infix)
        {
            List<string> result = new List<string>();

            //empty stack
            Stack<string> stack = new Stack<string>();

            //製造stack與result
            ComputeIterative(infix, result, stack);

            //最後把operators都pop出來
            while (stack.Count > 0)
            {
                result.Add(stack.Pop());
            }

            return result;
        }

        /// <summary>
        /// 中序轉後序的演算法
        /// </summary>
        /// <param name="infix">中序</param>
        /// <param name="result">結果</param>
        /// <param name="stack">堆疊</param>
        private void ComputeIterative(List<string> infix, List<string> result, Stack<string> stack)
        {
            foreach (var item in infix)
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
                else
                {
                    //限定成既有的運算符號。若輸入了未定義運算符會error
                    while (stack.Count > 0
                        && OperatorController.GetInstance().GetPriority(item) <= OperatorController.GetInstance().GetPriority(stack.Peek()))
                    {
                        result.Add(stack.Pop());
                    }
                    stack.Push(item);
                }
            }
        }
       
        /// <summary>
        /// 判斷是否為數字
        /// </summary>
        /// <param name="str">字串</param>
        /// <returns>「是數字」的布林值</returns>
        private bool IsNumber(string str)
        {
            if (decimal.TryParse(str, out decimal i))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 回傳計算結果
        /// </summary>
        /// <returns>計算結果</returns>
        public decimal Result()
        {
            return Evaluator.PostfixToAns(InfixToPostFix(ToInfix()));
        }

        /// <summary>
        /// Clear事件
        /// </summary>
        public void Clear()
        {
            Expression = string.Empty;
        }

        /// <summary>
        /// Clear Error。也就是清掉最後一個數字
        /// </summary>
        public void ClearError()
        {
            int lastIndex = Expression.Length - 1;
            char element = Expression[lastIndex];

            for (int index = Expression.Length - 1; index >= 0; index--)
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

        /// <summary>
        /// 返回鍵，也就是清最後一個輸入
        /// </summary>
        public void BackSpace()
        {
            if (Expression != null && Expression.Length > 0)
            {
                Expression = Expression.Substring(0, Expression.Length - 1);
            }
        }
    }
}
