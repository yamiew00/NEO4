using Calculator.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Controllers;

namespace Calculator.Setting
{
    /// <summary>
    /// 畫面邏輯的設定
    /// </summary>
    public static class FrameLogic
    {
        /// <summary>
        /// 輸入器
        /// </summary>
        private static InputController IC = InputController.GetInstance();

        /// <summary>
        /// 指令集
        /// </summary>
        private static List<Commands> CommandList = new List<Commands>
        {
            new Commands("=", (form) => 
            {
                decimal ans = IC.GetResult();
                //秀畫面
                form.SubPanelShow($"{IC.GetExpression()} = ");
                form.PanelShow(ans.ToString());
            }),
            new Commands("C", (form) => 
            {
                IC.Clear();
                //秀畫面
                form.SubPanelShow("");
                form.PanelShow("");
            }),
            new Commands("CE", (form) => 
            {
                IC.ClearError();
                //秀畫面
                form.SubPanelShow($"{IC.GetExpression()}");
                form.PanelShow("0");
            }),
            new Commands("⌫", (form) => 
            {
                IC.BackSpace();
                //秀畫面
                form.SubPanelShow("");
                form.PanelShow(IC.GetExpression());
            })
        };

        /// <summary>
        /// 按指令集做事，不在指令集中的就直接傳給輸入器
        /// </summary>
        /// <param name="command">指令</param>
        /// <param name="form">指定的Form</param>
        public static void WhatToDo(string command, Form1 form)
        {
            var action = CommandList.Where(x => x.Command == command);

            //action不為null表現畫面層有要處理事情，null則將接到的任何東西往裡面傳
            if (action.Count() > 0)
            {
                action.First().Action.Invoke(form);
            }
            else
            {
                IC.Input(command);
                form.PanelShow(IC.GetExpression());
            }
        }
    }
}
