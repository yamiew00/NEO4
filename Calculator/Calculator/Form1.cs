using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calculator.Tools;
using Calculator.Extensions;
using Calculator.Controllers;
using Calculator.Setting;
using System.Linq.Expressions;
using Calculator.Tools.Lambdas;

namespace Calculator
{
    /// <summary>
    /// 主頁面
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            //靜態面板初始化
            Panel = TextBoxPanel;
            SubPanel = TextBoxSubPanel;

            //手動更新運算符字典
            OperatorController.GetInstance()
                .SetRules(this);
        }

        /// <summary>
        /// 主數字面板
        /// </summary>
        private static TextBox Panel;

        /// <summary>
        /// 運算式面板
        /// </summary>
        private static TextBox SubPanel;

        /// <summary>
        /// 全按鈕點擊事件
        /// </summary>
        /// <param name="sender">按鈕</param>
        /// <param name="e">點擊事件</param>
        private void All_Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;

            FrameLogic.WhatToDo(button.Text, this);
        }

        /// <summary>
        /// 讓Panel顯示指定文字，(為了讓其他類別呼叫這個control用的
        /// </summary>
        /// <param name="str">指字文字</param>
        public void PanelShow(string str)
        {
            Panel.ShowText(str);
        }

        /// <summary>
        /// 讓SubPanel顯示指定文字，(為了讓其他類別呼叫這個control用的
        /// </summary>
        /// <param name="str">指字文字</param>
        public void SubPanelShow(string str)
        {
            SubPanel.ShowText(str);
        }
    }
}
