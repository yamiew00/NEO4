using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.NewTrees
{
    public class NewTree
    {
        public Node Root { get; set; }
        public Node CurrentNode { get; set; }

        public NewTree()
        {
            Root = null;
            CurrentNode = null;
        }

        //先假定運算符不會連續發生
        public void Add(NewBinary Operator)
        {
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



            //處於最高點，以及非最高點
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
            }

            //非第一個數字，先預設輸入順序正確，也就是currentNode是運算符
            if (CurrentNode.IsOperator())
            {
                CurrentNode.RightNode = Node.Build()
                                            .SetParentNode(CurrentNode)
                                            .SetNumber(number)
                                            .Exec();
                CurrentNode = CurrentNode.RightNode;
            }


        }
        
        //加一棵子樹進來
        public void Add(NewTree tree)
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

        }

        public Node GetTop()
        {
            //防呆
            if (Root == null)
            {
                throw new Exception("不可為空");
            }

            Node tmp = CurrentNode;
            while(tmp.ParentNode != null)
            {
                tmp = tmp.ParentNode;
            }
            return tmp;
        }
        

    }
}
