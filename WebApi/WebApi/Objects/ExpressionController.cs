using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    public class ExpressionController
    {
        Stack<ExpressionTree> TreeStack = new Stack<ExpressionTree>();


        public ExpressionController()
        {
            TreeStack.Push(new ExpressionTree());
        }

        public void Add(BinaryOperator Operator)
        {
            TreeStack.Peek().Add(Operator);
        }
        
        public void Add(decimal number)
        {
            TreeStack.Peek().Add(number);
        }

        /// <summary>
        /// 左括號事件
        /// </summary>
        public void LeftBracket()
        {
            TreeStack.Push(new ExpressionTree());
        }

        /// <summary>
        /// 右括號事件
        /// </summary>
        public void RightBracket()
        {
            var subTree = TreeStack.Pop();
            TreeStack.Peek().Add(subTree);
        }

        public decimal GetResult()
        {
            if (TreeStack.Count == 1)
            {
                //算完就pop? 有兩種做法
                var answer = new Evaluator().EvaluateTree(TreeStack.Pop());
                TreeStack.Push(new ExpressionTree());
                return answer;
            }
            else
            {
                throw new Exception("計算無效");
            }

        }

        public void Clear()
        {
            TreeStack = new Stack<ExpressionTree>();
            TreeStack.Push(new ExpressionTree());
        }
    }
}