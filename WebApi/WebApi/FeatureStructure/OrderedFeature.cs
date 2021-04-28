using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.FeatureStructure
{
    public abstract class OrderedFeature : FeatureObject
    {
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

        public abstract HashSet<Type> LegitPreviousType();

        public abstract HashSet<Type> LegitAfterWardType();
    }
}