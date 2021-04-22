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
        /// 計算樹的結果
        /// </summary>
        /// <param name="tree">運算樹</param>
        /// <returns>運算答案</returns>
        public decimal EvaluateTree(ExpressionTree tree)
        {
            System.Diagnostics.Debug.WriteLine($"there 11");
            Node top = tree.GetTop();
            if (top.NodeValue.Number.HasValue)
            {
                return top.NodeValue.Number.Value;
            }
            decimal ans = PackNode(top).NodeValue.Number.Value;
            return ans;
        }

        /// <summary>
        /// 將節點收束成一個數字。必須傳進運算符節點。
        /// </summary>
        /// <param name="node">運算符節點</param>
        /// <returns></returns>
        public Node PackNode(Node node)
        {
            //防呆
            if (!node.IsOperator())
            {
                throw new Exception("必須傳入運算符");
            }

            if (node.LeftNode.IsOperator())
            {
                node.LeftNode = PackNode(node.LeftNode);
            }
            if (node.RightNode.IsOperator())
            {
                node.RightNode = PackNode(node.RightNode);
            }

            var ans = Compute(node.NodeValue.Operator, node.LeftNode.NodeValue.Number.Value, node.RightNode.NodeValue.Number.Value); 

            Node newNode = Node.Build()
                                .SetParentNode(node)
                                .SetNumber(ans)
                                .Exec();

            return newNode;
        }

        /// <summary>
        /// 運算(雙元符號)
        /// </summary>
        /// <param name="Operator">雙元運算子</param>
        /// <param name="num1">數字1</param>
        /// <param name="num2">數字2</param>
        /// <returns>答案</returns>
        public decimal Compute(BinaryOperator Operator, decimal num1, decimal num2)
        {
            return Operator.Formula(num1, num2);
        }
    }
}