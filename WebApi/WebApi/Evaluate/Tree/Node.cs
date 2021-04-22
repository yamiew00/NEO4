using WebApi.Evaluate.Operators;

namespace WebApi.Evaluate.Tree
{
    /// <summary>
    /// 節點
    /// </summary>
    public class Node
    {
        /// <summary>
        /// 父節點
        /// </summary>
        public Node ParentNode { get; set; }

        /// <summary>
        /// 左子節點
        /// </summary>
        public Node LeftNode { get; set; }

        /// <summary>
        /// 右子節點
        /// </summary>
        public Node RightNode { get; set; }

        /// <summary>
        /// 值(數字或運算符)
        /// </summary>
        public Value NodeValue { get; set; }

        /// <summary>
        /// 值。如果有值，則其中一者必為空值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 運算符
            /// </summary>
            public BinaryOperator Operator { get; set; }

            /// <summary>
            /// 數字
            /// </summary>
            public decimal? Number { get; set; }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="Operator">運算符</param>
            public Value(BinaryOperator Operator)
            {
                this.Operator = Operator;
                this.Number = null;
            }

            /// <summary>
            /// 建構子
            /// </summary>
            /// <param name="Number">數字</param>
            public Value(decimal? Number)
            {
                this.Operator = null;
                this.Number = Number;
            }
        }

        /// <summary>
        /// 建造者
        /// </summary>
        /// <returns></returns>
        public static Builder Build()
        {
            return new Builder();
        }

        /// <summary>
        /// 建造者模式
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// 父節點
            /// </summary>
            private Node ParentNode;

            /// <summary>
            /// 左子節點
            /// </summary>
            private Node LeftNode;

            /// <summary>
            /// 右子節點
            /// </summary>
            private Node RightNode;

            /// <summary>
            /// 值
            /// </summary>
            private Value Value;

            /// <summary>
            /// 設定父節點
            /// </summary>
            /// <param name="parentNode">父節點</param>
            /// <returns>建造者實體</returns>
            public Builder SetParentNode(Node parentNode)
            {
                this.ParentNode = parentNode;
                return this;
            }

            /// <summary>
            /// 設定左子節點
            /// </summary>
            /// <param name="leftNode">左子節點</param>
            /// <returns>建造者實體</returns>
            public Builder SetLeftNode(Node leftNode)
            {
                this.LeftNode = leftNode;
                return this;
            }

            /// <summary>
            /// 設定右子節點
            /// </summary>
            /// <param name="rightNode">右子節點</param>
            /// <returns>建造者實體</returns>
            public Builder SetRightNode(Node rightNode)
            {
                this.RightNode = rightNode;
                return this;
            }

            /// <summary>
            /// 設定運算符
            /// </summary>
            /// <param name="Operator">運算符</param>
            /// <returns>建造者實體</returns>
            public Builder SetOperator(BinaryOperator Operator)
            {
                if (Operator == null)
                {
                    return this;
                }
                this.Value = new Value(Operator);
                return this;
            }

            /// <summary>
            /// 設定數字
            /// </summary>
            /// <param name="number">數字</param>
            /// <returns>建造者實體</returns>
            public Builder SetNumber(decimal? number)
            {
                if (number == null)
                {
                    return this;
                }
                this.Value = new Value(number);
                return this;
            }

            /// <summary>
            /// 生產節點
            /// </summary>
            /// <returns>節點</returns>
            public Node Exec()
            {
                return new Node(ParentNode, LeftNode, RightNode, Value);
            }
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="parentNode">父節點</param>
        /// <param name="leftNode">左子節點</param>
        /// <param name="rightNode">右子節點</param>
        /// <param name="value">數值</param>
        public Node(Node parentNode, Node leftNode, Node rightNode, Value value)
        {
            ParentNode = parentNode;
            LeftNode = leftNode;
            RightNode = rightNode;
            this.NodeValue = value;
        }

        /// <summary>
        /// 判斷是否為運算符
        /// </summary>
        /// <returns>是運算符的布林值</returns>
        public bool IsOperator()
        {
            return (NodeValue == null) ? false : NodeValue.Operator != null;
        }
    }
}