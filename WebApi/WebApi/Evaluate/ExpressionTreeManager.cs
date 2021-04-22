using System;
using System.Collections.Generic;
using WebApi.Evaluate.Operators;
using WebApi.Evaluate.Tree;
using WebApi.Evaluate.Utils;
using WebApi.Exceptions;

namespace WebApi.Evaluate
{
    /// <summary>
    /// 運算控制
    /// </summary>
    public class ExpressionTreeManager
    {
        /// <summary>
        /// 樹的stack。為了括號而存在
        /// </summary>
        private Stack<ExpressionTree> TreeStack = new Stack<ExpressionTree>();

        /// <summary>
        /// 運算方法的類別。
        /// </summary>
        private Evaluator Evaluator;

        private ExpressionTree LastTreeSingle;

        /// <summary>
        /// 建構子
        /// </summary>
        public ExpressionTreeManager()
        {
            TreeStack.Push(new ExpressionTree());
            Evaluator = new Evaluator();
            LastTreeSingle = new ExpressionTree();
        }

        /// <summary>
        /// 新增一個新數字
        /// </summary>
        /// <param name="number">新數字</param>
        public void Add(decimal number)
        {
            TreeStack.Peek().Add(number);
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
        
        /// <summary>
        /// 取得答案以及需要補足的右括號數量
        /// </summary>
        /// <returns>答案以及右括號的數量</returns>
        public Result TryGetResult()
        {
            if (TreeStack.Count == 1)
            {
                //新功能
                LastTreeSingle = TreeUtils.CloneTree(TreeStack.Peek());

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

                //新功能
                LastTreeSingle = TreeUtils.CloneTree(TreeStack.Peek());

                var ans = Evaluator.EvaluateTree(TreeStack.Pop());
                TreeStack.Push(new ExpressionTree());
                return new Result(ans, extraRightBracket);
            }
            else
            {
                throw new Exception("資料有誤");
            }
        }

        /// <summary>
        /// 取得最外層的計算結果
        /// </summary>
        /// <returns>計算結果</returns>
        public decimal TryGetTmpResult()
        {
            //暫時計算的結果只需要到第一層
            if (TreeStack.Count > 0)
            {
                var cloneTree = TreeUtils.CloneTree(TreeStack.Peek());

                return Evaluator.EvaluateTree(cloneTree);
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
        /// 取得樹頂左節點被更換後的計算結果
        /// </summary>
        /// <param name="newLeftNodeNumber">要更換的數字</param>
        /// <returns>計算結果</returns>
        public decimal GetLastTreeReplaceLeftResult(decimal newLeftNodeNumber)
        {
            
            var top = LastTreeSingle.GetTop();
            
            Node newLeftNode = Node.Build()
                                   .SetParentNode(top)
                                   .SetNumber(newLeftNodeNumber)
                                   .Exec();
            //一定要設定Root
            LastTreeSingle.Root = newLeftNode;
            top.LeftNode = newLeftNode;

            return Evaluator.EvaluateTree(TreeUtils.CloneTree(LastTreeSingle));
        }

        /// <summary>
        /// 取得樹頂右半邊的計算結果(不包含樹頂)
        /// </summary>
        /// <returns>計算結果</returns>
        public decimal GetLastTreeRightResult()
        {
            var tree = TreeUtils.CloneTree(LastTreeSingle);
            
            var newTop = tree.GetTop().RightNode;
            newTop.ParentNode = null;

            //分成節點是數字或非數字的情況
            if (newTop.NodeValue.Number.HasValue)
            {
                return newTop.NodeValue.Number.Value;
            }
            
            return Evaluator.EvaluateNode(newTop);
        }

        /// <summary>
        /// 取得樹頂的運算符
        /// </summary>
        /// <returns>樹頂運算符</returns>
        public string GetLastTreeTopOperator()
        {
            return LastTreeSingle.GetTop().NodeValue.Operator.Name.ToString();
        }

        /// <summary>
        /// Clear事件
        /// </summary>
        public void Clear()
        {
            TreeStack = new Stack<ExpressionTree>();
            TreeStack.Push(new ExpressionTree());
        }
    }
}