using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Evaluate.Tree;

namespace WebApi.Evaluate.Utils
{
    /// <summary>
    /// Tree相關的工具
    /// </summary>
    public class TreeUtils
    {
        /// <summary>
        /// 複製一棵樹
        /// </summary>
        /// <param name="expressionTree">要複製的樹</param>
        /// <returns>clone出來的樹</returns>
        public static ExpressionTree CloneTree(ExpressionTree expressionTree)
        {
            ExpressionTree cloneTree = new ExpressionTree();
            CloneNodes(expressionTree.GetTop(), expressionTree, cloneTree);
            return cloneTree;
        }

        /// <summary>
        /// 遞迴地複製所有的節點到cloneTree上。從基準樹的top節點開始出發。回傳值為standardNode的複製版節點
        /// </summary>
        /// <param name="standardNode">基準節點，當前節點會被複製所有屬性到cloneTree上</param>
        /// <param name="standardTree">基準樹</param>
        /// <param name="cloneTree">複製出來的樹</param>
        /// <returns>對應standardNode的複製節點</returns>
        private static Node CloneNodes(Node standardNode, ExpressionTree standardTree, ExpressionTree cloneTree)
        {
            var duplicateNode = Node.Build()
                                    .SetOperator(standardNode.NodeValue.Operator)
                                    .SetNumber(standardNode.NodeValue.Number)
                                    .Exec();
            CopyAttribute(standardNode, duplicateNode, standardTree, cloneTree);

            if (standardNode.LeftNode != null)
            {
                if (standardNode.LeftNode.IsOperator())
                {
                    var tmpLeftNode = CloneNodes(standardNode.LeftNode, standardTree, cloneTree);

                    tmpLeftNode.ParentNode = duplicateNode;
                    duplicateNode.LeftNode = tmpLeftNode;
                }
                else if (standardNode.LeftNode.NodeValue.Number.HasValue)
                {
                    var tmpLeftNodeWithNumber = Node.Build()
                                                   .SetNumber(standardNode.LeftNode.NodeValue.Number)
                                                   .SetParentNode(duplicateNode)
                                                   .Exec();
                    duplicateNode.LeftNode = tmpLeftNodeWithNumber;
                    CopyAttribute(standardNode.LeftNode, tmpLeftNodeWithNumber, standardTree, cloneTree);
                }
            }
            if (standardNode.RightNode != null)
            {
                if (standardNode.RightNode.IsOperator())
                {
                    Node tmpRightNode = CloneNodes(standardNode.RightNode, standardTree, cloneTree);

                    duplicateNode.RightNode = tmpRightNode;
                    tmpRightNode.ParentNode = duplicateNode;
                }
                else if (standardNode.RightNode.NodeValue.Number.HasValue)
                {
                    var tmpRightNodeWithNumber = Node.Build()
                                                   .SetNumber(standardNode.RightNode.NodeValue.Number)
                                                   .SetParentNode(duplicateNode)
                                                   .Exec();
                    duplicateNode.RightNode = tmpRightNodeWithNumber;
                    //檢查與複製成員
                    CopyAttribute(standardNode.RightNode, tmpRightNodeWithNumber, standardTree, cloneTree);
                }
            }

            return duplicateNode;
        }

        /// <summary>
        /// 檢查standardNode在standardTree上是否為(Root/CurrentRoot/LastCurrentRoot)，是的話將此屬性複製到cloneTree上
        /// </summary>
        /// <param name="standardNode">基準節點</param>
        /// <param name="duplicateNode">複製節點</param>
        /// <param name="standardTree">基準樹</param>
        /// <param name="cloneTree">複製樹</param>
        private static void CopyAttribute(Node standardNode, Node duplicateNode, ExpressionTree standardTree, ExpressionTree cloneTree)
        {
            if (standardNode == standardTree.Root)
            {
                cloneTree.Root = duplicateNode;
            }
            if (standardNode == standardTree.CurrentNode)
            {
                cloneTree.CurrentNode = duplicateNode;
            }
            if (standardNode == standardTree.LastCurrentNode)
            {
                cloneTree.LastCurrentNode = duplicateNode;
            }
        }
    }
}