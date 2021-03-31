using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calculator.CalculateObject;
using Calculator.Tools;
using Calculator.Tools.OperateObject;
using Calculator.Extensions;
using Calculator.Controllers;

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
            
        }
        
        //以下新的
        private string Expression;

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

        private static ExpressionTree ExpressionTree = ExpressionTree.GetInstance();

        /// <summary>
        /// 全按鈕點擊事件
        /// </summary>
        /// <param name="sender">按鈕</param>
        /// <param name="e">點擊事件</param>
        private void All_Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            InputController.NewInput(button.Text);
            Panel.ShowText(ExpressionTree.Expression);
            //Console.WriteLine($"ExpressionTree.Expression = {ExpressionTree.Expression}");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var ans = ExpressionTree.Result();
            SubPanel.ShowText($"= {ans}");
        }
    }
}
