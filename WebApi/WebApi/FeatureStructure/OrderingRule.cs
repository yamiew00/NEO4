using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.FeatureStructure
{
    public class OrderingRule
    {
        public static Dictionary<Type, HashSet<Type>> OrderingDic = new Dictionary<Type, HashSet<Type>>();

        /// <summary>
        /// 載入順位字典
        /// </summary>
        public static void LoadOrdering()
        {
            var types = typeof(IFeature).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(IFeature)));
            //前位字典
            foreach(var afterType in types)
            {
                var name = afterType.Name;
                var reflect = Activator.CreateInstance(afterType);
                

                HashSet<Type> previousTypeSet = (HashSet<Type>)afterType.GetMethod("LegitPreviousType").Invoke(reflect, null);
                
                foreach (var previousType in previousTypeSet)
                {
                    AddToDic(previousType, afterType);
                }
            }

            //check, ok
            System.Diagnostics.Debug.WriteLine("---1---");
            foreach (var pair in OrderingDic)
            {
                System.Diagnostics.Debug.WriteLine($"{pair.Key.Name}:(count = {pair.Value.Count})");
                System.Diagnostics.Debug.Write("        ");
                foreach (var item in pair.Value)
                {
                    System.Diagnostics.Debug.Write(item.Name + ", ");
                }
                System.Diagnostics.Debug.WriteLine("");
            }

            //後位字典
            foreach(var type in types)
            {
                //var name = afterType.Name;
                var reflect = Activator.CreateInstance(type);

                HashSet<Type> afterTypeSet = (HashSet<Type>)type.GetMethod("LegitAfterWardType").Invoke(reflect, null);
                foreach(var afterType in afterTypeSet)
                {
                    AddToDic(type, afterType);
                }
            }

            //check, ok
            System.Diagnostics.Debug.WriteLine("---2---");
            foreach (var pair in OrderingDic)
            {
                System.Diagnostics.Debug.WriteLine($"{pair.Key.Name}:(count = {pair.Value.Count})");
                System.Diagnostics.Debug.Write("        ");
                foreach (var item in pair.Value)
                {
                    System.Diagnostics.Debug.Write(item.Name + ", ");
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }

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