using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.CalculateObjects
{
    /// <summary>
    /// 特殊符號
    /// </summary>
    public class WildSymbol
    {
        public int Priority;

        public WildSymbol(int priority)
        {
            Priority = priority;
        }
    }
}
