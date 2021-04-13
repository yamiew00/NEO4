using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Objects
{
    /// <summary>
    /// 運算樹
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        /// 根節點
        /// </summary>
        public Node Root { get; set; }

        /// <summary>
        /// 當前節點
        /// </summary>
        public Node CurrentNode { get; set; }

        /// <summary>
        /// 前次節點。為了括號而存在的
        /// </summary>
        public Node LastCurrentNode { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public ExpressionTree()
        {
            Root = null;
            CurrentNode = null;
        }

        /// <summary>
        /// 新增運算符
        /// </summary>
        /// <param name="Operator">運算符</param>
        public void Add(BinaryOperator Operator)
        {
            //第一個運算case
            if (Root.ParentNode == null)
            {
                Root.ParentNode = Node.Build()
                                      .SetLeftNode(Root)
                                      .SetOperator(Operator)
                                      .Exec();
                CurrentNode = Root.ParentNode;
                return;
            }

            //非第一個運算case，則一直找到優先度的最高點。從底往上升，升到優先度比自己小為止
            Node tmp = CurrentNode;
            while (tmp.ParentNode != null
                && (tmp.ParentNode.NodeValue.Operator != null
                && tmp.ParentNode.NodeValue.Operator.Priority >= Operator.Priority))   
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

        /// <summary>
        /// 新增數字節點
        /// </summary>
        /// <param name="number">數字</param>
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
                LastCurrentNode = CurrentNode;
                System.Diagnostics.Debug.WriteLine($"here {LastCurrentNode == null}");
            }
        }

        /// <summary>
        /// 修改運算符
        /// </summary>
        /// <param name="Operator">運算符</param>
        public void ModifyOperator(BinaryOperator Operator)
        {
            if (CurrentNode.ParentNode == null)
            {
                //當前節點在樹頂的case
                CurrentNode.LeftNode.ParentNode = null;
                CurrentNode = LastCurrentNode;
                System.Diagnostics.Debug.WriteLine($"{CurrentNode == null}");
                System.Diagnostics.Debug.WriteLine($"{LastCurrentNode == null}");
            }
            else
            {
                //當前節點不在樹頂的case
                CurrentNode.LeftNode.ParentNode = CurrentNode.ParentNode;
                CurrentNode.ParentNode.RightNode = CurrentNode.LeftNode;
                CurrentNode = CurrentNode.LeftNode; //多加這行了
            }
            //修改完之後再新增
            Add(Operator);
        }

        /// <summary>
        /// 加一棵子樹進來
        /// </summary>
        /// <param name="tree">子樹</param>
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

        /// <summary>
        /// 取得頂部節點
        /// </summary>
        /// <returns>頂部節點</returns>
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