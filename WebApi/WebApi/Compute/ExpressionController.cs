using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    /// <summary>
    /// 運算控制
    /// </summary>
    public class ExpressionController
    {
        /// <summary>
        /// 存下樹的stack(為了判斷括號計算順序)
        /// </summary>
        private Stack<ExpressionTree> TreeStack = new Stack<ExpressionTree>();

        /// <summary>
        /// 建構子
        /// </summary>
        public ExpressionController()
        {
            TreeStack.Push(new ExpressionTree());
        }

        /// <summary>
        /// 新增一個運算子：在stack最頂層新增
        /// </summary>
        /// <param name="Operator">運算子</param>
        public void Add(BinaryOperator Operator)
        {
            TreeStack.Peek().Add(Operator);
        }

        /// <summary>
        /// 更改上次運算子：在stack最頂層更改
        /// </summary>
        /// <param name="Operator">運算子</param>
        public void Modify(BinaryOperator Operator)
        {
            TreeStack.Peek().ModifyOperator(Operator);
        }

        /// <summary>
        /// 新增一個經過單元運算的數字：在stack最頂層新增
        /// </summary>
        /// <param name="number">數字</param>
        /// <param name="unaryList">單元運算列</param>
        public void Add(decimal number, List<UnaryOperator> unaryList)
        {
            TreeStack.Peek().Add(number);
            unaryList.ForEach(x => ExecuteUnary(x));
        }

        /// <summary>
        /// 左括號事件：在stack最頂層新增一個子樹
        /// </summary>
        public void LeftBracket()
        {
            TreeStack.Push(new ExpressionTree());
        }

        /// <summary>
        /// 右括號事件：在stack最頂層pop掉一個子樹，並新增至下一棵樹的當前節點中
        /// </summary>
        public void RightBracket()
        {
            var subTree = TreeStack.Pop();
            TreeStack.Peek().Add(subTree);
        }

        /// <summary>
        /// 回傳計算結果
        /// </summary>
        /// <returns>計算結果</returns>
        public decimal GetResult()
        {
            if (TreeStack.Count == 1)
            {
                //算完就pop掉
                var answer = new Evaluator().EvaluateTree(TreeStack.Pop());
                TreeStack.Push(new ExpressionTree());
                return answer;
            }
            else
            {
                throw new Exception("計算無效");
            }
        }

        /// <summary>
        /// Clear事件，運算器重建。
        /// </summary>
        public void Clear()
        {
            TreeStack = new Stack<ExpressionTree>();
            TreeStack.Push(new ExpressionTree());
        }

        /// <summary>
        /// 對當前數字進行單元運算。
        /// </summary>
        /// <param name="unaryOperator">單元運算列</param>
        public void ExecuteUnary(UnaryOperator unaryOperator)
        {
            var tree = TreeStack.Peek();
            var formula = unaryOperator.Formula;

            var number = tree.CurrentNode.Value.Number;
            if (!number.HasValue)
            {
                throw new Exception("單元運算子輸入錯誤");
            }
            tree.CurrentNode.Value.Number = formula(number ?? 0);
        }
    }
}