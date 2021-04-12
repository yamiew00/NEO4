using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    /// <summary>
    /// 運算器
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// 計算出運算樹的計算結果
        /// </summary>
        /// <param name="tree">運算樹</param>
        /// <returns>計算結果</returns>
        public decimal EvaluateTree(ExpressionTree tree)
        {
            Node top = tree.GetTop();
            decimal ans = PackNode(top).Value.Number ?? 0;
            return ans;
        }

        /// <summary>
        /// 將運算符節點及其數字子節點計算，並回傳一個數字節點(必須傳進運算符)
        /// </summary>
        /// <param name="node">運算符節點</param>
        /// <returns>數字節點</returns>
        public Node PackNode(Node node)
        {
            //防呆
            if (!node.IsOperator())
            {
                throw new Exception("必須傳入運算符");
            }

            //若左子節點非數字，則將其算成數字
            if (node.LeftNode.IsOperator())
            {
                node.LeftNode = PackNode(node.LeftNode);
            }

            //若右子節點非數字，則將其算成數字
            if (node.RightNode.IsOperator())
            {
                node.RightNode = PackNode(node.RightNode);
            }

            var ans = Compute(node.Value.Operator, node.LeftNode.Value.Number ?? 0, node.RightNode.Value.Number ?? 0); //不可能為null，放0只是為了讓編譯過

            Node newNode = Node.Build()
                                .SetParentNode(node)
                                .SetNumber(ans)
                                .Exec();

            return newNode;
        }

        /// <summary>
        /// 雙元運算
        /// </summary>
        /// <param name="Operator">雙元運算子</param>
        /// <param name="num1">數字1</param>
        /// <param name="num2">數字2</param>
        /// <returns>運算結果</returns>
        public decimal Compute(BinaryOperator Operator, decimal num1, decimal num2)
        {
            return Operator.Formula(num1, num2);
        }
    }
}