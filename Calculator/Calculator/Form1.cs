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
using Calculator.NewTrees;
using Calculator.NewThings;


namespace Calculator
{
    /// <summary>
    /// 主頁面
    /// </summary>
    public partial class Form1 : Form
    {
        NewInputController nic = NewInputController.GetInstance();
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

            //手動更新運算符字典
            OperatorController.GetInstance()
                .SetRules(this);

            //測試
            NewBinary AddOp = new NewBinary(1, '+', (num1, num2) => num1 + num2);
            NewBinary MinusOP = new NewBinary(1, '-', (num1, num2) => num1 - num2);
            NewBinary MultiplyOp = new NewBinary(2, '*', (num1, num2) => num1 * num2);

            NewTree newTree = new NewTree();
            newTree.Add(1);
            newTree.Add(MultiplyOp);
            newTree.Add(2);
            newTree.Add(AddOp);
            newTree.Add(3);
            newTree.Add(MultiplyOp);
            newTree.Add(4);
            //newTree.Add(AddOp);
            //newTree.Add(5);
            //newTree.Add(MultiplyOp);
            //newTree.Add(6);

            Node root = newTree.Root;
            //Node current = newTree.CurrentNode;
            //Console.WriteLine($"current = {current.ToString()}");

            //測計算
            NewEvaluator newEvaluator = new NewEvaluator();
            //Console.WriteLine($"ans = {newEvaluator.EvaluateTree(newTree)}");

            //controller測試
            NewTreeController ntc = new NewTreeController();
            ntc.Add(1);
            ntc.Add(AddOp);

            ntc.LeftBracket();
            ntc.Add(2);
            ntc.Add(AddOp);
            ntc.Add(3);
            ntc.RightBracket();

            ntc.Add(MultiplyOp);

            ntc.LeftBracket();
            ntc.Add(4);
            ntc.Add(MultiplyOp);
            ntc.Add(5);
            ntc.RightBracket();

            ntc.Add(AddOp);
            ntc.Add(6);
            ntc.Add(MultiplyOp);
            ntc.Add(7);
            Console.WriteLine($"ans = {ntc.GetResult()}");

            //測試
            string num = "30.";
            decimal.TryParse(num, out decimal d);
            Console.WriteLine($"d = {d}");
            
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

            ////原做法
            //FrameLogic.WhatToDo(button.Text, this);
            PanelString += button.Text;
            PanelShow(PanelString);

            //新
            switch (button.Tag.ToString())
            {
                case "Number":
                    NumberLimit();
                    if (!nic.AddNumber(button.Text))
                    {
                        PanelString = PanelString.Substring(0, PanelString.Length - 1);
                        PanelShow(PanelString);
                    }
                    break;
                case "Operator":
                    OperatorLimit();
                    nic.SetOperator(button.Text);
                    nc.OperatorRequest(nic.GenerateCurrentExpression());
                    if (LastTag.Equals("Operator"))
                    {
                        PanelString = PanelString.Substring(0, PanelString.Length - 2);
                        PanelString += button.Text;
                        PanelShow(PanelString);
                    }
                    break;
                case "LeftBracket":
                    nic.SetLeftBracket();
                    LeftBracketLimit();
                    break;
                case "RightBracket":
                    nic.SetRightBracket();
                    RightBracketLimit();
                    break;
                case "Equal":
                    nc.EqualRequest(nic.GenerateCurrentEqualExpression(), this);
                    PanelString = string.Empty;
                    break;
                case "Clear":
                    ClearLimit();
                    nic.Clear();
                    nc.ClearRequest();
                    PanelString = string.Empty;
                    PanelShow(PanelString);
                    break;
                case "ClearError":
                    //limit跟number一樣
                    ClearErrorLimit();
                    var BackLength = nic.NumberStr.Length + 2;
                    PanelString= PanelString.Substring(0, Panel.Text.Length - BackLength);
                    PanelShow(PanelString);
                    nic.ClearError();
                    break;
                case "BackSpace":
                    nic.BackSpace();
                    PanelString = PanelString.Substring(0, PanelString.Length - 2);
                    PanelShow(PanelString);
                    break;
                default:
                    break;
            }
            LastTag = button.Tag.ToString();
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
