using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonResponse
{
    public class ResponseClearErrorJson
    {
        public Updates Update;

        public class Updates
        {
            public int RemoveLength;
        }
    }
}
