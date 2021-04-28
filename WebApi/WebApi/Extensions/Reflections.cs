using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Extensions
{
    /// <summary>
    /// 反射的擴充方法
    /// </summary>
    public static class Reflections
    {
        /// <summary>
        /// 回傳繼承了特定Type的所有Type
        /// </summary>
        /// <param name="type">父類</param>
        /// <returns>所有子類</returns>
        public static IEnumerable<Type> GetAllTypes(this Type type)
        {
            return type.Assembly.GetTypes().Where(subType => subType.IsSubclassOf(type));
        }
    }
}