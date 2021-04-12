using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calculator.Controllers;
using Calculator.Extensions;
using Calculator.Objects;

namespace Calculator.Setting
{
    public static class FrameLogic
    {
        private static NetworkController NetworkController = NetworkController.GetInstance();
        private static InputController InputController = InputController.GetInstance();
        private static string LastTag = string.Empty;

        /// <summary>
        /// 主畫面邏輯。以tag動作，處理了運算、畫面顯示以及按鈕禁用。與NetworkController、InputController連動。
        /// Key: tag； Value: 會回傳(text, frameObject)的Task的委派。
        /// </summary>
        public static Dictionary<string, Func<string, FrameObject, Task>> Actions =new Dictionary<string, Func<string, FrameObject, Task>>()
        {
            {
                //tag, (Button.text, 目前的FrameObject)
                "Number", (text, frameObject) =>
                Task.Run(() =>
                {
                    //判斷新增數字是否成功
                    if (InputController.AddNumber(text))
                    {
                        frameObject.AppendPanel(text);
                    }
                    //禁用表
                    frameObject.SetEnable("Number", "Operator", "RightBracket", "Equal", "Clear", "ClearError", "BackSpace", "Unary");
                })
            },
            {
                //tag, (Button.text, 目前的FrameObject)
                "Operator", (text, frameObject) =>
                Task.Run(() =>
                {
                    InputController.SetOperator(text);
                    NetworkController.OperatorRequest(InputController.GenerateCurrentExpression());
                    if (LastTag.Equals("Operator"))
                    {
                        frameObject.PanelString = frameObject.PanelString.RemoveLast(1);
                    }
                    frameObject.AppendPanel(text);
                    frameObject.SetEnable("Number", "LeftBracket", "Clear", "Operator");
                })
            },
            {
                //tag, (Button.text, 目前的FrameObject)
                "LeftBracket", (text, frameObject) =>
                Task.Run(() =>
                {
                    InputController.SetLeftBracket();
                    frameObject.AppendPanel(text);
                    frameObject.SetEnable("Number","RightBracket", "Clear");
                })
            },
            {
                //tag, (Button.text, 目前的FrameObject)
                "RightBracket", (text, frameObject) =>
                Task.Run(() =>
                {
                    InputController.SetRightBracket();
                    frameObject.AppendPanel(text);
                    frameObject.SetEnable("Operator", "Equal", "Clear");
                })
            },
            {
                //tag, (Button.text, 目前的FrameObject)
                "Equal", (text, frameObject) =>
                Task.Run(async () =>
                {
                    await NetworkController.EqualRequest(InputController.GenerateCurrentEqualExpression(),frameObject);
                    frameObject.AppendPanel(text);
                    frameObject.SetEnable("Number", "LeftBracket");
                })
            },
            {
                //tag, (Button.text, 目前的FrameObject)
                "Clear", (text, frameObject) =>
                Task.Run(() =>
                {
                    InputController.Clear();
                    NetworkController.ClearRequest();
                    frameObject.PanelString = string.Empty;
                    frameObject.SetEnable("Number", "LeftBracket");
                })
            },
            {
                //tag, (Button.text, 目前的FrameObject)
                "ClearError", (text, frameObject) =>
                Task.Run(() =>
                {
                    var BackLength = InputController.NumberStr.Length + 2;
                    frameObject.PanelString = frameObject.PanelString.RemoveLast(BackLength);
                    InputController.ClearError();
                    frameObject.SetEnable("Number", "Operator","LeftBracket", "RightBracket", "Equal", "Clear");
                })
            },
            {
                //tag, (Button.text, 目前的FrameObject)
                "BackSpace", (text, frameObject) =>
                Task.Run(() =>
                {
                    InputController.BackSpace();
                    frameObject.PanelString = frameObject.PanelString.RemoveLast(1);
                    frameObject.SetEnable("Number", "Operator", "LeftBracket", "RightBracket", "Equal", "Clear", "ClearError", "BackSpace", "Unary");
                })
            },
            {
                //tag, (Button.text, 目前的FrameObject)
                "Unary", (text, frameObject) =>
                Task.Run(() =>
                {
                    InputController.AddUnary(text);
                    frameObject.AppendPanel(text);
                    frameObject.SetEnable("Operator","RightBracket", "Equal", "Clear", "BackSpace", "Unary");
                })
            }
        };

        /// <summary>
        /// 根據「上一次執行命令的tag」，來決定該做什麼事
        /// </summary>
        /// <param name="frameObject">目前的frameObject</param>
        /// <param name="tag">control的tag</param>
        /// <param name="text">control的text</param>
        /// <returns></returns>
        public static Task FrameDealer(FrameObject frameObject, string tag, string text)
        {
            return Task.Run(async () =>
            {
                //按完等號之後需要清空畫面
                if (LastTag.Equals("Equal"))
                {
                    frameObject.PanelString = string.Empty;
                    frameObject.SubPanelString = string.Empty;
                }

                //按tag做事
                await Actions[tag](text, frameObject);

                //記錄這次執行命令的tag
                LastTag = tag;
            });
        }
    }
}
