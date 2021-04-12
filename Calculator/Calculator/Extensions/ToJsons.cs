using Calculator.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Calculator.Objects;
using System.Text;
using System.Threading.Tasks;
using Calculator.Setting;

namespace Calculator.Extensions
{
    /// <summary>
    /// Json擴充字元
    /// </summary>
    public static class ToJsons
    {
        /// <summary>
        /// OperatorExpression轉Json
        /// </summary>
        /// <param name="expression">OperatorExpression</param>
        /// <returns>Json字串</returns>
        public static string ToJson(this OperatorExpression expression)
        {
            return new JsonBuilder().SetStringValue(JsonSetting.OPERATOR_STRING, expression.BinaryOperatorMark)
                                    .SetObjectValue(JsonSetting.NUMBER_STRING, expression.Number)
                                    .SetObjectValue(JsonSetting.LEFTBRACKET_STRING, expression.LeftBracket)
                                    .SetObjectValue(JsonSetting.RIGHTBRACKET_STRING, expression.RightBracket)
                                    .SetStringListValue(JsonSetting.UNARYLIST_STRING, expression.UnaryList)
                                    .ToString();
        }

        /// <summary>
        /// EqualExpression轉Json
        /// </summary>
        /// <param name="equalExpression">EqualExpression</param>
        /// <returns>Json字串</returns>
        public static string ToJson(this EqualExpression equalExpression)
        {
           return new JsonBuilder().SetObjectValue(JsonSetting.NUMBER_STRING, equalExpression.Number)
                                   .SetObjectValue(JsonSetting.RIGHTBRACKET_STRING, equalExpression.RightBracket)
                                   .SetStringListValue(JsonSetting.UNARYLIST_STRING, equalExpression.UnaryList)
                                   .ToString();
        }
    }
}
