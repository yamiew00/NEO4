using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Evaluate;
using WebApi.Exceptions;

namespace WebApi.Objects
{
    /// <summary>
    /// 運算控制
    /// </summary>
    public class ExpressionTreeManager
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
        public ExpressionTreeManager()
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
            if (TreeStack.Count <= 1)
            {
                throw new BracketException("右括號數量過多");
            }
            var subTree = TreeStack.Pop();

            TreeStack.Peek().Add(subTree);
        }
        

        public Result TryGetResult()
        {
            if (TreeStack.Count == 1)
            {
                //算完就pop
                var answer = Evaluator.EvaluateTree(TreeStack.Pop());
                TreeStack.Push(new ExpressionTree());
                return new Result(answer, 0);
            }
            else if (TreeStack.Count > 1)
            {
                int extraRightBracket = 0;
                //這裡不用tmp嗎，再想想
                while (TreeStack.Count > 1)
                {
                    var subTree = TreeStack.Pop();

                    TreeStack.Peek().Add(subTree);
                    extraRightBracket++;
                }
                var ans = Evaluator.EvaluateTree(TreeStack.Pop());
                TreeStack.Push(new ExpressionTree());
                return new Result(ans, extraRightBracket);
            }
            else
            {
                throw new Exception("資料有誤");
            }
        }

        //多count的狀況應該會錯
        public decimal TryGetTmpResult()
        {
            //暫時計算的結果只需要到第一層
            if (TreeStack.Count > 0)
            {
                //算完就pop
                var cloneTree = TreeUtils.CloneTree(TreeStack.Peek());
                var ans = Evaluator.EvaluateTree(cloneTree);

                
                return ans;
            }
            else if (TreeStack.Count == 0)
            {
                return 0;
            }
            else
            {
                throw new Exception("資料有誤");
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