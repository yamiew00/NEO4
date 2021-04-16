using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class RightBracketResponse : StatusMessage
    {
        public Updates Update;

        public RightBracketResponse()
        {

        }

        public RightBracketResponse(Updates update)
        {
            Update = update;
        }
        
    }
}