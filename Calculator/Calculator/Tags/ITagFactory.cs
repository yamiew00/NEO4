using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Frames
{
    /// <summary>
    /// 製造Itag的工廠
    /// </summary>
    public class ITagFactory
    {
        /// <summary>
        /// 按字串製造Itag
        /// </summary>
        /// <param name="tag">tag</param>
        /// <returns>Itag物件</returns>
        public static ITag CreateITag(string tag)
        {
            switch (tag)
            {
                case "BackSpace":
                    return BackSpace.Instance;
                case "Clear":
                    return Clear.Instance;
                case "ClearError":
                    return ClearError.Instance;
                case "Equal":
                    return Equal.Instance;
                case "LeftBracket":
                    return LeftBracket.Instance;
                case "Number":
                    return Number.Instance;
                case "Operator":
                    return Operator.Instance;
                case "RightBracket":
                    return RightBracket.Instance;
                case "Unary":
                    return Unary.Instance;
                default:
                    throw new Exception("No such ITag");
            }
        }
    }
}
