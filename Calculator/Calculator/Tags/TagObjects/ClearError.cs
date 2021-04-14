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
    /// 清除錯誤(CE)功能
    /// </summary>
    public class ClearError : ITag
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static readonly ClearError _instance = new ClearError();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static ClearError Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 私有建構子
        /// </summary>
        private ClearError()
        {
        }

        /// <summary>
        /// 回傳啟用表
        /// </summary>
        /// <returns></returns>
        public List<string> GetEnableList()
        {
            return new List<string>() { "Number", "Operator", "LeftBracket", "RightBracket", "Equal", "Clear" };
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
                var BackLength = InputController.Instance.NumberStr.Length + 2;
                frameObject.PanelString = frameObject.PanelString.RemoveLast(BackLength);
                InputController.Instance.ClearError();
            });
        }
    }
}
