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
        private Node Root { get; set; }

        /// <summary>
        /// 當前節點
        /// </summary>
        public Node CurrentNode { get; set; }

        /// <summary>
        /// 「上一次的當前節點」。為了「取消」上一次的動作而存在。只有在AddNumber後才會更新，AddOperator不會
        /// </summary>
        private Node LastCurrentNode { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public ExpressionTree()
        {
            Root = null;
            CurrentNode = null;
        }

        /// <summary>
        /// 新增一個運算子(節點)
        /// </summary>
        /// <param name="Operator">運算子</param>
        public void Add(BinaryOperator Operator)
        {
            //是第一個運算
            if (Root.ParentNode == null)
            {
                Root.ParentNode = Node.Build()
                                      .SetLeftNode(Root)
                                      .SetOperator(Operator)
                                      .Exec();
                CurrentNode = Root.ParentNode;
                return;
            }

            //非第一個，則一直找到優先度的最高點。節點從底往上升，升到優先度比自己小為止
            Node tmp = CurrentNode;
            while (tmp.ParentNode != null
                && (tmp.ParentNode.Value.Operator != null
                && tmp.ParentNode.Value.Operator.Priority >= Operator.Priority))   
            {
                tmp = tmp.ParentNode;
            }
            
            //處於最高點，以及非最高點的case
            if (tmp.ParentNode == null)
            {
                //如果是樹頂→頂上枝頭
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
        /// 新增一個數字(節點)
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

            //非第一個數字，預設輸入順序正確，也就是currentNode是運算符
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

        /// <summary>
        /// 取代上次新增的運算符。先讓樹回歸到上一次的狀態，再AddOperator。
        /// </summary>
        /// <param name="Operator">要更新的運算符</param>
        public void ModifyOperator(BinaryOperator Operator)
        {
            if (CurrentNode.ParentNode == null)
            {
                //樹頂的狀態
                CurrentNode.LeftNode.ParentNode = null;
                CurrentNode = LastCurrentNode;
            }
            else
            {
                //非樹頂的狀態
                CurrentNode.LeftNode.ParentNode = CurrentNode.ParentNode;
                CurrentNode.ParentNode.RightNode = CurrentNode.LeftNode;
                CurrentNode = CurrentNode.LeftNode; 
            }
            //修改完之後再新增
            Add(Operator);
        }

        /// <summary>
        /// 加一棵子樹進來。為了括號的功能。
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

            //非第一個數字
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
        /// 取得當前的樹頂節點。
        /// </summary>
        /// <returns>樹頂節點</returns>
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