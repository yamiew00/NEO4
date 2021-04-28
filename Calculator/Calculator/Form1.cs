using System;
using System.Windows.Forms;
using Calculator.News;
using Calculator.Setting;
using Calculator.Networks.Request;
using System.Collections.Generic;
using System.Net.Http;

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
            
            Bond bond = new Bond(feature: tag, content: Convert.ToChar(text));
            try
            {
                var response = await NetworkController.Instance.Request(bond);

                TextBoxPanel.Text = response.Panel;
                TextBoxSubPanel.Text = response.SubPanel;
            }
            catch (Exception exception)
            {
                if (exception is HttpRequestException)
                {
                    MessageBox.Show("無網路連線");
                    System.Environment.Exit(0);
                }
            }
        }
    }
}
