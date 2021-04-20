using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Networks.Request
{
    public class Bond
    {
        public string Feature;

        public char Content;

        public Bond(string feature, char content)
        {
            Feature = feature;
            Content = content;
        }
    }
}
