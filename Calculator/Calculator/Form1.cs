using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calculator.Extensions;
using System.Linq.Expressions;
using Calculator.Controllers;
using Calculator.Objects;
using Calculator.Setting;

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
            
            //計算機初始化
            NetworkController.GetInstance().ClearRequest();
        }

        /// <summary>
        /// 全按鈕點擊事件
        /// </summary>
        /// <param name="sender">按鈕</param>
        /// <param name="e">點擊事件</param>
        private async void All_Button_Click(object sender, EventArgs e)
        {
            //按鈕相關屬性
            var button = (Button)sender;
            var text = button.Text;
            var tag = button.Tag.ToString();

            //現有畫面，FrameObject擁有Panel，Subpanel以及按鈕禁用表
            var frame = new FrameObject(TextBoxPanel, TextBoxSubPanel);

            //等候計算
            await FrameLogic.FrameDealer(frame, tag, text);

            //按鈕禁用與解禁
            Enable(frame.EnableList);

            //畫面更新
            TextBoxPanel.Text = frame.PanelString;
            TextBoxSubPanel.Text = frame.SubPanelString;
        }
        
        private void Enable(List<string> enableList)
        {
            foreach (var item in this.Controls)
            {
                if (!(item is Button) || ((Button)item).Tag == null)
                {
                    continue;
                }
                if (enableList.Contains(((Button)item).Tag.ToString()))
                {
                    ((Button)item).Enabled = true;
                }
                else
                {
                    ((Button)item).Enabled = false;
                }
            }
        }
    }
}
