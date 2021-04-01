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

            //更新字典
            OperatorController.GetInstance()
                .SetRules(this);



            //測試
            var ans = test("").Invoke(2, 3);
            //Console.WriteLine(ans);


            //測試2
            IEvaluator ie = new IEvaluator();
            List<string> list = new List<string>() { "x", "y", "+" ,"5", "*"};
            var func = ie.InfixToDelegate(list);
            //Console.WriteLine(func(3, 13));

            //測試3，正常程序起始點
            ITree itree = new ITree();
            List<string> test3Infix = itree.ToInfix("(a + b) * a + 1");
            var Infix = test3Infix.Print();

            //Console.WriteLine($"test3Str = {Infix}");

            
            //測試4
            var PostFix = itree.InfixToPostFix(test3Infix);
            //Console.WriteLine($"PostFix.Print() = {PostFix.Print()}");

            //測試5，正常程序結束點
            IEvaluator ieTest5 = new IEvaluator();
            var funcFinal = ieTest5.InfixToDelegate(PostFix);
            //Console.WriteLine($"Final ans = {funcFinal(2, 5)}");

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
        /// 輸入器實體
        /// </summary>
        private static InputController InputController = InputController.GetInstance();

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

        public void PanelShow(string str)
        {
            Panel.ShowText(str);
        }

        public void SubPanelShow(string str)
        {
            SubPanel.ShowText(str);
        }


        //測試用
        private Func<decimal, decimal, decimal> test(string str)
        {
            ParameterExpression a = Expression.Parameter(typeof(decimal), "a");
            ParameterExpression b = Expression.Parameter(typeof(decimal), "b");
            ConstantExpression constant = Expression.Constant(2M);

            BinaryExpression firstStep = Expression.Multiply(b, constant);
            BinaryExpression body = Expression.Add(a, firstStep);

            var test = Expression.Add(a, b);
            var testBody = Expression.Multiply(test, a);

            Expression<Func<decimal, decimal, decimal>> expr = Expression.Lambda<Func<decimal, decimal, decimal>>(testBody, a, b);



            return expr.Compile();
        }


        //接一個後序
        //private Func<decimal, decimal, decimal> test(List<string> postfix)
        //{
        //    ParameterExpression a = Expression.Parameter(typeof(decimal), "a");
        //    ParameterExpression b = Expression.Parameter(typeof(decimal), "b");


        //}
    }
}
