using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Response;

namespace WebApi.Models
{
    public class BinaryUpdate : Updates
    {
        public string NumberString;

        public BinaryUpdate(string numberString)
        {
            NumberString = numberString;
        }

        public BinaryUpdate(string numberString, Updates updates) : base(updates.RemoveLength, updates.UpdateString)
        {
            NumberString = numberString;
        }
    }
}