using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class Updates
    {
        public int RemoveLength;
        public string UpdateString;

        public Updates(int removeLength, string updateString)
        {
            RemoveLength = removeLength;
            UpdateString = updateString;
        }
    }
}