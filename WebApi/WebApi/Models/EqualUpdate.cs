using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Response;

namespace WebApi.Models
{
    public class EqualUpdate : Updates
    {
        public string NumberString;

        public EqualUpdate(string numberString)
        {
            NumberString = numberString;
        }

        public EqualUpdate(string numberString, Updates updates) : base(updates.RemoveLength, updates.UpdateString)
        {
            NumberString = numberString;
        }
    }
}