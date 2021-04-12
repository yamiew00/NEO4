using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    /// <summary>
    /// 運算樹的節點
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
        /// 此節點的值(數值或是運算子擇一)
        /// </summary>
        public NodeValue Value { get; set; }

        /// <summary>
        /// 值的類別。如果有值，則其中一者必為空值
        /// </summary>
        public class NodeValue
        {
            /// <summary>
            /// 運算子
            /// </summary>
            public BinaryOperator Operator { get; set; }

            /// <summary>
            /// 數值(可空)
            /// </summary>
            public decimal? Number { get; set; }

            /// <summary>
            /// 運算子建構子
            /// </summary>
            /// <param name="Operator">運算子</param>
            public NodeValue(BinaryOperator Operator)
            {
                this.Operator = Operator;
                this.Number = null;
            }

            /// <summary>
            /// 數值建樣子
            /// </summary>
            /// <param name="Number">數值</param>
            public NodeValue(decimal? Number)
            {
                this.Operator = null;
                this.Number = Number;
            }
        }

        /// <summary>
        /// 建造者模式
        /// </summary>
        /// <returns>建造者實體</returns>
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
            /// 節點值
            /// </summary>
            private NodeValue Value;

            /// <summary>
            /// 建造者模式(父節點)
            /// </summary>
            /// <param name="parentNode">父節點</param>
            /// <returns>建造者實體</returns>
            public Builder SetParentNode(Node parentNode)
            {
                this.ParentNode = parentNode;
                return this;
            }

            /// <summary>
            /// 建造者模式(左子節點)
            /// </summary>
            /// <param name="leftNode">左子節點</param>
            /// <returns>建造者實體</returns>
            public Builder SetLeftNode(Node leftNode)
            {
                this.LeftNode = leftNode;
                return this;
            }

            /// <summary>
            /// 建造者模式(右子節點)
            /// </summary>
            /// <param name="rightNode">右子節點</param>
            /// <returns>建造者實體</returns>
            public Builder SetRightNode(Node rightNode)
            {
                this.RightNode = rightNode;
                return this;
            }

            /// <summary>
            /// 建造者模式(數值)
            /// </summary>
            /// <param name="value">數值</param>
            /// <returns>建造者實體</returns>
            public Builder SetValue(NodeValue value)
            {
                this.Value = value;
                return this;
            }

            /// <summary>
            /// 建造者模式(運算子)
            /// </summary>
            /// <param name="Operator">運算子</param>
            /// <returns>建造者實體</returns>
            public Builder SetOperator(BinaryOperator Operator)
            {
                this.Value = new NodeValue(Operator);
                return this;
            }

            /// <summary>
            /// 建造者模式(數值)
            /// </summary>
            /// <param name="number">數值</param>
            /// <returns>建造者實體</returns>
            public Builder SetNumber(decimal? number)
            {
                this.Value = new NodeValue(number);
                return this;
            }

            /// <summary>
            /// 回傳設定好的節點
            /// </summary>
            /// <returns>設定好的節點</returns>
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
        public Node(Node parentNode, Node leftNode, Node rightNode, NodeValue value)
        {
            ParentNode = parentNode;
            LeftNode = leftNode;
            RightNode = rightNode;
            this.Value = value;
        }
        
        /// <summary>
        /// 判斷此節點是否為運算子
        /// </summary>
        /// <returns>是運算子的布林值</returns>
        public bool IsOperator()
        {
            return (Value == null) ? false : Value.Operator != null;
        }
    }
}