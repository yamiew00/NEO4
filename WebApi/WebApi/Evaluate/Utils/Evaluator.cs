using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Evaluate.Operators;
using WebApi.Evaluate.Tree;

namespace WebApi.Evaluate.Utils
{
    /// <summary>
    /// 計算器
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// 計算一棵樹的最終答案
        /// </summary>
        /// <param name="tree">樹</param>
        /// <returns>答案</returns>
        public decimal EvaluateTree(ExpressionTree tree)
        {
            var top = tree.GetTop();
            if (top.IsNumber())
            {
                return top.NodeValue.Number.Value;
            }
            var ans = PackNode(tree.GetTop());
            //清除運算雜質
            ClearAllPartial(tree);

            return ans;
        }
        
        /// <summary>
        /// 將指定Node收束成一個數字(該Node及其以下的所有Node視為一棵子樹)
        /// </summary>
        /// <param name="node">節點</param>
        /// <returns>計算結果</returns>
        public decimal PackNode(Node node)
        {
            if (node.NodeValue.Number.HasValue)
            {
                return node.NodeValue.Number.Value;
            }

            //防呆
            if (!node.IsOperator())
            {
                throw new Exception("必須傳入運算符");
            }

            var leftNode = node.LeftNode;
            var rightNode = node.RightNode;

            if (!(leftNode.HasPartial()))
            {
                if (leftNode.IsNumber())
                {
                    leftNode.PartialAnswer = leftNode.NodeValue.Number;
                }
                else if (leftNode.IsOperator())
                {
                    leftNode.PartialAnswer = PackNode(leftNode);
                }
                else
                {
                    throw new Exception("節點必定要是運算元或數字");
                }
            }
            if (!(rightNode.HasPartial()))
            {
                if (rightNode.IsNumber())
                {
                    rightNode.PartialAnswer = rightNode.NodeValue.Number;
                }
                else if (rightNode.IsOperator())
                {
                    rightNode.PartialAnswer = PackNode(rightNode);
                }
                else
                {
                    throw new Exception("節點必定要是運算元或數字");
                }
            }

            var leftPartial = leftNode.PartialAnswer.Value;
            var rightPartial = rightNode.PartialAnswer.Value;
            
            var ans = Compute(node.NodeValue.Operator, leftPartial, rightPartial);
            
            return ans;
        }
        
        /// <summary>
        /// 運算(雙元符號)
        /// </summary>
        /// <param name="Operator">雙元運算子</param>
        /// <param name="num1">數字1</param>
        /// <param name="num2">數字2</param>
        /// <returns>答案</returns>
        private decimal Compute(BinaryOperator Operator, decimal num1, decimal num2)
        {
            return Operator.Formula(num1, num2);
        }

        /// <summary>
        /// 清除指定樹的雜質
        /// </summary>
        /// <param name="tree">樹</param>
        private void ClearAllPartial(ExpressionTree tree)
        {
            if (tree == null)
            {
                return;
            }
            var top = tree.GetTop();
            if (top == null)
            {
                return;
            }
            ClearNodePartial(top);
        }

        /// <summary>
        /// 清除指定節點的雜質
        /// </summary>
        /// <param name="node">節點</param>
        private void ClearNodePartial(Node node)
        {
            node.PartialAnswer = null;
            if (node.LeftNode != null)
            {
                ClearNodePartial(node.LeftNode);
            }
            if (node.RightNode != null)
            {
                ClearNodePartial(node.RightNode);
            }
        }
    }
}