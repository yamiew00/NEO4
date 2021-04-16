using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class BinaryResponse
    {
        public int RemoveLength;
        public string UpdateString;
        

        public BinaryResponse(int removeLength, string updateString)
        {
            RemoveLength = removeLength;
            UpdateString = updateString;
        }
    }
}