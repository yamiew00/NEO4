using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonResponse
{
    public class ResponseEqual
    {
        public Update Update;
        public Status Status;
        public decimal? answer;

        public ResponseEqual(Update update, decimal? answer)
        {
            Update = update;
            this.answer = answer;
        }
    }
}
