using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Controllers
{
    public class OuterController
    {
        private static OuterController Instance;
        public static OuterController GetInstance()
        {
            if (Instance == null)
            {
                Instance = new OuterController();
                return Instance;
            }
            return Instance;
        }
        private OuterController()
        {

        }

        private string PanelString;

        public void Dealer(string tag, string buttonText)
        {

        }
    }
}
