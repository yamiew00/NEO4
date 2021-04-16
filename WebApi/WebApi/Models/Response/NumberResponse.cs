using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class NumberResponse : StatusMessage
    {
        public Updates Update;
        
        public NumberResponse(Updates update)
        {
            Update = update;
        }

        public NumberResponse()
        {
        }
        
    }
}