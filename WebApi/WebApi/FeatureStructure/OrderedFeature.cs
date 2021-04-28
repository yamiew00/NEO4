using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 遵守排序的功能
    /// </summary>
    public abstract class OrderedFeature : FeatureObject
    {
        /// <summary>
        /// 空建構子。反射用的
        /// </summary>
        protected OrderedFeature()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userId">用戶id</param>
        protected OrderedFeature(int userId) : base(userId)
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="content">功能內容</param>
        protected OrderedFeature(int userId, char content) : base(userId, content)
        {
        }

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
    }
}