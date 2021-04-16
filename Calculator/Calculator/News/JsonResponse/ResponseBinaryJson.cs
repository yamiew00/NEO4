using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonResponse
{
    public class ResponseBinaryJson
    {
        public Updates update;
        public class Updates
        {
            public string updateString;
            public int removeLength;
        }
    }
}
