using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Objects;

namespace WebApi.Evaluate
{
    public class TreeUtils
    {
        private static ExpressionTree StandardTree;
        private static ExpressionTree TmpTree;

        public static ExpressionTree CloneTree(ExpressionTree expressionTree)
        {
            System.Diagnostics.Debug.WriteLine("hereNumber = " + expressionTree.Root.NodeValue.Number);
            var top = expressionTree.GetTop();

            TmpTree = new ExpressionTree();
            StandardTree = expressionTree;

            var recursive = CloneTopNode(top);


            //System.Diagnostics.Debug.WriteLine($"口口口口口口口口口口口口口口口口口口口口");

            //System.Diagnostics.Debug.WriteLine($"check,recursive.LeftNode == null is  {recursive.LeftNode == null}");
            //if (recursive.LeftNode != null)
            //{
            //    System.Diagnostics.Debug.WriteLine($"check,recursive.LeftNode.Number == null is  {recursive.LeftNode.NodeValue.Number}");
            //    System.Diagnostics.Debug.WriteLine($"check,recursive.LeftNode.Operator == null is  {recursive.LeftNode.NodeValue.Operator}");

            //    //第二層
            //    System.Diagnostics.Debug.WriteLine($"check,recursive.LeftNode.LeftNode == null is  {recursive.LeftNode.LeftNode == null}");
            //    if (recursive.LeftNode.LeftNode != null)
            //    {
            //        System.Diagnostics.Debug.WriteLine($"check,recursive.LeftNode.LeftNode.Number == null is  {recursive.LeftNode.LeftNode.NodeValue.Number}");
            //        System.Diagnostics.Debug.WriteLine($"check,recursive.LeftNode.LeftNode.Operator == null is  {recursive.LeftNode.LeftNode.NodeValue.Operator}");
            //    }
            //}




            //System.Diagnostics.Debug.WriteLine($"check,recursive.RightNode == null is  {recursive.RightNode == null}");
            //if (recursive.RightNode != null) { 

            //System.Diagnostics.Debug.WriteLine($"check,recursive.RightNode.Number == null is  {recursive.RightNode.NodeValue.Number}");
            //System.Diagnostics.Debug.WriteLine($"check,recursive.RightNode.Operator == null is  {recursive.RightNode.NodeValue.Operator}");
            //}

            //return recursive;
            return TmpTree;
        }

        private static Node CloneTopNode(Node node)
        {
            
            var duplicateNode = Node.Build()
                                    .SetOperator(node.NodeValue.Operator)
                                    .SetNumber(node.NodeValue.Number)
                                    .Exec();
            //複製成員
            //if (node == StandardTree.Root)
            //{
            //    TmpTree.Root = duplicateNode;
            //}
            //if (node == StandardTree.CurrentNode)
            //{
            //    TmpTree.CurrentNode = duplicateNode;
            //}
            //if (node == StandardTree.LastCurrentNode)
            //{
            //    TmpTree.LastCurrentNode = duplicateNode;
            //}
            CopyAttribute(node, duplicateNode);


            if (node.LeftNode != null)
            {
                if (node.LeftNode.IsOperator())
                {
                    var tmpLeftNode = Node.Build()
                                           .SetOperator(node.LeftNode.NodeValue.Operator)
                                           .SetParentNode(duplicateNode)
                                           .Exec();
                    duplicateNode.LeftNode = CloneTopNode(node.LeftNode);
                }
                else if (node.LeftNode.NodeValue.Number.HasValue)
                {
                    var tmpLeftNodeWithNumber = Node.Build()
                                                   .SetNumber(node.LeftNode.NodeValue.Number)
                                                   .SetParentNode(duplicateNode)
                                                   .Exec();
                    duplicateNode.LeftNode = tmpLeftNodeWithNumber;
                    //檢查與複製成員
                    //if (node.LeftNode == StandardTree.Root)
                    //{
                    //    TmpTree.Root = duplicateNode.LeftNode;
                    //}
                    //if (node.LeftNode == StandardTree.CurrentNode)
                    //{
                    //    TmpTree.CurrentNode = duplicateNode.LeftNode;
                    //}
                    //if (node.LeftNode == StandardTree.LastCurrentNode)
                    //{
                    //    TmpTree.LastCurrentNode = duplicateNode.LeftNode;
                    //}
                    CopyAttribute(node.LeftNode, tmpLeftNodeWithNumber);

                }
                else
                {
                    throw new Exception("資料結構有誤");
                }
            }

            if (node.RightNode != null)
            {
                if (node.RightNode.IsOperator())
                {
                    var tmpRightNode = Node.Build()
                                           .SetOperator(node.RightNode.NodeValue.Operator)
                                           .SetParentNode(duplicateNode)
                                           .Exec();
                    duplicateNode.RightNode = CloneTopNode(node.RightNode);
                }
                else if (node.RightNode.NodeValue.Number.HasValue)
                {
                    var tmpRightNodeWithNumber = Node.Build()
                                                   .SetNumber(node.RightNode.NodeValue.Number)
                                                   .SetParentNode(duplicateNode)
                                                   .Exec();
                    duplicateNode.RightNode = tmpRightNodeWithNumber;

                    //檢查與複製成員
                    CopyAttribute(node.RightNode, tmpRightNodeWithNumber);
                }
            }

            return duplicateNode;
        }

        private static void CopyAttribute(Node standardNode, Node duplicate)
        {
            if (standardNode == StandardTree.Root)
            {
                System.Diagnostics.Debug.WriteLine("root在這");
                TmpTree.Root = duplicate;
            }
            if (standardNode == StandardTree.CurrentNode)
            {
                TmpTree.CurrentNode = duplicate;
            }
            if (standardNode == StandardTree.LastCurrentNode)
            {
                TmpTree.LastCurrentNode = duplicate;
            }
        }
    }
}