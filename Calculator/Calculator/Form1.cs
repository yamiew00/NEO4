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

        //弄成靜態物件
        private static CalculateMachine calculateMachine = CalculateMachine.getInstance();
        private  CalculateMachine op = new CalculateMachine();

        //行為字典
        Dictionary<object, Dictionary<string, Action>> dict;

        //運算符字典
        Dictionary<string, IOperator> OperatorDic = new Dictionary<string, IOperator>();


        //Symbol字典
        Dictionary<string, Action<NumberField>> SymbolDic = new Dictionary<string, Action<NumberField>>();

        public Form1()
        {
            InitializeComponent();
            number = new NumberField(0);    //第一個數字CanOperate

            //op初始化


            //(字典)初始化
            Init();

            //新做法
            OperatorDic.Add("+", new Add());
            OperatorDic.Add("-", new Minus());
            OperatorDic.Add("x", new Multiply());
            OperatorDic.Add("÷", new Divide());

            //symbol初始化
            //calculateMachine.Operate(number, operatorDic[button.Text]);
            SymbolDic.Add("+", (number) => {calculateMachine.Operate(number, new Add());} );
            SymbolDic.Add("-", (number) => { calculateMachine.Operate(number, new Minus()); });
            SymbolDic.Add("x", (number) => { calculateMachine.Operate(number, new Multiply()); });
            SymbolDic.Add("÷", (number) => { calculateMachine.Operate(number, new Divide()); });
            SymbolDic.Add("=", (number) => { calculateMachine.EqualEvent(number); });

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
            var operatorDic = new Dictionary<string, Action>();
            operatorDic.Add("+", () => calculateMachine.OperateSelector = new Add());
            operatorDic.Add("-", () => calculateMachine.OperateSelector = new Minus());
            operatorDic.Add("x", () => calculateMachine.OperateSelector = new Multiply());
            operatorDic.Add("=", () => calculateMachine.IsDealingEqual = true);

            dict.Add(operatorType, operatorDic);
        }

        //數字相關按鈕
        private void Button_Click(object sender, EventArgs e)
        {

            //執行期
            
            //純數字功能
            number.Input(((Button)sender).Text);

            //這裡還需要處理TextBoxSubPanel!!
            TextBoxPanel.ShowText(number.ToString());
        }

        //測試用按鈕
        private void ButtonTest_Click(object sender, EventArgs e)
        {
        }

        //四則運算按鈕。之後要整合
        private void Button_Operate_Click(object sender, EventArgs e)
        {
            var tmpButton = (Button)sender;

            //執行運算符
            var currentAns = calculateMachine.Execute(number);

            //儲存新的運算符
            dict[tmpButton.Tag][tmpButton.Text].Invoke();

            //計算區歸零
            number = new NumberField();

            //副Panel，等號的處理怎麼辦
            TextBoxSubPanel.ShowText(calculateMachine.OperateExpression());

            //主Panel
            TextBoxPanel.ShowText(currentAns.ToString());

            //新做法
            //var tmpButton = (Button)sender;

            //op.Operate(number, );
        }


        //暫時用的四則運算按鈕
        private void Operators_Button_Click(object sender, EventArgs e)
        {
            OperateAction(TextBoxPanel, TextBoxSubPanel, number, OperatorDic, sender);
            number = new NumberField();
        }
        

        //四則運算Action
        private Action<TextBox, TextBox, NumberField, Dictionary<string, IOperator>, object> OperateAction 
            = (panel, subpanel, number, operatorDic, sender) =>
        {
            //sender是按鈕
            var button = (Button)sender;

            Console.WriteLine($"number.IsInput = {number.isInput}");

            //做四則運算
            calculateMachine.Operate(number, operatorDic[button.Text]);
            
            //副Panel，等號的處理怎麼辦
            subpanel.ShowText(calculateMachine.Expression); //op可弄成靜態?
            
            //主Panel
            panel.ShowText(calculateMachine.currentNumber);
        };


        //symbol整合用
        private Action<TextBox, TextBox, NumberField, Dictionary<string, Action<NumberField>>, object> symbolAction
            = (panel, subpanel, number, symboldic, sender) =>
            {
                //sender是按鈕
                var button = (Button)sender;

                symboldic[button.Text].Invoke(number);

                //副Panel，等號的處理怎麼辦
                subpanel.ShowText(calculateMachine.Expression); //op可弄成靜態?

                //主Panel
                panel.ShowText(calculateMachine.currentNumber);
            };

        private void Symbol_Button_Click(object sender, EventArgs e)
        {
            symbolAction(TextBoxPanel, TextBoxSubPanel, number, SymbolDic, sender);
            number = new NumberField();
        }


        //暫時用的等號按鈕
        private void Tmp_Equal_Button_Click(object sender, EventArgs e)
        {
            EqualAction(TextBoxPanel, TextBoxSubPanel, number, OperatorDic, sender);
            number = new NumberField();
        }

        //等號Action
        private Action<TextBox, TextBox, NumberField, Dictionary<string, IOperator>, object> EqualAction
            = (panel, subpanel, number, operatorDic, sender) =>
        {
            //sender是按鈕
            var button = (Button)sender;

            calculateMachine.EqualEvent(number);

            //副Panel，等號的處理怎麼辦
            subpanel.ShowText(calculateMachine.Expression); //op可弄成靜態?

            //主Panel
            panel.ShowText(calculateMachine.currentNumber);
        };



        
        //等號按鈕
        private void Button_Equal_Click(object sender, EventArgs e)
        {
            var tmpButton = (Button)sender;

            //執行運算符
            var currentAns = calculateMachine.Equal(number);

            //儲存新的運算符
            dict[tmpButton.Tag][tmpButton.Text].Invoke();

            //計算區歸零
            number = new NumberField();

            //副Panel，等號的處理怎麼辦
            TextBoxSubPanel.ShowText(calculateMachine.EqualExpression());

            //主Panel
            TextBoxPanel.ShowText(currentAns.ToString());
        }

        //正負號按鈕
        private void ButtonNegate_Click(object sender, EventArgs e)
        {
            number.Value *= -1;

            string subPanelString = (calculateMachine.IsDealingEqual) ?
                $"negate({calculateMachine.LastResult})"
                : (calculateMachine.LastResult == null) ? 
                    "" 
                    : calculateMachine.OperateExpression();

            string panelString = (calculateMachine.IsDealingEqual) ?
                (number = calculateMachine.LastResult).ToString()
                : number.ToString();

            //副Panel，等號的處理怎麼辦
            TextBoxSubPanel.ShowText(subPanelString);

            //主Panel
            TextBoxPanel.ShowText(panelString.ToString());
        }

        //Clear按鈕
        private void ButtonC_Click(object sender, EventArgs e)
        {
            calculateMachine.Clear();
            number = new NumberField();

            //副Panel
            TextBoxSubPanel.ShowText("");

            //主Panel
            TextBoxPanel.ShowText(number.ToString());
        }

        //CE按鈕
        private void ButtonCE_Click(object sender, EventArgs e)
        {
            number.Value = 0;

            //副Panel
            TextBoxSubPanel.ShowText((calculateMachine.IsDealingEqual) ? 
                "" 
                : calculateMachine.OperateExpression());

            //主Panel
            TextBoxPanel.ShowText(number.ToString());
        }
    }
}
