using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebApi.Extensions;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 執行順序的規則
    /// </summary>
    public class OrderingRule
    {
        /// <summary>
        /// 順序字典。key:先前執行的type；value:可在key之後執行的所有type
        /// </summary>
        private static Dictionary<Type, HashSet<Type>> OrderingDic = new Dictionary<Type, HashSet<Type>>();

        /// <summary>
        /// 載入順位字典
        /// </summary>
        public static void LoadOrdering()
        {
            var types = typeof(Feature).GetAllTypes();

            //順位字典的輸入
            foreach (var type in types)
            {
                var props = type.GetProperties();

                var obj = Activator.CreateInstance(type);

                var previousSet = (HashSet<Type>)type.GetProperty("PreviousType").GetValue(obj);

                var afterSet = (HashSet<Type>)type.GetProperty("AfterWardType").GetValue(obj);
                
                foreach (var previous in previousSet)
                {
                    AddToDic(previous, type);
                }

                foreach (var after in afterSet)
                {
                    AddToDic(type, after);
                }   
            }
        }

        /// <summary>
        /// 新增到OrderingDic的方法
        /// </summary>
        /// <param name="key">key(Type)</param>
        /// <param name="value">value(Type)</param>
        private static void AddToDic(Type key, Type value)
        {
            if (OrderingDic.Keys.Contains(key))
            {
                if (OrderingDic[key].Contains(value))
                {
                    return;
                }
                OrderingDic[key].Add(value);
                return;
            }
            else
            {
                OrderingDic.Add(key, new HashSet<Type>() { value });
            }
        }

        /// <summary>
        /// 判斷執行順序是否有誤
        /// </summary>
        /// <param name="lastType">先前的執行功能</param>
        /// <param name="currentType">現在的執行功能</param>
        /// <returns></returns>
        public static bool IsOrderLegit(Type lastType, Type currentType)
        {
            if (OrderingDic.Keys.Contains(lastType))
            {
                return OrderingDic[lastType].Contains(currentType);
            }
            else
            {
                throw new Exception("IFeature類型錯誤");
            }
        }
    }
}