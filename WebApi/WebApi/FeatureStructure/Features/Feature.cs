using System;
using System.Collections.Generic;
using WebApi.DataBase;
using WebApi.Evaluate.Tree;
using WebApi.Evaluate.Utils;
using WebApi.Exceptions;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// Feature抽象類，存各種方法
    /// </summary>
    public abstract class Feature : IBoard, ICompute
    {
        /// <summary>
        /// 功能內容
        /// </summary>
        protected char Content { get; set; }

        /// <summary>
        /// 例外字典
        /// </summary>
        private readonly Dictionary<Type, Func<FrameObject, UserInfo, FrameObject>> ExceptionDic = new Dictionary<Type, Func<FrameObject, UserInfo, FrameObject>>()
        {
            {
                //BracketException:括號數量不正確→畫面維持原狀
                typeof(BracketException),  (frameObject, userInfo) => frameObject
            },
            {
                //TooLongDigitException:輸入位數限制→畫面維持原狀
                typeof(TooLongDigitException), (frameObject, userInfo) => frameObject
            },
            {
                typeof(SquareRootException), (frameObject, userInfo) =>
                {
                   //對負號做根號運算→回傳錯訊
                    var subpanel = frameObject.SubPanel;

                    //清除
                    userInfo.Init();

                    return new FrameObject(panel: "無效的輸入", subPanel: subpanel);
                }
            },
            {
                typeof(DivideByZeroException), (frameObject, userInfo) =>
                {
                    //零在分母→回傳錯訊
                    var subpanel = frameObject.SubPanel;

                    //清除
                    userInfo.Init();

                    return new FrameObject(panel: "無法除以零。", subPanel: subpanel);
                }
            }
        };

        /// <summary>
        /// 可執行的前一個Type
        /// </summary>
        public abstract HashSet<Type> PreviousType { get; }

        /// <summary>
        /// 可執行的後一個Type
        /// </summary>
        public abstract HashSet<Type> AfterWardType { get; }

        /// <summary>
        /// 空建構子。
        /// </summary>
        public Feature()
        {
        }
        
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="content">功能內容</param>
        public Feature(char content)
        {
            Content = content;
        }

        /// <summary>
        /// 回傳當下的運算樹計算結果
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>計算結果</returns>
        protected Result GetResult(ComputeObject computeObject)
        {
            if (computeObject.TreeStack.Count == 1)
            {
                //算完不能pop
                var answer = new Evaluator().EvaluateTree(computeObject.TreeStack.Peek());
                return new Result(answer, 0);
            }
            else if (computeObject.TreeStack.Count > 1)
            {
                int extraRightBracket = 0;
                //這裡不用tmp嗎，再想想
                while (computeObject.TreeStack.Count > 1)
                {
                    var subTree = computeObject.TreeStack.Pop();

                    computeObject.TreeStack.Peek().Add(subTree);
                    extraRightBracket++;
                }

                //算完不能pop
                var ans = new Evaluator().EvaluateTree(computeObject.TreeStack.Peek());
                return new Result(ans, extraRightBracket);
            }
            else
            {
                throw new Exception("資料有誤");
            }
        }

        /// <summary>
        /// 回傳當下最外層子樹的計算結果
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>計算結果</returns>
        protected decimal GetTmpResult(ComputeObject computeObject)
        {
            //暫時計算的結果只需要到第一層
            if (computeObject.TreeStack.Count > 0)
            {
                var outerTree = computeObject.TreeStack.Peek();
                return new Evaluator().EvaluateTree(outerTree);
            }
            else if (computeObject.TreeStack.Count == 0)
            {
                return 0;
            }
            else
            {
                throw new Exception("資料有誤");
            }
        }
        
        ///// <summary>
        ///// 回傳此功能前面可以接的功能集
        ///// </summary>
        ///// <returns>前面可以接的功能集</returns>
        //public abstract HashSet<Type> LegitPreviousType();

        ///// <summary>
        ///// 回傳此功能後面可以接的功能集
        ///// </summary>
        ///// <returns>後面可以接的功能集</returns>
        //public abstract HashSet<Type> LegitAfterWardType();

        /// <summary>
        /// 依功能回傳畫面物件
        /// </summary>
        /// <param name="boardObject">面板物件</param>
        /// <param name="frameUpdate">畫面更新</param>
        /// <returns>畫面物件</returns>
        public abstract FrameObject GetFrameObject(BoardObject boardObject, FrameUpdate frameUpdate);

        /// <summary>
        /// 依計畫內容回傳畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        public abstract FrameUpdate Compute(ComputeObject computeObject);

        /// <summary>
        /// 處理例外狀況後回傳畫面物件
        /// </summary>
        /// <param name="userInfo">用戶資訊</param>
        /// <returns>畫面物件</returns>
        public FrameObject GetAnswer(UserInfo userInfo)
        {
            if (!(OrderingRule.IsOrderLegit(userInfo.ComputeObject.LastFeature, this.GetType())))
            {
                return userInfo.BoardObject.FrameObject;
            }

            try
            {
                //特殊狀況處理
                if (userInfo.ComputeObject.LastFeature == typeof(Equal) && this.GetType() == typeof(Number))
                {
                    userInfo.BoardObject.Init();
                }

                var frameObject = GetFrameObject(userInfo.BoardObject, Compute(userInfo.ComputeObject));

                userInfo.ComputeObject.LastFeature = this.GetType();
                return frameObject;
            }
            catch (Exception exception)
            {
                //查詢例外字典
                if (ExceptionDic.ContainsKey(exception.GetType()))
                {
                    //這裡不太對
                    return ExceptionDic[exception.GetType()].Invoke(userInfo.BoardObject.FrameObject, userInfo);
                }
                else
                {
                    //沒處理到的情況→直接死機
                    throw new Exception(exception.Message);
                }
            }
        }
    }
}
