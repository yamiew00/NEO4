using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Setting
{
    /// <summary>
    /// 功能鍵的輸入規則
    /// </summary>
    public class FeatureRule
    {
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public static readonly string INCORRECT_ORDER_MSG = "輸入順序有誤，因此無響應";
        
        /// <summary>
        /// 功能種類
        /// </summary>
        public enum Feature
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
        public static readonly Dictionary<Feature, List<Feature>> LegitOrder = new Dictionary<Feature, List<Feature>>()
        {
            { Feature.Null, new List<Feature>(){ Feature.NUMBER, Feature.BINARY, Feature.EQUAL, Feature.LEFT_BRACKET, Feature.UNARY} },
            { Feature.NUMBER, new List<Feature>(){ Feature.NUMBER, Feature.BINARY, Feature.EQUAL, Feature.RIGHT_BRACKET, Feature.CLEAR, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY } },
            { Feature.BINARY, new List<Feature>(){ Feature.NUMBER, Feature.BINARY, Feature.LEFT_BRACKET, Feature.CLEAR } },
            { Feature.EQUAL, new List<Feature>(){ Feature.NUMBER, Feature.LEFT_BRACKET, Feature.CLEAR } }, //暫不支援直接接上BINARY
            { Feature.LEFT_BRACKET, new List<Feature>(){ Feature.NUMBER, Feature.RIGHT_BRACKET, Feature.CLEAR, Feature.LEFT_BRACKET } },
            { Feature.RIGHT_BRACKET, new List<Feature>(){ Feature.BINARY, Feature.EQUAL, Feature.CLEAR, Feature.RIGHT_BRACKET } },
            { Feature.CLEAR, new List<Feature>(){ Feature.NUMBER, Feature.LEFT_BRACKET } },
            { Feature.CLEAR_ERROR, new List<Feature>(){ Feature.NUMBER, Feature.BINARY, Feature.LEFT_BRACKET, Feature.RIGHT_BRACKET, Feature.EQUAL, Feature.CLEAR } },
            { Feature.BACKSPACE, new List<Feature>(){ Feature.NUMBER, Feature.BINARY, Feature.LEFT_BRACKET, Feature.RIGHT_BRACKET, Feature.EQUAL, Feature.CLEAR, Feature.CLEAR_ERROR, Feature.BACKSPACE, Feature.UNARY } }, 
            { Feature.UNARY, new List<Feature>(){ Feature.BINARY, Feature.RIGHT_BRACKET, Feature.EQUAL, Feature.CLEAR, Feature.UNARY } }
        };

        /// <summary>
        /// 判斷輸入順序的正確性
        /// </summary>
        /// <param name="former">前者</param>
        /// <param name="latter">後者</param>
        /// <returns>正確性的布林值</returns>
        public static bool IsTheOrderingLegit(Feature former, Feature latter)
        {
            return LegitOrder[former].Contains(latter);
        }
    }
}