using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.NewTrees
{
    public class NewTreeController
    {
        Stack<NewTree> newTreeStack = new Stack<NewTree>();
        NewEvaluator Evaluator = new NewEvaluator();


        public NewTreeController()
        {
            newTreeStack.Push(new NewTree());
        }

        public void Add(NewBinary Operator)
        {
            newTreeStack.Peek().Add(Operator);
        }

        public void Add(decimal number)
        {
            newTreeStack.Peek().Add(number);
        }

        /// <summary>
        /// 左括號事件
        /// </summary>
        public void LeftBracket()
        {
            newTreeStack.Push(new NewTree());
        }

        /// <summary>
        /// 右括號事件
        /// </summary>
        public void RightBracket()
        {
            var subTree = newTreeStack.Pop();
            newTreeStack.Peek().Add(subTree);
        }

        public decimal GetResult()
        { 
            if (newTreeStack.Count == 1)
            {
                return Evaluator.EvaluateTree(newTreeStack.Pop());
            }
            else
            {
                throw new Exception("計算無效");
            }
            
        }
    }
}
