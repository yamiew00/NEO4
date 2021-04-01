using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Commands
    {
        public string Command;
        public Action<Form1> Action;

        public Commands(string command, Action<Form1> action)
        {
            Command = command;
            Action = action;
        }
    }
}
