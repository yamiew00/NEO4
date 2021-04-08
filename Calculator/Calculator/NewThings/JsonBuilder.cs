using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.NewThings
{
    public class JsonBuilder
    {
        public string JsonString;
        private int count;

        public JsonBuilder()
        {
            JsonString = "{";
            count = 0;
        }

        public JsonBuilder SetStringValue(string key, string value)
        {
            if (count == 0)
            {
                JsonString += $"\"{key}\" : \"{value}\"";
                count ++;
            }
            else
            {
                JsonString += $",\"{key}\" : \"{value}\"";
                count ++;
            }
            return this;
        }

        public JsonBuilder SetObjectValue(string key, object value)
        {   
            //ToLower是為了處理false.toString()會變成False的問題。Json中的布林值必須是小寫
            if (count == 0)
            {
                JsonString += $"\"{key}\" : {value.ToString().ToLower()}";
                count++;
            }
            else
            {
                JsonString += $",\"{key}\" : {value.ToString().ToLower()}";
                count ++;
            }
            return this;
        }

        public override string ToString()
        {
            JsonString += "}";
            return JsonString;
        }
    }
}
