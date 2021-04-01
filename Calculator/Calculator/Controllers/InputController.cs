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

        /// <summary>
        /// 運算樹
        /// </summary>
        private ExpressionTree ETree;

        /// <summary>
        /// 私有建構子，外部不可建立實體
        /// </summary>
        private InputController()
        {
            ETree = new ExpressionTree();
        }
        
        /// <summary>
        /// 承接畫面端送過來的文字(指令)
        /// </summary>
        /// <param name="controlText">文字</param>
        public void Input(string controlText)
        {
            if (!controlText.Equals("="))
            {
                ETree.Input(controlText);
            }
        }

        /// <summary>
        /// Clear鍵事件
        /// </summary>
        public void Clear()
        {
            ETree.Clear();
        }

        /// <summary>
        /// ClearError事件
        /// </summary>
        public void ClearError()
        {
            ETree.ClearError();
        }

        /// <summary>
        /// 獲取當下的運算式
        /// </summary>
        /// <returns>運算式</returns>
        public string GetExpression()
        {
            return ETree.Expression;
        }

        /// <summary>
        /// 獲取運算結果
        /// </summary>
        /// <returns>運算結果</returns>
        public decimal GetResult()
        {
            return ETree.Result();
        }

        /// <summary>
        /// BackSpace事件
        /// </summary>
        public void BackSpace()
        {
            ETree.BackSpace();
        }
    }
}
