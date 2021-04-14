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
    /// 雙元運算符功能
    /// </summary>
    public class Operator : ITag
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static readonly Operator _instance = new Operator();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static Operator Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 私有建構子
        /// </summary>
        private Operator()
        {
        }

        /// <summary>
        /// 畫面設定
        /// </summary>
        /// <param name="text">Control的text</param>
        /// <param name="frameObject">畫面物件</param>
        /// <returns>Task</returns>
        public async Task SetFrame(string text, FrameObject frameObject)
        {
            InputController.Instance.SetOperator(text);
            frameObject.AppendPanel(text);

            //裡面也有畫面處理
            await NetworkController.Instance.OperatorRequest(InputController.Instance.CreateOperatorExpression(), frameObject);
        }

        /// <summary>
        /// 回傳啟用表
        /// </summary>
        /// <returns>啟用表</returns>
        public List<string> GetEnableList()
        {
            return new List<string>() { "Number", "LeftBracket", "Clear", "Operator" };
        }
    }
}
