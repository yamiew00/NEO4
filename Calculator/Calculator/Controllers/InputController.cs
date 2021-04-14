using Calculator.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Controllers
{
    /// <summary>
    /// 輸入控制
    /// </summary>
    public class InputController
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static InputController _instance = new InputController();

        /// <summary>
        /// 取得實體方法
        /// </summary>
        /// <returns></returns>
        public static InputController Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 建構子
        /// </summary>
        private InputController()
        {
            NumberStr = string.Empty;
            OperatorStr = string.Empty;
            IsNumeric = false;
            LeftBracket = false;
            RightBracket = false;
            UnaryList = new List<string>();
        }

        /// <summary>
        /// 數字(字串)
        /// </summary>
        public string NumberStr {get; set; }
        
        /// <summary>
        /// 運算符(字串)
        /// </summary>
        public string OperatorStr { get; set; }

        /// <summary>
        /// 是否為小數的布林
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// 帶有左括號
        /// </summary>
        public bool LeftBracket { get; set; }

        /// <summary>
        /// 帶有右括號
        /// </summary>
        public bool RightBracket { get; set; }

        /// <summary>
        /// 單元運算列
        /// </summary>
        public List<string> UnaryList { get; set; }

        /// <summary>
        /// 新增數字
        /// </summary>
        /// <param name="numberStr">數字字串</param>
        /// <returns>新增成功</returns>
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

        /// <summary>
        /// 設定運算符
        /// </summary>
        /// <param name="operatorStr">運算符</param>
        public void SetOperator(string operatorStr)
        {
            OperatorStr = operatorStr;
        }

        /// <summary>
        /// 設定左括號
        /// </summary>
        public void SetLeftBracket()
        {
            this.LeftBracket = true;
        }

        /// <summary>
        /// 設定右括號
        /// </summary>
        public void SetRightBracket()
        {
            this.RightBracket = true;
        }

        /// <summary>
        /// Clear事件
        /// </summary>
        public void Clear()
        {
            InitThis();
        }

        /// <summary>
        /// ClearError事件
        /// </summary>
        public void ClearError()
        {
            NumberStr = string.Empty;
        }

        /// <summary>
        /// 返回鍵
        /// </summary>
        public void BackSpace()
        {
            if (NumberStr.Length == 0)
            {
                return;
            }
            NumberStr = NumberStr.Substring(0, NumberStr.Length - 1);
        }

        /// <summary>
        /// 新增單元運算
        /// </summary>
        /// <param name="unary">單元運算子</param>
        public void AddUnary(string unary)
        {
            UnaryList.Add(unary);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitThis()
        {
            NumberStr = string.Empty;
            OperatorStr = string.Empty;
            IsNumeric = false;
            LeftBracket = false;
            RightBracket = false;
            UnaryList = new List<string>();
        }

        /// <summary>
        /// 產生OperatorExpression
        /// </summary>
        /// <returns>OperatorExpression</returns>
        public OperatorExpression CreateOperatorExpression()
        {
            decimal.TryParse(NumberStr, out decimal number);

            var expression = new OperatorExpression(number: number, binaryOperatorMark: OperatorStr, leftBracket: LeftBracket, rightBracket: RightBracket, unaryList: UnaryList);
            InitThis();
            return expression;
        }

        /// <summary>
        /// 產生EqualExpression
        /// </summary>
        /// <returns>EqualExpression</returns>
        public EqualExpression CreateEqualExpression()
        {
            decimal.TryParse(NumberStr, out decimal number);
            var equalExpression = new EqualExpression(number: number, leftBracket: LeftBracket , rightBracket: RightBracket, unaryList: UnaryList);
            InitThis();
            return equalExpression;
        }
    }
}
