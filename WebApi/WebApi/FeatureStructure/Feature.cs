using System;
using System.Collections.Generic;
using WebApi.DataBase;
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
    public abstract class Feature
    {
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

        protected char Content;

        protected UserInfo UserInfo;

        /// <summary>
        /// 完整表示式
        /// </summary>
        protected string CompleteExpression
        {
            get
            {
                return UserInfo.CompleteExpression;
            }

            set
            {
                UserInfo.CompleteExpression = value;
            }
        }

        /// <summary>
        /// 畫面物件
        /// </summary>
        protected FrameObject FrameObject
        {
            get
            {
                return UserInfo.FrameObject;
            }

            set
            {
                UserInfo.FrameObject = value;
            }
        }

        /// <summary>
        /// 前次執行功能
        /// </summary>
        protected Type LastFeature
        {
            get
            {
                return UserInfo.LastFeature;
            }

            set
            {
                UserInfo.LastFeature = value;
            }
        }

        /// <summary>
        /// 當前操作數字
        /// </summary>
        protected NumberField NumberField
        {
            get
            {
                return UserInfo.NumberField;
            }

            set
            {
                UserInfo.NumberField = value;
            }
        }

        /// <summary>
        /// 當前運算結果
        /// </summary>
        protected decimal CurrentAnswer
        {
            get
            {
                return UserInfo.CurrentAnswer;
            }

            set
            {
                UserInfo.CurrentAnswer = value;
            }
        }

        /// <summary>
        /// 運算樹堆疊
        /// </summary>
        protected Stack<ExpressionTree> TreeStack
        {
            get
            {
                return UserInfo.TreeStack;
            }

            set
            {
                UserInfo.TreeStack = value;
            }
        }

        /// <summary>
        /// 單元字串
        /// </summary>
        protected string CurrentUnaryString
        {
            get
            {
                return UserInfo.CurrentUnaryString;
            }

            set
            {
                UserInfo.CurrentUnaryString = value;
            }
        }

        /// <summary>
        /// 空建構子。
        /// </summary>
        public Feature()
        {
        }
        
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userId">用戶id</param>
        public Feature(char content)
        {
            Content = content;
        }
        
        /// <summary>
        /// 回傳新增物件的方法
        /// </summary>
        /// <returns>委派</returns>
        public abstract Func<char, Feature> Create();

        /// <summary>
        /// 回傳畫面物件的方法。在這邊做順序判斷
        /// </summary>
        /// <returns>畫面物件</returns>
        public FrameObject GetFrameObject(UserInfo userInfo)
        {
            //要這樣嗎
            UserInfo = userInfo;

            //若順序不合法則返回原畫面
            if (!(OrderingRule.IsOrderLegit(userInfo.LastFeature, this.GetType())))
            {
                return userInfo.FrameObject;
            }
            
            //其他exception的情況
            try
            {
                var frameObject = CreateFrameObject();
                //若執行成功，則記錄下這一次執行的IFeature的type
                userInfo.LastFeature = this.GetType();

                return frameObject;
            }
            catch (Exception exception)
            {
                //查詢例外字典
                if (ExceptionDic.ContainsKey(exception.GetType()))
                {
                    //這裡不太對
                    return ExceptionDic[exception.GetType()].Invoke(userInfo.FrameObject, userInfo);
                }
                else
                {
                    //沒處理到的情況→直接死機
                    throw new Exception(exception.Message);
                }
            }
        }

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
        
        //順序
        /// <summary>
        /// 回傳此功能前面可以接的功能集
        /// </summary>
        /// <returns>前面可以接的功能集</returns>
        public abstract HashSet<Type> LegitPreviousType();

        /// <summary>
        /// 回傳此功能後面可以接的功能集
        /// </summary>
        /// <returns>後面可以接的功能集</returns>
        public abstract HashSet<Type> LegitAfterWardType();

        protected void InitAllInfo()
        {
            UserInfo.Init();
        }
    }
}
