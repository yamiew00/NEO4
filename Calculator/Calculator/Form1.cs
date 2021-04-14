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
using Calculator.Frames;
using Calculator.Tags;
using System.Configuration;

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
            NetworkController.Instance.ClearRequest();

            //測試
            string keyname = ConfigurationManager.AppSettings["DB"];
            Console.WriteLine(keyname);
        }

        /// <summary>
        /// 全按鈕點擊事件，每個按鈕依tag決定功能類型(Itag)，依text決定功能內容。
        /// </summary>
        /// <param name="sender">按鈕</param>
        /// <param name="e">點擊事件</param>
        private async void All_Button_Click(object sender, EventArgs e)
        {
            //按鈕相關屬性
            var button = (Button)sender;
            var text = button.Text;
            var tag = button.Tag.ToString();

            var frameObject = new FrameObject(TextBoxPanel, TextBoxSubPanel);

            var tm = FrameManager.Instance;
            tm.ChangeTag(tag);
            await tm.SetFrame(text, frameObject);
            var enableList = tm.GetEnableList();
            
            //按鈕禁用與解禁
            Enable(enableList);

            //畫面更新
            TextBoxPanel.Text = frameObject.PanelString;
            TextBoxSubPanel.Text = frameObject.SubPanelString;
        }
        
        /// <summary>
        /// 決定可以使用的按鈕
        /// </summary>
        /// <param name="enableList">啟用表</param>
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
