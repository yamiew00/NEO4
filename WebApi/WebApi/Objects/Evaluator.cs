using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    public class Evaluator
    {
        public decimal EvaluateTree(ExpressionTree tree)
        {
            Node top = tree.GetTop();
            decimal ans = PackNode(top).value.Number ?? 0;
            return ans;
        }

        /// <summary>
        /// 必須傳進運算符
        /// </summary>
        /// <param name="node"></param>
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


            var ans = Compute(node.value.Operator, node.LeftNode.value.Number ?? 0, node.RightNode.value.Number ?? 0); //不可能為null，放0只是為了讓編譯過

            Node newNode = Node.Build()
                                .SetParentNode(node)
                                .SetNumber(ans)
                                .Exec();

            return newNode;
        }

        public decimal Compute(BinaryOperator Operator, decimal num1, decimal num2)
        {
            return Operator.Formula(num1, num2);
        }
    }
}