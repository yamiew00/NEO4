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
using Calculator.News;


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

            //新Clear事件
            Task.Run(async () =>
            {
                await NewNetworkController.Instance.ClearRequest();
            });
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

            //var frameObject = new FrameObject(TextBoxPanel, TextBoxSubPanel);

            //var tm = FrameManager.Instance;
            //tm.ChangeTag(tag);
            //await tm.SetFrame(text, frameObject);
            //var enableList = tm.GetEnableList();

            ////按鈕禁用與解禁
            //Enable(enableList);

            ////畫面更新
            //TextBoxPanel.Text = frameObject.PanelString;
            //TextBoxSubPanel.Text = frameObject.SubPanelString;

            if (tag.Equals("Number"))
            {
                var a = await NewNetworkController.Instance.NumberRequest(Convert.ToChar(text));
                Console.WriteLine($"a = {a}");
                AppendText(TextBoxPanel, a);
            }
            else if (tag.Equals("Operator"))
            {
                var a = await NewNetworkController.Instance.BinaryRequest(Convert.ToChar(text));
                string updateString = a.update.updateString;
                int removeLength = a.update.removeLength;
                RemoveText(TextBoxPanel, removeLength);
                AppendText(TextBoxPanel, updateString);
            }
            else if (tag.Equals("Equal"))
            {
                var a = await NewNetworkController.Instance.EqualRequest();
                string updateString = a.Update.UpdateString;
                decimal answer = a.Update.answer;
                AppendText(TextBoxPanel, updateString);
                TextBoxSubPanel.Text = answer.ToString();
            }
            else if (tag.Equals("LeftBracket"))
            {
                var a = await NewNetworkController.Instance.LeftBracketRequest();
                AppendText(TextBoxPanel, a);
            }
            else if (tag.Equals("RightBracket"))
            {
                var a = await NewNetworkController.Instance.RightBracketRequest();
                AppendText(TextBoxPanel, a);
            }
            else if (tag.Equals("Clear"))
            {
                var a = await NewNetworkController.Instance.ClearRequest();
                if (a)
                {
                    TextBoxPanel.Text = string.Empty;
                    TextBoxSubPanel.Text = string.Empty;
                }
            }
            else if (tag.Equals("ClearError"))
            {
                var a = await NewNetworkController.Instance.ClearErrorRequest();
                Console.WriteLine(a);
                RemoveText(TextBoxPanel, a);
                //補0這個也要問後端才對
                AppendText(TextBoxPanel, "0");
            }
            else if (tag.Equals("BackSpace"))
            {
                var a = await NewNetworkController.Instance.BackSpaceRequest();
                int removeLength = a.Update.RemoveLength;
                string updateString = a.Update.UpdateString;
                RemoveText(TextBoxPanel, removeLength);
                AppendText(TextBoxPanel, updateString);
            }
            else if (tag.Equals("Unary"))
            {
                var a = await NewNetworkController.Instance.UnaryRequest(Convert.ToChar(text));
                int removeLength = a.Update.RemoveLength;
                string updateString = a.Update.UpdateString;
                RemoveText(TextBoxPanel, removeLength);
                AppendText(TextBoxPanel, updateString);

            }

        }

        private void AppendText(TextBox textBox, string str)
        {
            textBox.Text += str;
        }

        private void RemoveText(TextBox textBox, int length)
        {
            var text = textBox.Text;
            textBox.Text = text.Substring(0, text.Length - length);
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
