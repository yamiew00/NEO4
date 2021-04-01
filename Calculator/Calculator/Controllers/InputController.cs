using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Tools;


namespace Calculator.Controllers
{
    /// <summary>
    /// 輸入器實體
    /// </summary>
    public class InputController
    {
        /// <summary>
        /// 靜態唯一實體
        /// </summary>
        private static InputController Instance;
        
        /// <summary>
        /// 取得唯一實體
        /// </summary>
        /// <returns>實體</returns>
        public static InputController GetInstance()
        {
            if (Instance == null)
            {
                Instance = new InputController();
                return Instance;
            }
            return Instance;
        }

        private ExpressionTree ETree;

        /// <summary>
        /// 私有建構子，外部不可建立實體
        /// </summary>
        private InputController()
        {
            ETree = new ExpressionTree();
        }


        
        //以下新的
        //分成數字和等號處理
        public void Input(string controlText)
        {
            //暫時先寫死等號
            if (!controlText.Equals("="))
            {
                ETree.Input(controlText);
            }
        }

        public void Clear()
        {
            ETree.Clear();
        }

        public void ClearError()
        {
            ETree.ClearError();
        }

        public string GetExpression()
        {
            return ETree.Expression;
        }

        public decimal GetResult()
        {
            return ETree.Result();
        }

        public void BackSpace()
        {
            ETree.BackSpace();
        }

    }
}
