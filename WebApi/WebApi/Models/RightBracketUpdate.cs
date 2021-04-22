using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Response;

namespace WebApi.Models
{
    public class RightBracketUpdate : Updates
    {
        public string NumberString;

        public RightBracketUpdate(string numberString)
        {
            NumberString = numberString;
        }

        public RightBracketUpdate(string numberString, Updates updates) : base(updates.RemoveLength, updates.UpdateString)
        {
            NumberString = numberString;
        }
    }
}