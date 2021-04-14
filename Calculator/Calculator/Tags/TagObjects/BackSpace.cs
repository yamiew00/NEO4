using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Controllers;
using Calculator.Extensions;
using Calculator.Objects;

namespace Calculator.Frames
{
    /// <summary>
    /// 返回鍵
    /// </summary>
    public class BackSpace : ITag
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static readonly BackSpace _instance = new BackSpace();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static BackSpace Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 私有建構子
        /// </summary>
        private BackSpace()
        {
        }

        /// <summary>
        /// 回傳啟用表
        /// </summary>
        /// <returns>啟用表</returns>
        public List<string> GetEnableList()
        {
            return new List<string>() { "Number", "Operator", "LeftBracket", "RightBracket", "Equal", "Clear", "ClearError", "BackSpace", "Unary" };
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
                InputController.Instance.BackSpace();
                frameObject.PanelString = frameObject.PanelString.RemoveLast(1);
            });
        }
    }
}