using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonObject
{
    public class ResponseNumberJson
    {
        public Updates Update;
        public class Updates
        {
            public string updateString;
        }
    }
}
