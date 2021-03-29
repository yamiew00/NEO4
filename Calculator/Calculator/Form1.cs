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
        private static NumberField number;

        //靜態物件
        private static CalculateMachine calculateMachine = CalculateMachine.getInstance();

        //靜態Control
        private static TextBox Panel;
        private static TextBox SubPanel;

        //Tags
        object symbolTag = new { 種類 = "Symbol" };
        object inputTag = new { 種類 = "Input" };
        object featureTag = new { 種類 = "Feature" };


        //All字典
        Dictionary<object, Action<object>> AllDic = new Dictionary<object, Action<object>>();

        //Symbol字典
        Dictionary<string, Action> SymbolDic = new Dictionary<string, Action>();

        //Input字典
        Dictionary<string, Action<string>> InputDic = new Dictionary<string, Action<string>>();

        //Feature字典
        Dictionary<string, Action> FeatureDic = new Dictionary<string, Action>();


        //建構子，包含字典初始化
        public Form1()
        {
            InitializeComponent();

            //第一個數字視為使用者已輸入
            number = new NumberField(0);    

            //靜態物件初始化
            Panel = TextBoxPanel;
            SubPanel = TextBoxSubPanel;

            //Tag初始化
            TagInit();

            //字典初始化
            DictInit();
        }

        //初始化Tag
        private void TagInit()
        {
            //定義Tag→加法、減法、乘法
            ButtonAdd.Tag = symbolTag;
            ButtonMinus.Tag = symbolTag;
            ButtonMultiply.Tag = symbolTag;
            ButtonDivide.Tag = symbolTag;
            ButtonEqual.Tag = symbolTag;


            //Input的Tag→
            Button0.Tag = inputTag;
            Button1.Tag = inputTag;
            Button2.Tag = inputTag;
            Button3.Tag = inputTag;
            Button4.Tag = inputTag;
            Button5.Tag = inputTag;
            Button6.Tag = inputTag;
            Button7.Tag = inputTag;
            Button8.Tag = inputTag;
            Button9.Tag = inputTag;
            ButtonDecimalPoint.Tag = inputTag;
            ButtonBack.Tag = inputTag;
            
            //Feature的Tag
            ButtonNegate.Tag = featureTag;
            ButtonC.Tag = featureTag;
            ButtonCE.Tag = featureTag;
        }

        private void DictInit()
        {
            //symbol字典初始化 整合五個符號
            SymbolDic.Add("+", () => { calculateMachine.Operate(number, new Add()); });
            SymbolDic.Add("-", () => { calculateMachine.Operate(number, new Minus()); });
            SymbolDic.Add("x", () => { calculateMachine.Operate(number, new Multiply()); });
            SymbolDic.Add("÷", () => { calculateMachine.Operate(number, new Divide()); });
            SymbolDic.Add("=", () => { calculateMachine.EqualEvent(number); });

            //Input字典初始化 back和小數點
            InputDic.Add(".", (text) => { number.AddPoint(); });
            InputDic.Add("⌫", (text) => { number.BackSpace(); });
            InputDic.Add("0", (text) => { number.Input(text); });
            InputDic.Add("1", (text) => { number.Input(text); });
            InputDic.Add("2", (text) => { number.Input(text); });
            InputDic.Add("3", (text) => { number.Input(text); });
            InputDic.Add("4", (text) => { number.Input(text); });
            InputDic.Add("5", (text) => { number.Input(text); });
            InputDic.Add("6", (text) => { number.Input(text); });
            InputDic.Add("7", (text) => { number.Input(text); });
            InputDic.Add("8", (text) => { number.Input(text); });
            InputDic.Add("9", (text) => { number.Input(text); });

            //Feature字典初始化
            FeatureDic.Add("±", () =>
            {
                //調整數值
                number = (number.isInput) ?
                    new NumberField(-1 * number.Value)
                    : new NumberField(-1 * calculateMachine.LeftNumber.Value);


                //number.Value = (number.isInput) ? -1 * number.Value : number.Value;

                SubPanel.ShowText(calculateMachine.NegateExpression(SubPanel.Text));

                //Panel.ShowText((number.isInput) ? number.ToString() : calculateMachine.LeftNumber.ToString());
                Panel.ShowText(number.ToString());
            });
            FeatureDic.Add("C", () =>
            {
                calculateMachine.Clear();
                number = new NumberField(0);

                //副Panel
                SubPanel.ShowText("");

                //主Panel
                Panel.ShowText(number.ToString());
            });
            FeatureDic.Add("CE", () =>
            {
                number = new NumberField(0);

                //副Panel
                calculateMachine.ClearError();
                SubPanel.ShowText(calculateMachine.Expression);

                //主Panel
                TextBoxPanel.ShowText(number.ToString());
            });

            //All字典初始化
            AllDic.Add(symbolTag, (sender) =>
            {
                symbolAction(SymbolDic, sender);
                number = new NumberField();
            });
            AllDic.Add(inputTag, (sender) =>
            {
                var button = (Button)sender;
                InputDic[button.Text].Invoke(button.Text);
                Panel.ShowText(number.ToString());
            });
            AllDic.Add(featureTag, (sender) => {
                var button = (Button)sender;
                FeatureDic[button.Text].Invoke();
            });
        }



        //測試整合
        private void All_Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            AllDic[button.Tag].Invoke(sender);
        }

        //symbol用的Action
        private Action<Dictionary<string, Action>, object> symbolAction
            = (symboldic, sender) =>
            {
                //sender是按鈕
                var button = (Button)sender;

                symboldic[button.Text].Invoke();

                //副Panel
                SubPanel.ShowText(calculateMachine.Expression);

                //主Panel
                Panel.ShowText(calculateMachine.currentNumber);
            };
    }
}
