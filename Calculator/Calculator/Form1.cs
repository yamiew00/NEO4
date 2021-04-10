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
        InputController nic = InputController.GetInstance();
        NetworkController nc = NetworkController.GetInstance();
        string PanelString = string.Empty;
        string LastTag;

        /// <summary>
        /// 建構子
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            //靜態面板初始化
            Panel = TextBoxPanel;
            SubPanel = TextBoxSubPanel;

            FrameObject fo = new FrameObject();
            fo.PanelString = "z";
            test(fo);
            
            Console.WriteLine(fo.SubPanelString);
            Console.WriteLine("panel = " + fo.PanelString);
        }
        
        private void test(FrameObject fo)
        {
            
            fo.SubPanelString = "acg";
            fo.AppendPanel("abc");
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
        private async void All_Button_Click(object sender, EventArgs e)
        {
            //按鈕相關屬性
            var button = (Button)sender;
            var text = button.Text;
            var tag = button.Tag.ToString();

            //現有畫面
            var frame = new FrameObject(Panel, SubPanel);

            //等候計算
            await FrameLogic.dealer(frame, tag, text);

            //按鈕禁用與解禁
            Enable(frame.EnableList);

            //畫面更新
            Panel.Text = frame.PanelString;
            SubPanel.Text = frame.SubPanelString;
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

        
        //新做法
        private void LeftBracketLimit()
        {
            foreach(var item in this.Controls)
            {
                if (!(item is Button) || ((Button)item).Tag == null)
                {
                    continue;
                }
                if ((((Button)item).Tag.Equals("Number") || ((Button)item).Tag.Equals("Clear")))
                {
                    ((Button)item).Enabled = true;
                }
                else
                {
                    ((Button)item).Enabled = false;
                }
            }
        }

        private void RightBracketLimit()
        {
            foreach (var item in this.Controls)
            {
                if (!(item is Button) || ((Button)item).Tag == null)
                {
                    continue;
                }
                if (((Button)item).Tag.Equals("Operator") || ((Button)item).Tag.Equals("Equal") || ((Button)item).Tag.Equals("Clear"))
                {
                    ((Button)item).Enabled = true;
                }
                else
                {
                    ((Button)item).Enabled = false;
                }
            }
        }

        private void OperatorLimit()
        {
            foreach (var item in this.Controls)
            {
                if (!(item is Button) || ((Button)item).Tag == null)
                {
                    continue;
                }
                if (((Button)item).Tag.Equals("Number") || ((Button)item).Tag.Equals("LeftBracket") || ((Button)item).Tag.Equals("Clear") || ((Button)item).Tag.Equals("Operator"))
                {
                    ((Button)item).Enabled = true;
                }
                else
                {
                    ((Button)item).Enabled = false;
                }
            }
        }

        private void NumberLimit()
        {
            foreach (var item in this.Controls)
            {
                if (!(item is Button) || ((Button)item).Tag == null)
                {
                    continue;
                }
                if (!((Button)item).Tag.Equals("LeftBracket"))
                {
                    ((Button)item).Enabled = true;
                }
                else
                {
                    ((Button)item).Enabled = false;
                }
            }
        }

        private void EqualLimit()
        {
            foreach (var item in this.Controls)
            {
                if (!(item is Button) || ((Button)item).Tag == null)
                {
                    continue;
                }
                if (((Button)item).Tag.Equals("Number") || ((Button)item).Tag.Equals("LeftBracket") )
                {
                    ((Button)item).Enabled = true;
                }
                else
                {
                    ((Button)item).Enabled = false;
                }
            }
        }

        private void ClearErrorLimit()
        {
            foreach (var item in this.Controls)
            {
                if (item is Button)
                {
                    ((Button)item).Enabled = true;
                }
            }
        }

        private void ClearLimit()
        {
            foreach (var item in this.Controls)
            {
                if (!(item is Button) || ((Button)item).Tag == null)
                {
                    continue;
                }
                if (!((Button)item).Tag.Equals("BackSpace"))
                {
                    ((Button)item).Enabled = true;
                }
                else
                {
                    ((Button)item).Enabled = false;
                }
            }
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
