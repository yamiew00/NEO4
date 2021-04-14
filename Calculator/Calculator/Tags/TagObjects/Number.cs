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
    /// 數字鍵功能
    /// </summary>
    public class Number : ITag
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static readonly Number _instance = new Number();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static Number Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 私有建構子
        /// </summary>
        private Number()
        {
        }

        /// <summary>
        /// 畫面設定
        /// </summary>
        /// <param name="text">Control的text</param>
        /// <param name="frameObject">畫面物件</param>
        /// <returns>Task</returns>
        public Task SetFrame(string text, FrameObject frameObject)
        {
            return Task.Run(() =>
            {
                if (InputController.Instance.AddNumber(text))
                {
                    frameObject.AppendPanel(text);
                }
            });
        }

        /// <summary>
        /// 回傳啟用表
        /// </summary>
        /// <returns>啟用表</returns>
        public List<string> GetEnableList()
        {
            return new List<string>() { "Number", "Operator", "RightBracket", "Equal", "Clear", "ClearError", "BackSpace", "Unary" };
        }
    }
}
