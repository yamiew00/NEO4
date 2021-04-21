using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calculator.News;
using Calculator.Exceptions;
using System.Configuration;
using Calculator.Setting;
using Calculator.Networks.Request;

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
            
            //連線初始化
            //Task.Run(async () =>
            //{
            //    await NetworkController.Instance.InitRequest();
            //});

            //更新UserId
            Global.UpdateUserId();
            
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

            //畫面更新
            //UpdateFrame(tag, text);

            //新
            Bond bond = new Bond(feature: tag, content: Convert.ToChar(text));

            var response = await NetworkController.Instance.Request(bond);
            Console.WriteLine("out panel = " + response.Panel);
            TextBoxPanel.Text = response.Panel;
            TextBoxSubPanel.Text = response.SubPanel;
            
        }

        /// <summary>
        /// 畫面更新
        /// </summary>
        /// <param name="tag">button的tag</param>
        /// <param name="text">button的text</param>
        private async void UpdateFrame(string tag, string text)
        {
            //按照tag做事
            if (tag.Equals("Number"))
            {
                var update = await NetworkController.Instance.NumberRequest(Convert.ToChar(text));

                string updateString = update.UpdateString;
                int removeLength = (update.RemoveLength >= 0) ? update.RemoveLength : TextBoxPanel.Text.Length;
                UpdatePanel(TextBoxPanel, removeLength, updateString);
            }
            else if (tag.Equals("Operator"))
            {
                var update = await NetworkController.Instance.BinaryRequest(Convert.ToChar(text));

                string updateString = update.UpdateString;
                int removeLength = update.RemoveLength;
                UpdatePanel(TextBoxPanel, removeLength, updateString);
            }
            else if (tag.Equals("Equal"))
            {
                try
                {
                    var update = await NetworkController.Instance.EqualRequest();

                    string updateString = update.Update.UpdateString;
                    string answer = update.answer.ToString();
                    AppendText(TextBoxPanel, updateString);
                    TextBoxSubPanel.Text = answer;
                }
                catch (Exception exception)
                {
                    if (exception is Exception400)
                    {
                        TextBoxSubPanel.Text = exception.Message;
                        return;
                    }
                }
            }
            else if (tag.Equals("LeftBracket"))
            {
                var update = await NetworkController.Instance.LeftBracketRequest();
                AppendText(TextBoxPanel, update.UpdateString);
            }
            else
            {
                UpdateFrame2(tag, text);
            }
        }

        /// <summary>
        /// 畫面更新
        /// </summary>
        /// <param name="tag">button的tag</param>
        /// <param name="text">button的text</param>
        private async void UpdateFrame2(string tag, string text)
        {
            if (tag.Equals("RightBracket"))
            {
                var update = await NetworkController.Instance.RightBracketRequest();
                AppendText(TextBoxPanel, update.UpdateString);
            }
            else if (tag.Equals("Clear"))
            {
                var isClear = await NetworkController.Instance.ClearRequest();
                if (isClear)
                {
                    TextBoxPanel.Text = string.Empty;
                    TextBoxSubPanel.Text = string.Empty;
                }
            }
            else if (tag.Equals("ClearError"))
            {
                var update = await NetworkController.Instance.ClearErrorRequest();
                
                UpdatePanel(TextBoxPanel, update.RemoveLength, "0");
            }
            else if (tag.Equals("BackSpace"))
            {
                var update = await NetworkController.Instance.BackSpaceRequest();
                int removeLength = update.RemoveLength;
                string updateString = update.UpdateString;
                UpdatePanel(TextBoxPanel, removeLength, updateString);
            }
            else if (tag.Equals("Unary"))
            {
                try
                {
                    var update = await NetworkController.Instance.UnaryRequest(Convert.ToChar(text));
                    int removeLength = update.RemoveLength;
                    string updateString = update.UpdateString;
                    UpdatePanel(TextBoxPanel, removeLength, updateString);
                }
                catch (Exception exception)
                {
                    if (exception is Exception400)
                    {
                        TextBoxSubPanel.Text = exception.Message;
                    }
                }
            }
        }

        private void UpdatePanel(TextBox textBox, int removeLength, string updateString)
        {
            RemoveText(textBox, removeLength);
            AppendText(textBox, updateString);
        }

        /// <summary>
        /// 讓TextBox拓展字串
        /// </summary>
        /// <param name="textBox">textBox</param>
        /// <param name="str">新字串</param>
        private void AppendText(TextBox textBox, string str)
        {
            textBox.Text += str;
        }

        /// <summary>
        /// 讓TextBox移除字串
        /// </summary>
        /// <param name="textBox">textBox</param>
        /// <param name="length">要移除的長度</param>
        private void RemoveText(TextBox textBox, int length)
        {
            var text = textBox.Text;
            textBox.Text = text.Substring(0, text.Length - length);
        }
    }
}
