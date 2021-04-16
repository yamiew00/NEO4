using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Setting
{
    public class CastRule
    {
        //public static readonly string NUMBER = "Number";
        //public static readonly string BINARY = "Binary";
        //public static readonly string EQUAL = "Equal";
        //public static readonly string LEFT_BRACKET = "LeftBracket";
        //public static readonly string RIGHT_BRACKET = "RightBracket";
        //public static readonly string CLEAR = "Clear";
        //public static readonly string CLEAR_ERROR = "ClearError";
        //public static readonly string BACKSPACE = "BackSpace";
        //public static readonly string UNARY = "Unary";

        public static readonly string INCORRECT_ORDER_MSG = "輸入順序有誤，因此無響應";

        //需不需要null呢?
        public enum Cast {
            NUMBER,
            BINARY,
            EQUAL,
            LEFT_BRACKET,
            RIGHT_BRACKET,
            CLEAR,
            CLEAR_ERROR,
            BACKSPACE,
            UNARY
        }
        public static readonly Dictionary<Cast, List<Cast>> LegitOrder = new Dictionary<Cast, List<Cast>>()
        {
            { Cast.NUMBER, new List<Cast>(){ Cast.NUMBER, Cast.BINARY, Cast.EQUAL, Cast.RIGHT_BRACKET, Cast.CLEAR, Cast.CLEAR_ERROR, Cast.BACKSPACE, Cast.UNARY } },
            { Cast.BINARY, new List<Cast>(){ Cast.NUMBER, Cast.BINARY, Cast.LEFT_BRACKET, Cast.CLEAR } },
            { Cast.EQUAL, new List<Cast>(){ Cast.NUMBER, Cast.LEFT_BRACKET, Cast.CLEAR } }, //暫不支援直接接上BINARY
            { Cast.LEFT_BRACKET, new List<Cast>(){ Cast.NUMBER, Cast.RIGHT_BRACKET, Cast.CLEAR } },
            { Cast.RIGHT_BRACKET, new List<Cast>(){ Cast.BINARY, Cast.EQUAL, Cast.CLEAR } },
            { Cast.CLEAR, new List<Cast>(){ Cast.NUMBER, Cast.LEFT_BRACKET } },
            { Cast.CLEAR_ERROR, new List<Cast>(){ Cast.NUMBER, Cast.BINARY, Cast.LEFT_BRACKET, Cast.RIGHT_BRACKET, Cast.EQUAL, Cast.CLEAR } },
            { Cast.BACKSPACE, new List<Cast>(){ Cast.NUMBER, Cast.BINARY, Cast.LEFT_BRACKET, Cast.RIGHT_BRACKET, Cast.EQUAL, Cast.CLEAR, Cast.CLEAR_ERROR, Cast.BACKSPACE, Cast.UNARY } }, //LEFT_BRACKET case很可怕
            { Cast.UNARY, new List<Cast>(){ Cast.BINARY, Cast.RIGHT_BRACKET, Cast.EQUAL, Cast.CLEAR, Cast.UNARY } }
        };

        public static bool IsTheOrderingLegit(Cast former, Cast latter)
        {
            return LegitOrder[former].Contains(latter);
        }
    }
}