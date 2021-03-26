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

namespace Calculator
{
    public partial class Form1 : Form
    {
        private NumberField number;

        private CalculateMachine op = new CalculateMachine();

        //字典
        Dictionary<object, Dictionary<string, Action>> dict;
        public Form1()
        {
            InitializeComponent();
            number = new NumberField(0);    //第一個數字CanOperate

            //(字典)初始化
            Init();
        }

        //初始化的測試
        private void Init()
        {
            //
            dict = new Dictionary<object, Dictionary<string, Action>>();

            //定義Tag→加法、減法、乘法
            var operatorType = new { 種類 = "Operator" };
            ButtonAdd.Tag = operatorType;
            ButtonMinus.Tag = operatorType;
            ButtonMultiply.Tag = operatorType;
            ButtonEqual.Tag = operatorType;


            //Operator→加法、減法、乘法
            var operateDic = new Dictionary<string, Action>();
            operateDic.Add("+", () => op.OperateSelector = new Add());
            operateDic.Add("-", () => op.OperateSelector = new Minus());
            operateDic.Add("x", () => op.OperateSelector = new Multiply());
            operateDic.Add("=", () => op.IsDealingEqual = true);

            dict.Add(operatorType, operateDic);
            
        }

        //數字相關按鈕
        private void Button_Click(object sender, EventArgs e)
        {

            //ShowText(((Button)sender).Text);

            //測試tab
            //Console.WriteLine(Button0.Tag == Button2.Tag);
            //Console.WriteLine(Button0.Tag == ButtonEqual.Tag);

            //純數字功能
            number.Input(((Button)sender).Text);

            //這裡還需要處理TextBoxSubPanel!!
            TextBoxPanel.ShowText(number.ToString());
        }

        //測試用按鈕
        private void ButtonTest_Click(object sender, EventArgs e)
        { 
        }

        //add。之後要整合
        private void Button_Operate_Click(object sender, EventArgs e)
        {
            var tmpButton = (Button)sender;

            //執行運算符
            var currentAns = op.Execute(number);

            //儲存新的運算符
            dict[tmpButton.Tag][tmpButton.Text].Invoke();
            
            //計算區歸零
            number = new NumberField();

            //副Panel，等號的處理怎麼辦
            TextBoxSubPanel.ShowText(op.OperateExpression());

            //主Panel
            TextBoxPanel.ShowText(currentAns.ToString());
            
        }

        private void Button_Equal_Click(object sender, EventArgs e)
        {
            var tmpButton = (Button)sender;

            //執行運算符
            var currentAns = op.Equal(number);

            //儲存新的運算符
            dict[tmpButton.Tag][tmpButton.Text].Invoke();

            //計算區歸零
            number = new NumberField();

            //副Panel，等號的處理怎麼辦
            TextBoxSubPanel.ShowText(op.EqualExpression());

            //主Panel
            TextBoxPanel.ShowText(currentAns.ToString());
        }

        private void ButtonNegate_Click(object sender, EventArgs e)
        {
            number.Value *= -1;

            string subPanelString = (op.IsDealingEqual) ?
                $"negate({op.LastResult})"
                : (op.LastResult == null) ? 
                    "" 
                    : op.OperateExpression();

            string panelString = (op.IsDealingEqual) ?
                (number = op.LastResult).ToString()
                : number.ToString();

            //副Panel，等號的處理怎麼辦
            TextBoxSubPanel.ShowText(subPanelString);

            //主Panel
            TextBoxPanel.ShowText(panelString.ToString());
        }

        //Clear
        private void ButtonC_Click(object sender, EventArgs e)
        {
            op.Clear();
            number = new NumberField();

            //副Panel
            TextBoxSubPanel.ShowText("");

            //主Panel
            TextBoxPanel.ShowText(number.ToString());
        }

        //CE
        private void ButtonCE_Click(object sender, EventArgs e)
        {
            number.Value = 0;

            //副Panel
            TextBoxSubPanel.ShowText((op.IsDealingEqual) ? 
                "" 
                : op.OperateExpression());

            //主Panel
            TextBoxPanel.ShowText(number.ToString());
        }
    }
}
