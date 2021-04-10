using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    public class ExpressionTree
    {
        public Node Root { get; set; }
        public Node CurrentNode { get; set; }

        /// <summary>
        /// Add Number之後才會更新，AddOperator不會
        /// </summary>
        public Node LastCurrentNode { get; set; }

        public ExpressionTree()
        {
            Root = null;
            CurrentNode = null;
        }

        //假定運算符不會連續發生(才怪)
        public void Add(BinaryOperator Operator)
        {
            System.Diagnostics.Debug.WriteLine($"CurrentNode == null: {CurrentNode == null}");

            //第一個運算
            if (Root.ParentNode == null)
            {
                Root.ParentNode = Node.Build()
                                      .SetLeftNode(Root)
                                      .SetOperator(Operator)
                                      .Exec();
                CurrentNode = Root.ParentNode;
                return;
            }

            //非第一個，則一直找到優先度的最高點
            Node tmp = CurrentNode;
            while (tmp.ParentNode != null
                && (tmp.ParentNode.value.Operator != null
                && tmp.ParentNode.value.Operator.Priority >= Operator.Priority))   //從底往上升，升到優先度比自己小為止
            {
                tmp = tmp.ParentNode;
            }
            
            //處於最高點，以及非最高點的case
            if (tmp.ParentNode == null)
            {
                //樹頂
                //雙向關係
                Node newParentNode = Node.Build()
                                         .SetLeftNode(tmp)
                                         .SetOperator(Operator)
                                         .Exec();
                tmp.ParentNode = newParentNode;

                CurrentNode = newParentNode;
            }
            else
            {
                //非樹頂→生出旁枝
                Node newRightNode = Node.Build()
                                        .SetParentNode(tmp.ParentNode)
                                        .SetOperator(Operator)
                                        .SetLeftNode(tmp)
                                        .Exec();
                tmp.ParentNode.RightNode = newRightNode;
                tmp.ParentNode = newRightNode;

                CurrentNode = newRightNode;
            }
        }

        public void Add(decimal number)
        {
            //第一個數字
            if (Root == null)
            {
                Root = Node.Build()
                           .SetNumber(number)
                           .Exec();
                CurrentNode = Root;
                LastCurrentNode = Root;
            }

            //非第一個數字，先預設輸入順序正確，也就是currentNode是運算符
            if (CurrentNode.IsOperator())
            {
                CurrentNode.RightNode = Node.Build()
                                            .SetParentNode(CurrentNode)
                                            .SetNumber(number)
                                            .Exec();
                CurrentNode = CurrentNode.RightNode;
                LastCurrentNode = CurrentNode.RightNode;
            }
        }

        //應該沒錯吧?
        public void ModifyOperator(BinaryOperator Operator)
        {
            //分成樹頂與非樹頂的狀態(不計括號)
            if (CurrentNode.ParentNode == null)
            {
                CurrentNode.LeftNode.ParentNode = null;
                CurrentNode = LastCurrentNode;
            }
            else
            {
                CurrentNode.LeftNode.ParentNode = CurrentNode.ParentNode;
                CurrentNode.ParentNode.RightNode = CurrentNode.LeftNode;
                CurrentNode = CurrentNode.LeftNode; //多加這行了
            }
            //修改完之後再新增
            Add(Operator);
        }

        //加一棵子樹進來，這裡寫得有點怪
        public void Add(ExpressionTree tree)
        {
            //如果是第一個數字
            if (Root == null)
            {
                this.Root = tree.Root;
                this.CurrentNode = tree.CurrentNode;
            }

            if (CurrentNode.IsOperator())
            {
                Node topNode = tree.GetTop();
                CurrentNode.RightNode = topNode;
                topNode.ParentNode = CurrentNode;

                //主樹現有node必須是子樹的頂端
                CurrentNode = topNode;
            }

            this.LastCurrentNode = tree.GetTop();

        }

        public Node GetTop()
        {
            //防呆
            if (Root == null)
            {
                throw new Exception("不可為空");
            }

            Node tmp = CurrentNode;
            while (tmp.ParentNode != null)
            {
                tmp = tmp.ParentNode;
            }
            return tmp;
        }
    }
}