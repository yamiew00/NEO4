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

        public void Modify(BinaryOperator Operator)
        {
            TreeStack.Peek().ModifyOperator(Operator);
        }
        
        public void Add(decimal number)
        {
            TreeStack.Peek().Add(number);
        }
        public void Add(decimal number, List<UnaryOperator> unaryList)
        {
            TreeStack.Peek().Add(number);
            unaryList.ForEach(x => ExecuteUnary(x));
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

        public void ExecuteUnary(UnaryOperator unaryOperator)
        {
            var tree = TreeStack.Peek();
            var formula = unaryOperator.Formula;


            var number = tree.CurrentNode.value.Number;
            if (!number.HasValue)
            {
                throw new Exception("單元運算子輸入錯誤");
            }
            tree.CurrentNode.value.Number = formula(number ?? 0);
        }
    }
}