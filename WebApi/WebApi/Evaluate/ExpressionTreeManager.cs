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
        

        /// <summary>
        /// 建構子
        /// </summary>
        public ExpressionTreeManager()
        {
            TreeStack.Push(new ExpressionTree());
            Evaluator = new Evaluator();
        }

        //Binary, Equal,RightBracket
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
        
        //都是Equal在用
        /// <summary>
        /// 取得答案以及需要補足的右括號數量
        /// </summary>
        /// <returns>答案以及右括號的數量</returns>
        public Result TryGetResult()
        {
            if (TreeStack.Count == 1)
            {
                //算完不能pop
                var answer = Evaluator.EvaluateTree(TreeStack.Peek());
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
                
                //算完不能pop
                var ans = Evaluator.EvaluateTree(TreeStack.Peek());
                return new Result(ans, extraRightBracket);
            }
            else
            {
                throw new Exception("資料有誤");
            }
        }

        //Binary, RightBracket
        /// <summary>
        /// 取得最外層的計算結果
        /// </summary>
        /// <returns>計算結果</returns>
        public decimal TryGetTmpResult()
        {
            //暫時計算的結果只需要到第一層
            if (TreeStack.Count > 0)
            {
                var outerTree = TreeStack.Peek();
                return Evaluator.EvaluateTree(outerTree);
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
        
        //EqualCombo1
        public void ReplaceLeft(decimal newNumber)
        {
            //在這個方法被呼叫之前，TreeStack已經收束成一棵完整的樹了，也就是TreeStack.Count = 1
            var tree = TreeStack.Peek();
            var top = tree.GetTop();

            System.Diagnostics.Debug.WriteLine($"Here2");
            //新的左節點
            Node newLeftNode = Node.Build()
                       .SetParentNode(top)
                       .SetNumber(newNumber)
                       .Exec();
            top.LeftNode = newLeftNode;
            tree.Root = newLeftNode;   
        }

        //EqualCombo2
        public Node GetTop()
        {
            return TreeStack.Peek().GetTop();
        }

        //EqualCombo3
        public decimal GetRightHalf()
        {
            var top = TreeStack.Peek().GetTop();
            if (top.RightNode == null)
            {
                return 0;
            }

            return Evaluator.PackNode(top.RightNode);
        }
    }
}