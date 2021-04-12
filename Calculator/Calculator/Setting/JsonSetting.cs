using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Setting
{
    /// <summary>
    /// 存放Json格式中key的字串
    /// </summary>
    public static class JsonSetting
    {
        /// <summary>
        /// 雙元運算子
        /// </summary>
        public const string OPERATOR_STRING = "BinaryOperator";

        /// <summary>
        /// 數字
        /// </summary>
        public const string NUMBER_STRING = "number";

        /// <summary>
        /// 左括號
        /// </summary>
        public const string LEFTBRACKET_STRING = "LeftBracket";

        /// <summary>
        /// 右括號
        /// </summary>
        public const string RIGHTBRACKET_STRING = "RightBracket";

        /// <summary>
        /// 單元運算子
        /// </summary>
        public const string UNARYLIST_STRING = "UnaryList";
    }
}
