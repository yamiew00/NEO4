using System;
using System.Collections.Generic;
using WebApi.Evaluate.Tree;
using WebApi.Evaluate.Utils;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// Feature抽象類，存各種方法
    /// </summary>
    public abstract class IFeature : OrderedFeature
    {
        /// <summary>
        /// 合法的前一次功能集
        /// </summary>
        protected HashSet<Feature> FeatureSet { get; set; }

        /// <summary>
        /// 例外字典
        /// </summary>
        private readonly Dictionary<Type, Func<FrameObject, int, FrameObject>> ExceptionDic = new Dictionary<Type, Func<FrameObject, int, FrameObject>>()
        {
            {
                //BracketException:括號數量不正確→畫面維持原狀
                typeof(BracketException),  (frameObject, userId) => frameObject
            },
            {
                //TooLongDigitException:輸入位數限制→畫面維持原狀
                typeof(TooLongDigitException), (frameObject, userId) => frameObject
            },
            {
                typeof(SquareRootException), (frameObject, userId) =>
                {
                   //對負號做根號運算→回傳錯訊
                    var subpanel = frameObject.SubPanel;

                    //清除
                    new Clear(userId).GetFrameObject();
                    
                    return new FrameObject(panel: "無效的輸入", subPanel: subpanel);
                }
            },
            {
                typeof(DivideByZeroException), (frameObject, userId) =>
                {
                    //零在分母→回傳錯訊
                    var subpanel = frameObject.SubPanel;

                    //清除
                    new Clear(userId).GetFrameObject();

                    return new FrameObject(panel: "無法除以零。", subPanel: subpanel);
                }
            }
        };

        public IFeature()
        {

        }
        
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userId">用戶id</param>
        public IFeature(int userId) : base(userId)
        {
            FeatureSet = SetPreviousFeatures();
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="content">功能內容</param>
        public IFeature(int userId, char content) : base(userId, content)
        {
            FeatureSet = SetPreviousFeatures();
        }

        /// <summary>
        /// 回傳畫面物件的方法。在這邊做順序判斷
        /// </summary>
        /// <returns>畫面物件</returns>
        public FrameObject GetFrameObject()
        {
            //若順序不合法則返回原畫面
            if (!(OrderingRule.IsOrderLegit(LastFeature, this.GetType())))
            {
                return FrameObject;
            }
            
            //其他exception的情況
            try
            {
                var frameObject = CreateFrameObject();
                //若執行成功，則記錄下這一次執行的IFeature的type
                LastFeature = this.GetType();

                return frameObject;
            }
            catch (Exception exception)
            {
                //查詢例外字典
                if (ExceptionDic.ContainsKey(exception.GetType()))
                {
                    return ExceptionDic[exception.GetType()].Invoke(FrameObject, UserId);
                }
                else
                {
                    //沒處理到的情況→直接死機
                    throw new Exception(exception.Message);
                }
            }
        }

        /// <summary>
        /// 設定實體物件的合法順序規則
        /// </summary>
        /// <returns>合法順序集</returns>
        protected abstract HashSet<Feature> SetPreviousFeatures();

        /// <summary>
        /// 根據OrderingDealer方法的回傳值，製造畫面物件。
        /// </summary>
        /// <returns>畫面面件</returns>
        protected abstract FrameObject CreateFrameObject();

        /// <summary>
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected abstract FrameUpdate OrderingDealer();
                
        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected abstract FrameUpdate Tree();

        /// <summary>
        /// 回傳當下的運算樹計算結果
        /// </summary>
        /// <returns>運算樹的計算結果</returns>
        protected Result GetResult()
        {
            if (TreeStack.Count == 1)
            {
                //算完不能pop
                var answer = new Evaluator().EvaluateTree(TreeStack.Peek());
                return new Result(answer, 0);
            }
            else if (TreeStack.Count > 1)
            {
                int extraRightBracket = 0;
                //這裡不用tmp嗎，再想想
                while (TreeStack.Count > 1)
                {
                    var subTree = TreeStack.Pop();

                    TreeStack.Peek().Add(subTree);
                    extraRightBracket++;
                }

                //算完不能pop
                var ans = new Evaluator().EvaluateTree(TreeStack.Peek());
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
        /// <returns>最外層子樹的計算結果</returns>
        protected decimal GetTmpResult()
        {
            //暫時計算的結果只需要到第一層
            if (TreeStack.Count > 0)
            {
                var outerTree = TreeStack.Peek();
                return new Evaluator().EvaluateTree(outerTree);
            }
            else if (TreeStack.Count == 0)
            {
                return 0;
            }
            else
            {
                throw new Exception("資料有誤");
            }
        }
    }
}
