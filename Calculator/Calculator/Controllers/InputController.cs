using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Tools;
using Calculator.CalculateObject;

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
            CalculateMachine = CalculateMachine.GetInstance();
        }

        private CalculateMachine CalculateMachine;

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
