using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Response;

namespace WebApi.Models
{
    public class NumberUpdate : Updates
    {
        public string NumberString;

        public NumberUpdate(string numberString)
        {
            NumberString = numberString;
        }

        public NumberUpdate(string numberString, Updates updates) : base(updates.RemoveLength, updates.UpdateString)
        {
            NumberString = numberString;
        }
    }
}