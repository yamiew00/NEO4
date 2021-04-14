using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Controllers;
using Calculator.Objects;
using Calculator.Extensions;

namespace Calculator.Frames
{
    /// <summary>
    /// 單元運算符功能
    /// </summary>
    public class Unary : ITag
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static readonly Unary _instance = new Unary();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static Unary Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 私有建構子
        /// </summary>
        private Unary()
        {
        }

        /// <summary>
        /// 回傳啟用表
        /// </summary>
        /// <returns>啟用表</returns>
        public List<string> GetEnableList()
        {
            return new List<string>() { "Operator", "RightBracket", "Equal", "Clear", "BackSpace", "Unary" };
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
                InputController.Instance.AddUnary(text);
                var LastNumberStr = InputController.Instance.NumberStr;
                
                frameObject.PanelString = frameObject.PanelString.RemoveLast(LastNumberStr.Length);
                frameObject.AppendPanel(text);
                frameObject.AppendPanel(LastNumberStr);
            });
        }
    }
}
