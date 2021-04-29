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
        
    }
}