using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.CalculateObject;
using Calculator.Tools.OperateObject;

namespace Calculator.Tools
{
    public class CalculateMachine
    {
        private static CalculateMachine calculateMachine;

        public static CalculateMachine GetInstance()
        {
            if (calculateMachine == null)
            {
                calculateMachine = new CalculateMachine();
                return calculateMachine;
            }
            return calculateMachine;
        }
        private CalculateMachine()
        {
        }

        public NumberField LeftNumber;
        public NumberField RightNumber;

        //四則運算
        public IOperator Operator { get; set; }

        //subPanel的顯示
        public string Expression { get; private set; }

        //panel的顯示
        public string CurrentNumber { get { return (LeftNumber == null) ? string.Empty :  LeftNumber.ToString(); } }

        //與Input交互關係，只有當RightNumber有值時才會被用到
        public bool Interruptible {
            get
            {
                return RightNumber != null;  
            }
        }

        //與Input交互關係，只有當無限大才會被用到
        public bool OccurNaN
        {
            get
            {
                return (LeftNumber == null) ? false : LeftNumber.NaN;
            }
        }

        //計算
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
        
        //四則運算
        public void Operate(NumberField newNumber, IOperator nextOperator)
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

        //等號
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
                //case +=
                if (RightNumber == null)
                {
                    RightNumber = LeftNumber;
                    Expression = LongExpOfEqual(LeftNumber, RightNumber);
                    LeftNumber = Compute(LeftNumber, RightNumber);
                }
                //case ==
                else
                {
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

        private string LongExpOfEqual(NumberField Number1, NumberField Number2)
        {
            //等號符號的處理可以更好(?
            return Number1 + " " + Operator.Mark() + " " + Number2 + " =";
        }

        private string ShortExpOfEqual(NumberField Number)
        {
            //等號符號的處理可以更好(?
            return Number + " =";
        }

        //正負號
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
            //其實還有別種case，先處理到這裡
            //目前其餘case是「非等號之後
            else
            {
                return subPanelText;
            }
        }

        //清除 C
        public void Clear()
        {
            LeftNumber = null;
            Operator = null;
            RightNumber = null;
            Expression = null;
        }

        //Clear Error : CE鍵
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

        //因為Input而改變內部數值
        public void Interrupt()
        {
            LeftNumber = null;
            Expression = string.Empty;
        }

        public void refresh()
        {
            LeftNumber = null;
            RightNumber = null;
            Operator = null;
            Expression = null;
        }
    }
}
