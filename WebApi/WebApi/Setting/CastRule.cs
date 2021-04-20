using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Setting
{
    /// <summary>
    /// 功能鍵的輸入規則
    /// </summary>
    public class CastRule
    {
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public static readonly string INCORRECT_ORDER_MSG = "輸入順序有誤，因此無響應";
        
        /// <summary>
        /// 功能種類
        /// </summary>
        public enum Cast
        {
            Null,
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

        /// <summary>
        /// 順序字典。key之後可以接著使用value的種類
        /// </summary>
        public static readonly Dictionary<Cast, List<Cast>> LegitOrder = new Dictionary<Cast, List<Cast>>()
        {
            { Cast.Null, new List<Cast>(){ Cast.NUMBER, Cast.BINARY, Cast.EQUAL, Cast.LEFT_BRACKET, Cast.UNARY} },
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

        /// <summary>
        /// 判斷輸入順序的正確性
        /// </summary>
        /// <param name="former">前者</param>
        /// <param name="latter">後者</param>
        /// <returns>正確性的布林值</returns>
        public static bool IsTheOrderingLegit(Cast former, Cast latter)
        {
            return LegitOrder[former].Contains(latter);
        }
    }
}