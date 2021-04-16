using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class BackSpaceJson
    {
        public int RemoveLength;
        public string UpdateString;

        public BackSpaceJson(int removeLength, string updateString)
        {
            RemoveLength = removeLength;
            UpdateString = updateString;
        }
    }
}