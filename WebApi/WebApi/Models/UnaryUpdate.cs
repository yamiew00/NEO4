using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Response;

namespace WebApi.Models
{
    public class UnaryUpdate : Updates
    {
        public string NumberString;

        public UnaryUpdate(string numberString)
        {
            NumberString = numberString;
        }

        public UnaryUpdate(string numberString, Updates updates) : base(updates.RemoveLength, updates.UpdateString)
        {
            NumberString = numberString;
        }
    }
}