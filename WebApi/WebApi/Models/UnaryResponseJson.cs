using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class UnaryResponseJson
    {
        public int RemoveLength;
        public string UpdateString;

        public UnaryResponseJson(int removeLength, string updateString)
        {
            RemoveLength = removeLength;
            UpdateString = updateString;
        }
    }
}