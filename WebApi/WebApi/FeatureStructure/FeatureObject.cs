using System;
using System.Collections.Generic;
using WebApi.DataBase;
using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// Feature的抽象類別。
    /// </summary>
    public abstract class FeatureObject
    {
        /// <summary>
        /// 用戶Id
        /// </summary>
        protected int UserId { get; set; }

        /// <summary>
        /// 指令內容
        /// </summary>
        protected char Content { get; set; }

        /// <summary>
        /// 用戶資訊
        /// </summary>
        private UserInfo UserInfo;

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
        /// 空建構子。反射用的
        /// </summary>
        protected FeatureObject()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userId">用戶id</param>
        protected FeatureObject(int userId)
        {
            UserId = userId;
            UserInfo = Users.GetUserInfoDic(userId);
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="content">功能內容</param>
        protected FeatureObject(int userId, char content) : this(userId)
        {
            Content = content;
        }

        /// <summary>
        /// 用戶資訊初始化
        /// </summary>
        protected void InfoInit()
        {
            UserInfo.Init();
        }
    }
}