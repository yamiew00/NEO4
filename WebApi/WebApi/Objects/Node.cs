using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    public class Node
    {
        public Node ParentNode { get; set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }
        public Value value { get; set; }

        /// <summary>
        /// 如果有值，則其中一者必為空值
        /// </summary>
        public class Value
        {
            public BinaryOperator Operator { get; set; }
            public decimal? Number { get; set; }

            public Value(BinaryOperator Operator)
            {
                this.Operator = Operator;
                this.Number = null;
            }

            public Value(decimal? Number)
            {
                this.Operator = null;
                this.Number = Number;
            }
        }


        public static Builder Build()
        {
            return new Builder();
        }

        /// <summary>
        /// 建造者模式
        /// </summary>
        public class Builder
        {
            private Node ParentNode;
            private Node LeftNode;
            private Node RightNode;
            private Value value;

            public Builder SetParentNode(Node parentNode)
            {
                this.ParentNode = parentNode;
                return this;
            }
            public Builder SetLeftNode(Node leftNode)
            {
                this.LeftNode = leftNode;
                return this;
            }
            public Builder SetRightNode(Node rightNode)
            {
                this.RightNode = rightNode;
                return this;
            }
            public Builder SetValue(Value value)
            {
                this.value = value;
                return this;
            }
            public Builder SetOperator(BinaryOperator Operator)
            {
                this.value = new Value(Operator);
                return this;
            }
            public Builder SetNumber(decimal? number)
            {
                this.value = new Value(number);
                return this;
            }
            public Node Exec()
            {
                return new Node(ParentNode, LeftNode, RightNode, value);
            }

        }

        public Node(Node parentNode, Node leftNode, Node rightNode, Value value)
        {
            ParentNode = parentNode;
            LeftNode = leftNode;
            RightNode = rightNode;
            this.value = value;
        }

        public bool IsNumber()
        {
            return (value == null) ? false : value.Number.HasValue;
        }

        public bool IsOperator()
        {
            return (value == null) ? false : value.Operator != null;
        }

    }
}