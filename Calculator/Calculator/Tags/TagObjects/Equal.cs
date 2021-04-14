using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Controllers;
using Calculator.Objects;

namespace Calculator.Frames
{
    /// <summary>
    /// 等號功能
    /// </summary>
    public class Equal : ITag
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static readonly Equal _instance = new Equal();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static Equal Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 私有建構子
        /// </summary>
        private Equal()
        {
        }

        /// <summary>
        /// 回傳啟用表
        /// </summary>
        /// <returns>啟用表</returns>
        public List<string> GetEnableList()
        {
            return new List<string>() { "Number", "LeftBracket" };
        }

        /// <summary>
        /// 畫面設定
        /// </summary>
        /// <param name="text">Control的text</param>
        /// <param name="frameObject">畫面物件</param>
        /// <returns>Task</returns>
        public async Task SetFrame(string text, FrameObject frameObject)
        {
            await NetworkController.Instance.EqualRequest(InputController.Instance.CreateEqualExpression(), frameObject);
            frameObject.AppendPanel(text);
        }
    }
}
