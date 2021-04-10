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
        private static string LastTag = string.Empty; //這個該怎麼處理?

        public static Dictionary<string, Func<string, FrameObject, Task>> Actions =new Dictionary<string, Func<string, FrameObject, Task>>()
        {
            {
                "Number", (text, logicObject) =>
                Task.Run(() =>
                {
                    if (InputController.AddNumber(text))
                    {
                        logicObject.AppendPanel(text);
                    }
                    logicObject.SetEnable("Number", "Operator", "RightBracket", "Equal", "Clear", "ClearError", "BackSpace", "Unary");
                })
            },
            {
                "Operator", (text, logicObject) =>
                Task.Run(() =>
                {
                    InputController.SetOperator(text);
                    NetworkController.OperatorRequest(InputController.GenerateCurrentExpression());
                    if (LastTag.Equals("Operator"))
                    {
                        logicObject.PanelString = logicObject.PanelString.RemoveLast(1);
                    }
                    logicObject.AppendPanel(text);
                    logicObject.SetEnable("Number", "LeftBracket", "Clear", "Operator");
                })
            },
            {
                "LeftBracket", (text, logicObject) =>
                Task.Run(() =>
                {
                    InputController.SetLeftBracket();
                    logicObject.AppendPanel(text);
                    logicObject.SetEnable("Number","RightBracket", "Clear");
                })
            },
            {
                "RightBracket", (text, logicObject) =>
                Task.Run(() =>
                {
                    InputController.SetRightBracket();
                    logicObject.AppendPanel(text);
                    logicObject.SetEnable("Operator", "Equal", "Clear");
                })
            },
            {
                "Equal", (text, logicObject) =>
                Task.Run(async () =>
                {
                    //強轉form1真的可以嗎
                    //await Task.Run(async() => await Task.WhenAll(NetworkController.EqualRequest(InputController.GenerateCurrentEqualExpression(),logicObject)));
                    await NetworkController.EqualRequest(InputController.GenerateCurrentEqualExpression(),logicObject);
                    Console.WriteLine("sub = " + logicObject.SubPanelString);
                    logicObject.AppendPanel(text);
                    logicObject.SetEnable("Number", "LeftBracket");
                })
            },
            {
                "Clear", (text, logicObject) =>
                Task.Run(() =>
                {
                    InputController.Clear();
                    NetworkController.ClearRequest();
                    logicObject.PanelString = string.Empty;
                    logicObject.SetEnable("Number", "LeftBracket");
                })
            },
            {
                "ClearError", (text, logicObject) =>
                Task.Run(() =>
                {
                    var BackLength = InputController.NumberStr.Length + 2;
                    logicObject.PanelString = logicObject.PanelString.RemoveLast(BackLength);
                    InputController.ClearError();
                    logicObject.SetEnable("Number", "Operator","LeftBracket", "RightBracket", "Equal", "Clear");
                })
            },
            {
                //Limit沒做
                "BackSpace", (text, logicObject) =>
                Task.Run(() =>
                {
                    InputController.BackSpace();
                    logicObject.PanelString = logicObject.PanelString.RemoveLast(1);
                    logicObject.SetEnable("Number", "Operator", "LeftBracket", "RightBracket", "Equal", "Clear", "ClearError", "BackSpace", "Unary");
                })
            },
            {
                //Limit沒做
                "Unary", (text, logicObject) =>
                Task.Run(() =>
                {
                    InputController.AddUnary(text);
                    logicObject.SetEnable("Operator","RightBracket", "Equal", "Clear", "BackSpace", "Unary");
                })
            }

        };

        public static Task dealer(FrameObject frameObject, string tag, string text)
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

                //記錄下最後一次執行命令的tag
                LastTag = tag;
            });
        }
    }
}
