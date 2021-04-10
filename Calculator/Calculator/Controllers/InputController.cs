using Calculator.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Calculator.Controllers
{
    public class InputController
    {
        private static InputController Instance;
        public static InputController GetInstance()
        {
            if (Instance == null)
            {
                Instance = new InputController();
                return Instance;
            }
            return Instance;
        }

        private InputController()
        {
            NumberStr = string.Empty;
            OperatorStr = string.Empty;
            IsNumeric = false;
            LeftBracket = false;
            RightBracket = false;
            UnaryList = new List<string>();
        }

        public string NumberStr {get;set;}
        
        public string OperatorStr { get; set; }

        public bool IsNumeric { get; set; }

        public bool LeftBracket { get; set; }

        public bool RightBracket { get; set; }

        public List<string> UnaryList { get; set; }

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
            decimal? number = null;
            decimal num = 0;
            if (decimal.TryParse(NumberStr, out num))
            {
                number = num;
            }


            var expression = new Expression(number: number, binaryOperatorMark: OperatorStr, leftBracket: LeftBracket, rightBracket: RightBracket, unaryList: UnaryList);
            InitThis();
            return expression;

        }

        public EqualExpression GenerateCurrentEqualExpression()
        {
            decimal.TryParse(NumberStr, out decimal number);
            var equalExpression = new EqualExpression(number: number, rightBracket: RightBracket, unaryList: UnaryList);
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
            UnaryList = new List<string>();
        }

        public void AddUnary(string unary)
        {
            UnaryList.Add(unary);
        }
    }
}
