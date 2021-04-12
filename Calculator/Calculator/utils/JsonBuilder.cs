using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.utils
{
    /// <summary>
    /// 做Json字串用的
    /// </summary>
    public class JsonBuilder
    {
        /// <summary>
        /// Json字串
        /// </summary>
        private string JsonString;

        /// <summary>
        /// 個數
        /// </summary>
        private int Count;

        /// <summary>
        /// 建構子
        /// </summary>
        public JsonBuilder()
        {
            JsonString = "{";
            Count = 0;
        }

        /// <summary>
        /// 設定單個值(字串)
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value(字串)</param>
        /// <returns>JsonBuilder</returns>
        public JsonBuilder SetStringValue(string key, string value)
        {
            if (Count == 0)
            {
                JsonString += $"\"{key}\" : \"{value}\"";
                Count++;
            }
            else
            {
                JsonString += $",\"{key}\" : \"{value}\"";
                Count++;
            }
            return this;
        }

        /// <summary>
        /// 設定單個值(非字串)
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value(非字串)</param>
        /// <returns>JsonBuilder</returns>
        public JsonBuilder SetObjectValue(string key, object value)
        {
            //ToLower是為了處理false.toString()會變成False的問題。Json中的布林值必須是小寫
            string valueString = (value == null) ? "null" : value.ToString().ToLower();
            
            if (Count == 0)
            {
                JsonString += $"\"{key}\" : {valueString}";
                Count++;
            }
            else
            {
                JsonString += $",\"{key}\" : {valueString}";
                Count++;
            }
            return this;
        }

        /// <summary>
        /// 設定List(字串)
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="list">List Value(字串)</param>
        /// <returns>JsonBuilder</returns>
        public JsonBuilder SetStringListValue(string key, List<string> list)
        {
            string value = "[";
            for (int i = 0; i < list.Count(); i++)
            {
                if (i == 0)
                {
                    value += $"\"{list[i]}\"";
                }
                else
                {
                    value += $",\"{list[i]}\"";
                }
            }
            value += "]";

            if (Count == 0)
            {
                JsonString += $"\"{key}\" : {value}";
            }
            else
            {
                JsonString += $",\"{key}\" : {value}";
            }

            return this;
        }

        /// <summary>
        /// 轉成最終Json格式
        /// </summary>
        /// <returns>Json String</returns>
        public override string ToString()
        {
            JsonString += "}";
            return JsonString;
        }
    }
}
