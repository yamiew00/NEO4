using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Calculator.NewThings
{
    public class NewInputController
    {
        private static NewInputController Instance;
        public static NewInputController GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NewInputController();
                return Instance;
            }
            return Instance;
        }

        private NewInputController()
        {
            NumberStr = string.Empty;
            OperatorStr = string.Empty;
            IsNumeric = false;
            LeftBracket = false;
            RightBracket = false;
        }

        public string NumberStr {get;set;}
        
        public string OperatorStr { get; set; }

        public bool IsNumeric { get; set; }

        public bool LeftBracket { get; set; }

        public bool RightBracket { get; set; }

        public bool AddNumber(string numberStr)
        {
            if (numberStr.Equals("."))
            {
                if (!IsNumeric)
                {
                    NumberStr += numberStr;
                    IsNumeric = true;
                    return true;
                }
                return false;
            }
            NumberStr += numberStr;
            return true;
        }

        public void SetOperator(string operatorStr)
        {
            OperatorStr = operatorStr;
        }

        public  void SetLeftBracket()
        {
            this.LeftBracket = true;
        }

        public void SetRightBracket()
        {
            this.RightBracket = true;
        }

        public void Clear()
        {
            InitThis();
        }

        public void ClearError()
        {
            NumberStr = string.Empty;
        }

        public void BackSpace()
        {
            if (NumberStr.Length == 0)
            {
                return;
            }
            NumberStr = NumberStr.Substring(0, NumberStr.Length - 1);
        }

        public Expression GenerateCurrentExpression()
        {
            decimal.TryParse(NumberStr, out decimal number);
            var expression = new Expression(number: number, binaryOperatorMark: OperatorStr, leftBracket: LeftBracket, rightBracket: RightBracket);
            InitThis();
            return expression;

        }

        public EqualExpression GenerateCurrentEqualExpression()
        {
            decimal.TryParse(NumberStr, out decimal number);
            var equalExpression = new EqualExpression(number: number, rightBracket: RightBracket);
            InitThis();
            return equalExpression;
        }

        private void InitThis()
        {
            NumberStr = string.Empty;
            OperatorStr = string.Empty;
            IsNumeric = false;
            LeftBracket = false;
            RightBracket = false;
        }
    }
}
