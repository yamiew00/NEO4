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
    /// Clear功能
    /// </summary>
    public class Clear : ITag
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static readonly Clear _instance = new Clear();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static Clear Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 私有建構子
        /// </summary>
        private Clear()
        {
        }

        /// <summary>
        /// 回傳啟用表
        /// </summary>
        /// <returns></returns>
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
        public Task SetFrame(string text, FrameObject frameObject)
        {
            return Task.Run(() => 
            {
                InputController.Instance.Clear();
                NetworkController.Instance.ClearRequest();
                frameObject.PanelString = string.Empty;
            });   
        }
    }
}
