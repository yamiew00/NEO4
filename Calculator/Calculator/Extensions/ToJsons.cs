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
    public static class ToJsons
    {
        public static string ToJson(this Expression expression)
        {
            return new JsonBuilder().SetStringValue(JsonSetting.OPERATOR_STRING, expression.BinaryOperatorMark)
                                    .SetObjectValue(JsonSetting.NUMBER_STRING, expression.Number)
                                    .SetObjectValue(JsonSetting.LEFTBRACKET_STRING, expression.LeftBracket)
                                    .SetObjectValue(JsonSetting.RIGHTBRACKET_STRING, expression.RightBracket)
                                    .SetStringListValue(JsonSetting.UNARYLIST_STRING, expression.UnaryList)
                                    .ToString();
        }

        public static string ToJson(this EqualExpression equalExpression)
        {
           return new JsonBuilder().SetObjectValue(JsonSetting.NUMBER_STRING, equalExpression.Number)
                                   .SetObjectValue(JsonSetting.RIGHTBRACKET_STRING, equalExpression.RightBracket)
                                   .SetStringListValue(JsonSetting.UNARYLIST_STRING, equalExpression.UnaryList)
                                   .ToString();
        }
    }
}
