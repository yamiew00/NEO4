using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.CalculateObject;
using Calculator.Tools.OperateObject;

namespace Calculator.Tools
{
    /// <summary>
    /// 計算器，是唯一實體
    /// </summary>
    public class CalculateMachine
    {
        /// <summary>
        /// 靜態唯一實體
        /// </summary>
        private static CalculateMachine Instance;

        /// <summary>
        /// 取得唯一實體的方法
        /// </summary>
        /// <returns>回傳實體</returns>
        public static CalculateMachine GetInstance()
        {
            if (Instance == null)
            {
                Instance = new CalculateMachine();
                return Instance;
            }
            return Instance;
        }

        /// <summary>
        /// private建構子，外部不可建構Instance
        /// </summary>
        private CalculateMachine()
        {
        }

        /// <summary>
        /// 左數：運算子左邊的數
        /// </summary>
        public NumberField LeftNumber { get; set; }

        /// <summary>
        /// 右數：運算子右邊的數
        /// </summary>
        public NumberField RightNumber { get; set; }

        /// <summary>
        /// 運算符號
        /// </summary>
        public IOperator Operator { get; set; }

        /// <summary>
        /// 運算式。是SubPanel的顯示
        /// </summary>
        public string Expression { get; private set; }

        /// <summary>
        /// 當下的計算結果。是Panel的顯示
        /// </summary>
        public string CurrentNumber
        {
            get
            {
                return (LeftNumber == null) ? string.Empty : LeftNumber.ToString();
            }
        }

        /// <summary>
        /// 是否與Input有交互關係。只有當RightNumber有值時才會被用到
        /// </summary>
        public bool Interruptible
        {
            get
            {
                return RightNumber != null;  
            }
        }

        /// <summary>
        /// 是否與Input有交互關係。只有當無限大才會被用到
        /// </summary>
        public bool OccurNaN
        {
            get
            {
                return (LeftNumber == null) ? false : LeftNumber.NaN;
            }
        }

        /// <summary>
        /// 兩數計算，同時完成運算式
        /// </summary>
        /// <param name="number1">運算子左邊的數</param>
        /// <param name="number2">運算子右邊的數</param>
        /// <returns>計算結果</returns>
        private NumberField Compute(NumberField number1, NumberField number2)
        {
            //無限大在這裡處理好嗎
            var value = Operator.Compute(number1.Value, number2.Value);
            if (value == null)
            {
                Expression = $"{number1.ToString()} ÷ ";
            }
            return new NumberField(value);
        }

        /// <summary>
        /// 四則運算事件
        /// </summary>
        /// <param name="newNumber">待處理數字</param>
        /// <param name="nextOperator">下次要執行的運算子</param>
        public void OperateEvent(NumberField newNumber, IOperator nextOperator)
        {
            //RightNumber一定會變null
            //無限大時，Expression的處理被Compute負責了(?)
            if (newNumber.isInput == false)
            {
                //如果等號後input, LeftNumber不變，並將RightNumber清空
                RightNumber = null;
                Operator = nextOperator;
            }
            else if (LeftNumber == null && Operator != null && RightNumber != null)
            {
                //等號後Input + Interrupt的狀況
                LeftNumber = newNumber;
                Operator = nextOperator;
                RightNumber = null;
            }
            else if (LeftNumber == null && Operator == null && RightNumber == null)
            {
                //第一次輸進數字，不做運算 
                LeftNumber = newNumber;
                Operator = nextOperator;
            }
            else if (LeftNumber != null && Operator != null && RightNumber == null)
            {
                //左數 四則運算 右數
                LeftNumber = Compute(LeftNumber, newNumber);
                Operator = nextOperator;
            }
            else if (LeftNumber != null && Operator != null && RightNumber != null)
            {
                //negate case
                LeftNumber = newNumber;
                Operator = nextOperator;
                RightNumber = null;
            }

            //這個階段若LeftNumber.NaN 為true ，代表除到0了
            if (!LeftNumber.NaN)
            {
                Expression = $"{LeftNumber.ToString()} {nextOperator.Mark()}";
            }
            
            //無限大處理，後續還未做
            if (LeftNumber.NaN)
            {
                Expression += $" 0 {Operator.Mark()}";
            }
        }

        /// <summary>
        /// 等號事件
        /// </summary>
        /// <param name="newNumber">待處理數字</param>
        public void EqualEvent(NumberField newNumber)
        {
            //無限大時，Expression的處理被Compute負責了
            if (Operator == null)
            {
                //未運算，等號連點。
                LeftNumber = (newNumber.isInput == true) ? newNumber : LeftNumber;
                Expression = ShortExpOfEqual(LeftNumber);
            }
            else if (newNumber.isInput == false)
            {
                //有運算，運算符連做
                if (RightNumber == null)
                {
                    //case +=
                    RightNumber = LeftNumber;
                    Expression = LongExpOfEqual(LeftNumber, RightNumber);
                    LeftNumber = Compute(LeftNumber, RightNumber);
                }
                else
                {
                    //case ==
                    Expression = LongExpOfEqual(LeftNumber, RightNumber);
                    LeftNumber = Compute(LeftNumber, RightNumber);
                }
            }
            else if ( LeftNumber == null && Operator != null && RightNumber != null)
            {
                //等號後Input + Interrupt的狀況
                Expression = LongExpOfEqual(newNumber, RightNumber);
                LeftNumber = Compute(newNumber, RightNumber);         
            }
            else if (LeftNumber != null && Operator != null && RightNumber == null)
            {
                //兩數運算
                Expression = LongExpOfEqual(LeftNumber, newNumber);
                LeftNumber = Compute(LeftNumber, newNumber);         
                RightNumber = newNumber;
            }
            else if (LeftNumber != null && Operator != null && RightNumber != null)
            {
                //等號連按，相當於重複計算LeftNumber與RightNumber，按當前的Operator做
                Expression = LongExpOfEqual(LeftNumber, RightNumber);
                LeftNumber = Compute(LeftNumber, RightNumber); 
            }
        }

        /// <summary>
        /// 等號事件的長表示式。「a + b = 」
        /// </summary>
        /// <param name="Number1">運算子左邊的數</param>
        /// <param name="Number2">運算子右邊、等號左邊的數</param>
        /// <returns>表示式</returns>
        private string LongExpOfEqual(NumberField Number1, NumberField Number2)
        {
            //等號符號的處理可以更好(?
            return Number1 + " " + Operator.Mark() + " " + Number2 + " =";
        }

        /// <summary>
        /// 等號事件的短表示式。「a = 」
        /// </summary>
        /// <param name="Number">等號左邊的數</param>
        /// <returns>表示式</returns>
        private string ShortExpOfEqual(NumberField Number)
        {
            //等號符號的處理可以更好(?
            return Number + " =";
        }

        /// <summary>
        /// 回傳正負號事件後的表示式
        /// </summary>
        /// <param name="subPanelText">原本SubPanel上的文字</param>
        /// <returns>新表示式</returns>
        public string NegateExpression(string subPanelText)
        {
            if (subPanelText == null)
            {
                return "";
            }
            else if (subPanelText.Contains("="))
            {
                return $"negate({LeftNumber})";
            }
            else if (subPanelText.Contains("negate"))
            {
                return $"negate({subPanelText})";
            }
            else
            {
                //「非等號之後」
                return subPanelText;
            }
        }

        /// <summary>
        /// 清除鍵「C」的事件
        /// </summary>
        public void Clear()
        {
            LeftNumber = null;
            Operator = null;
            RightNumber = null;
            Expression = null;
        }

        /// <summary>
        /// 清除鍵「CE」(Clear Error)的事件
        /// </summary>
        public void ClearError()
        {
            //等號後就clear，四則運算後則不變
            if (RightNumber != null)
            {
                Clear();
                Expression = string.Empty;
            }
            else
            {
                if (LeftNumber != null && Operator == null)
                {
                    LeftNumber = new NumberField(0);
                }
            }
        }

        /// <summary>
        /// 與Input的交互關係，被Input改變值的事件
        /// </summary>
        public void Interrupt()
        {
            LeftNumber = null;
            Expression = string.Empty;
        }

        /// <summary>
        /// 重置計算器內所有內容
        /// </summary>
        public void Refresh()
        {
            LeftNumber = null;
            RightNumber = null;
            Operator = null;
            Expression = null;
        }
    }
}
