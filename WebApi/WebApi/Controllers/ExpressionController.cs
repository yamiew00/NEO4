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
        /// 樹的stack
        /// </summary>
        private Stack<ExpressionTree> TreeStack = new Stack<ExpressionTree>();

        /// <summary>
        /// 運算方法的類別
        /// </summary>
        private Evaluator Evaluator;

        /// <summary>
        /// 建構子
        /// </summary>
        public ExpressionController()
        {
            TreeStack.Push(new ExpressionTree());
            Evaluator = new Evaluator();
        }

        /// <summary>
        /// 為最外層的樹新增運算符
        /// </summary>
        /// <param name="Operator">運算符</param>
        public void Add(BinaryOperator Operator)
        {
            TreeStack.Peek().Add(Operator);
        }

        /// <summary>
        /// 為最外層的樹修改運算符
        /// </summary>
        /// <param name="Operator">運算符</param>
        public void Modify(BinaryOperator Operator)
        {
            TreeStack.Peek().ModifyOperator(Operator);
        }
        
        /// <summary>
        /// 為最外層的數新增樹，並同時做單元運算
        /// </summary>
        /// <param name="number">數字</param>
        /// <param name="unaryList">單元運算列</param>
        public void Add(decimal number, List<UnaryOperator> unaryList)
        {
            TreeStack.Peek().Add(number);
            unaryList.ForEach(x => ExecuteUnary(x));
        }

        /// <summary>
        /// 左括號事件，為stack新增一個子樹
        /// </summary>
        public void LeftBracket()
        {
            TreeStack.Push(new ExpressionTree());
        }

        /// <summary>
        /// 右括號事件，pop掉一棵子樹並新增至下一層的樹
        /// </summary>
        public void RightBracket()
        {
            var subTree = TreeStack.Pop();

            TreeStack.Peek().Add(subTree);
        }

        /// <summary>
        /// 運算出答案
        /// </summary>
        /// <returns></returns>
        public decimal GetResult()
        {
            //如果stack中還存在子樹，則必定有誤。
            if (TreeStack.Count == 1)
            {
                //算完就pop
                var answer = Evaluator.EvaluateTree(TreeStack.Pop());
                TreeStack.Push(new ExpressionTree());
                return answer;
            }
            else
            {
                throw new Exception("計算無效");
            }
        }

        /// <summary>
        /// Clear事件
        /// </summary>
        public void Clear()
        {
            TreeStack = new Stack<ExpressionTree>();
            TreeStack.Push(new ExpressionTree());
        }

        /// <summary>
        /// 執行單元運算
        /// </summary>
        /// <param name="unaryOperator">單元運算</param>
        public void ExecuteUnary(UnaryOperator unaryOperator)
        {
            var tree = TreeStack.Peek();
            var formula = unaryOperator.Formula;

            var number = tree.CurrentNode.NodeValue.Number;
            if (!number.HasValue)
            {
                throw new Exception("單元運算子輸入錯誤");
            }
            tree.CurrentNode.NodeValue.Number = formula(number.Value);
        }

        /// <summary>
        /// 新增一個新數字
        /// </summary>
        /// <param name="number">新數字</param>
        public void Add(decimal number)
        {
            TreeStack.Peek().Add(number);
        }
    }
}