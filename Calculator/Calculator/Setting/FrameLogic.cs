using Calculator.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Controllers;


namespace Calculator.Setting
{
    public static class FrameLogic
    {
        private static InputController IC = InputController.GetInstance();

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
