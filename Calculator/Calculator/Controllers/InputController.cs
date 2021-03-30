using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Tools;
using Calculator.CalculateObject;

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
        /// 私有建構子，外部不可建立實體
        /// </summary>
        private InputController()
        {
            CalculateMachine = CalculateMachine.GetInstance();
        }

        /// <summary>
        /// 計算機實體
        /// </summary>
        private CalculateMachine CalculateMachine;

        /// <summary>
        /// 輸入數字，同時判斷是否要更動計算器內容
        /// </summary>
        /// <param name="numberField">原有的數字</param>
        /// <param name="numberStr">要更新的新(數字)字元</param>
        public void Input(NumberField numberField, string numberStr)
        {
            int number;
            if (!int.TryParse(numberStr, out number))
            {
                return;
            }
            //判斷需不需要更動CalculateMachine
            if (CalculateMachine.Interruptible)
            {
                CalculateMachine.Interrupt();
            }
            //
            numberField.Input(number);
        }
    }
}
