using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonResponse
{
    public class Update
    {
        public int RemoveLength;
        public string UpdateString;

        public Update()
        {
        }

        public Update(int removeLength, string updateString)
        {
            RemoveLength = removeLength;
            UpdateString = updateString;
        }
    }
}
