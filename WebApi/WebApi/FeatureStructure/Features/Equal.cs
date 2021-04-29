using System;
using System.Collections.Generic;
using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Evaluate.Utils;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 等號鍵:Concrete IFeature物件
    /// </summary>
    public class Equal : Feature
    {
        /// <summary>
        /// 空建構子。反射用的
        /// </summary>
        public Equal()
        {
        }

        /// <summary>
        /// 以一個數字替換Tree的左半部
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <param name="newNumber">新數字</param>
        private void ReplaceLeft(ComputeObject computeObject, decimal newNumber)
        {
            //在這個方法被呼叫之前，TreeStack已經收束成一棵完整的樹了，也就是TreeStack.Count = 1
            var tree = computeObject.TreeStack.Peek();
            var top = tree.GetTop();
            
            //新的左節點
            Node newLeftNode = Node.Build()
                       .SetParentNode(top)
                       .SetNumber(newNumber)
                       .Exec();
            top.LeftNode = newLeftNode;
            tree.Root = newLeftNode;
        }

        /// <summary>
        /// 回傳Tree右半部的計算結果
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>計算結果</returns>
        private decimal GetRightHalf(ComputeObject computeObject)
        {
            var top = computeObject.TreeStack.Peek().GetTop();
            if (top.RightNode == null)
            {
                return 0;
            }

            return new Evaluator().PackNode(top.RightNode);
        }
        
        /// <summary>
        /// 回傳此功能後面可以接的功能集
        /// </summary>
        /// <returns>後面可以接的功能集</returns>
        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Equal), typeof(LeftBracket), typeof(Clear), typeof(Unary) };
        }

        /// <summary>
        /// 回傳此功能前面可以接的功能集
        /// </summary>
        /// <returns>前面可以接的功能集</returns>
        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace), typeof(Unary) };
        }

        /// <summary>
        /// 依功能回傳畫面物件
        /// </summary>
        /// <param name="boardObject">面板物件</param>
        /// <param name="frameUpdate">畫面更新</param>
        /// <returns>畫面物件</returns>
        public override FrameObject GetFrameObject(BoardObject boardObject, FrameUpdate frameUpdate)
        {
            //完整運算式的刷新
            boardObject.CompleteExpression = frameUpdate.Refresh(boardObject.CompleteExpression);

            //panel, subpanel設定
            boardObject.FrameObject.SubPanel = boardObject.CompleteExpression;
            boardObject.FrameObject.Panel = frameUpdate.Answer;

            return boardObject.FrameObject;
        }

        /// <summary>
        /// 依計畫內容回傳畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        public override FrameUpdate Compute(ComputeObject computeObject)
        {
            if (computeObject.LastFeature == typeof(Clear))
            {
                return new FrameUpdate("0", removeLength: 0, updateString: "0=");
            }
            else if (computeObject.LastFeature == typeof(Equal))
            {
                //等號連按
                return GetUpdateAfterEqual(computeObject);
            }
            else if (computeObject.LastFeature == typeof(Binary))
            {
                //運算符配等號
                return GetUpdateAfterBinary(computeObject);
            }

            return GetUpdateDefault(computeObject);
        }

        /// <summary>
        /// 回傳連續等號的畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        private FrameUpdate GetUpdateAfterEqual(ComputeObject computeObject)
        {
            string updateString = computeObject.CurrentAnswer.ToString();

            //更新計算答案
            ReplaceLeft(computeObject, computeObject.CurrentAnswer);
            var op = computeObject.TreeStack.Peek().GetTop().NodeValue.Operator.Name;
            var rightResult = GetRightHalf(computeObject);

            updateString += $"{op}{rightResult}=";
            computeObject.CurrentAnswer = GetResult(computeObject).Answer;
            return new FrameUpdate(computeObject.CurrentAnswer.ToString(), removeLength: FrameUpdate.REMOVE_ALL, updateString: updateString);
        }

        /// <summary>
        /// 回傳雙元運算子後接等號的畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        private FrameUpdate GetUpdateAfterBinary(ComputeObject computeObject)
        {
            //CurrentAnswer已經被Binary算好了
            computeObject.TreeStack.Peek().Add(computeObject.CurrentAnswer);

            //result處理
            var result = GetResult(computeObject);
            var ans = result.Answer;

            //做更新字串
            var updateString = computeObject.CurrentAnswer.ToString();
            for (int i = 0; i < result.ExtraRightBracketCount; i++)
            {
                updateString += ")";
            }

            //送出結果
            updateString += "=";

            return new FrameUpdate(ans.ToString(), removeLength: 0, updateString: updateString);
        }

        /// <summary>
        /// 回傳預設的等號事件的畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        private FrameUpdate GetUpdateDefault(ComputeObject computeObject)
        {
            if (computeObject.NumberField != null)
            {
                computeObject.TreeStack.Peek().Add(computeObject.NumberField.Number.Value);
                computeObject.NumberField = null;
            }

            //result處理
            var result = GetResult(computeObject);
            var updateString = string.Empty;
            for (int i = 0; i < result.ExtraRightBracketCount; i++)
            {
                updateString += ")";
            }

            //送出結果
            updateString += "=";
            var ans = result.Answer;

            //暫存答案
            computeObject.CurrentAnswer = ans;
            return new FrameUpdate(ans.ToString(), removeLength: 0, updateString: updateString);
        }
    }
}