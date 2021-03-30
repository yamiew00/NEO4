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
        /// 操作中的數字
        /// </summary>
        private static NumberField CurrentNumber;

        /// <summary>
        /// 計算器實體
        /// </summary>
        private static CalculateMachine CalculateMachine = CalculateMachine.GetInstance();

        /// <summary>
        /// 輸入器實體
        /// </summary>
        private static InputController InputController = InputController.GetInstance();

        /// <summary>
        /// 主數字面板
        /// </summary>
        private static TextBox Panel;

        /// <summary>
        /// 運算式面板
        /// </summary>
        private static TextBox SubPanel;

        /// <summary>
        /// Symbol表示運算符號及等號
        /// </summary>
        private readonly object SYMBOL_TAG = new { 種類 = "Symbol" };

        /// <summary>
        /// Input表示數字、小數點及返回鍵
        /// </summary>
        private readonly object INPUT_TAG = new { 種類 = "Input" };

        /// <summary>
        /// Feature表示清除鍵及正負號
        /// </summary>
        private readonly object FEATURE_TAG = new { 種類 = "Feature" };

        /// <summary>
        /// 所有行動流程的字典。(KEY, VALUE) = (TAG, ACTION)
        /// </summary>
        private Dictionary<object, Action<object>> AllActionDic = new Dictionary<object, Action<object>>();

        /// <summary>
        /// TAG為SYMBOL_TAG時，應行動的字典。(KEY, VALUE) = (TEXT, ACTION)。其中TEXT傳入的是BUTTON擁有的TEXT
        /// </summary>
        private Dictionary<string, Action> SymbolActionDic = new Dictionary<string, Action>();

        /// <summary>
        /// TAG為INPUT_TAG時，應行動的字典。(KEY, VALUE) = (TEXT, ACTION)。其中TEXT傳入的是BUTTON擁有的TEXT
        /// </summary>
        private Dictionary<string, Action<string>> InputActionDic = new Dictionary<string, Action<string>>();

        /// <summary>
        /// TAG為FEATURE_TAG時，應行動的字典。(KEY, VALUE) = (TEXT, ACTION)。其中TEXT傳入的是BUTTON擁有的TEXT
        /// </summary>
        private Dictionary<string, Action> FeatureActionDic = new Dictionary<string, Action>();

        /// <summary>
        /// symbol用的Action
        /// </summary>
        private Action<Dictionary<string, Action>, object> SymbolAction
            = (symboldic, sender) =>
            {
                //sender是按鈕
                var button = (Button)sender;

                symboldic[button.Text].Invoke();

                //副Panel
                SubPanel.ShowText(CalculateMachine.Expression);

                //主Panel
                Panel.ShowText(CalculateMachine.CurrentNumber);
            };

        /// <summary>
        /// 要無效化的按鈕LIST
        /// </summary>
        private List<Button> DisableList;

        /// <summary>
        /// 建構子
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            
            //預設數字視為使用者輸入
            CurrentNumber = new NumberField(0);    

            //靜態面板初始化
            Panel = TextBoxPanel;
            SubPanel = TextBoxSubPanel;

            //所有Tag初始化
            InitTag();

            //所有字典初始化
            InitActionDic();

            //DisableList初始化
            DisableListInit();
        }

        /// <summary>
        /// TAG的初始化
        /// </summary>
        private void InitTag()
        {
            //有SYMBOL_Tag的包含四則運算、等號
            ButtonAdd.Tag = SYMBOL_TAG;
            ButtonMinus.Tag = SYMBOL_TAG;
            ButtonMultiply.Tag = SYMBOL_TAG;
            ButtonDivide.Tag = SYMBOL_TAG;
            ButtonEqual.Tag = SYMBOL_TAG;

            //有INPUT_TAG的包含數字、小數點及返回鍵
            Button0.Tag = INPUT_TAG;
            Button1.Tag = INPUT_TAG;
            Button2.Tag = INPUT_TAG;
            Button3.Tag = INPUT_TAG;
            Button4.Tag = INPUT_TAG;
            Button5.Tag = INPUT_TAG;
            Button6.Tag = INPUT_TAG;
            Button7.Tag = INPUT_TAG;
            Button8.Tag = INPUT_TAG;
            Button9.Tag = INPUT_TAG;
            ButtonDecimalPoint.Tag = INPUT_TAG;
            ButtonBack.Tag = INPUT_TAG;

            //有FEATURE_TAG的包含正負號、及清除鍵
            ButtonNegate.Tag = FEATURE_TAG;
            ButtonC.Tag = FEATURE_TAG;
            ButtonCE.Tag = FEATURE_TAG;
        }

        /// <summary>
        /// 主流程字典初始化
        /// </summary>
        private void InitActionDic()
        {
            //AllActionDic字典初始化
            InitAllActionDic();

            //Symbol字典初始化 
            InitSymbolActionDic();

            //Input字典初始化
            InitInputActionDic();

            //Feature字典初始化
            InitFeatureActionDic();
        }

        /// <summary>
        /// AllActionDic的初始化
        /// </summary>
        private void InitAllActionDic()
        {
            AllActionDic.Add(SYMBOL_TAG, (sender) =>
            {
                SymbolAction(SymbolActionDic, sender);
                CurrentNumber = new NumberField();
                if (CalculateMachine.OccurNaN)
                {
                    DisableButtons();
                }
            });
            AllActionDic.Add(INPUT_TAG, (sender) =>
            {
                var button = (Button)sender;
                InputActionDic[button.Text].Invoke(button.Text);
                Panel.ShowText(CurrentNumber.ToString());
                SubPanel.ShowText(CalculateMachine.Expression);
            });
            AllActionDic.Add(FEATURE_TAG, (sender) =>
            {
                var button = (Button)sender;
                FeatureActionDic[button.Text].Invoke();
            });
        }

        /// <summary>
        /// SymbolActionDic的初始化
        /// </summary>
        private void InitSymbolActionDic()
        {
            SymbolActionDic.Add(ButtonAdd.Text, () => { CalculateMachine.OperateEvent(CurrentNumber, new Add()); });
            SymbolActionDic.Add(ButtonMinus.Text, () => { CalculateMachine.OperateEvent(CurrentNumber, new Minus()); });
            SymbolActionDic.Add(ButtonMultiply.Text, () => { CalculateMachine.OperateEvent(CurrentNumber, new Multiply()); });
            SymbolActionDic.Add(ButtonDivide.Text, () => { CalculateMachine.OperateEvent(CurrentNumber, new Divide()); });
            SymbolActionDic.Add(ButtonEqual.Text, () =>
            {
                EnableButtons();
                CalculateMachine.EqualEvent(CurrentNumber);
            });
        }

        /// <summary>
        /// InputActionDic的初始化 
        /// </summary>
        private void InitInputActionDic()
        {
            Action<string> inputNumber = ((text) =>
            {
                EnableButtons();
                InputController.Input(CurrentNumber, text);
            });
            InputActionDic.Add(".", (text) => { CurrentNumber.AddPoint(); });
            InputActionDic.Add("⌫", (text) =>
            {
                EnableButtons();
                CurrentNumber.BackSpace();
            });
            InputActionDic.Add("0", inputNumber);
            InputActionDic.Add("1", inputNumber);
            InputActionDic.Add("2", inputNumber);
            InputActionDic.Add("3", inputNumber);
            InputActionDic.Add("4", inputNumber);
            InputActionDic.Add("5", inputNumber);
            InputActionDic.Add("6", inputNumber);
            InputActionDic.Add("7", inputNumber);
            InputActionDic.Add("8", inputNumber);
            InputActionDic.Add("9", inputNumber);
        }

        /// <summary>
        /// FeatureActionDic的初始化 
        /// </summary>
        private void InitFeatureActionDic()
        {
            FeatureActionDic.Add("±", () =>
            {
                //調整數值
                CurrentNumber = (CurrentNumber.isInput) ?
                    new NumberField(-1 * CurrentNumber.Value)
                    : new NumberField(-1 * CalculateMachine.LeftNumber.Value);

                SubPanel.ShowText(CalculateMachine.NegateExpression(SubPanel.Text));

                Panel.ShowText(CurrentNumber.ToString());
            });
            FeatureActionDic.Add("C", () =>
            {
                EnableButtons();
                CalculateMachine.Clear();
                CurrentNumber = new NumberField(0);

                //副Panel
                SubPanel.ShowText("");

                //主Panel
                Panel.ShowText(CurrentNumber.ToString());
            });
            FeatureActionDic.Add("CE", () =>
            {
                EnableButtons();
                CurrentNumber = new NumberField(0);

                //副Panel
                CalculateMachine.ClearError();
                SubPanel.ShowText(CalculateMachine.Expression);

                //主Panel
                TextBoxPanel.ShowText(CurrentNumber.ToString());
            });
        }

        /// <summary>
        /// 指定可能要無效化的按鈕
        /// </summary>
        private void DisableListInit()
        {
            DisableList = new List<Button>();
            DisableList.Add(ButtonAdd);
            DisableList.Add(ButtonMinus);
            DisableList.Add(ButtonMultiply);
            DisableList.Add(ButtonDivide);
            DisableList.Add(ButtonNegate);
            DisableList.Add(ButtonDecimalPoint);
        }

        /// <summary>
        /// 全按鈕點擊事件
        /// </summary>
        /// <param name="sender">按鈕</param>
        /// <param name="e">點擊事件</param>
        private void All_Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            AllActionDic[button.Tag].Invoke(sender);
        }

        /// <summary>
        /// 指定按鈕無效化(發生無限大的時候)
        /// </summary>
        private void DisableButtons()
        {
            foreach (var Button in DisableList)
            {
                Button.Enabled = false;
            }
        }

        /// <summary>
        /// 復原無效化的按鈕
        /// </summary>
        private void EnableButtons()
        {
            if (CalculateMachine.OccurNaN)
            {
                CalculateMachine.Refresh();
                foreach (var Button in DisableList)
                {
                    Button.Enabled = true;
                }
                //視為使用者輸入0
                CurrentNumber = new NumberField(0);
            }
        }
    }
}
