using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class LeftBracketResponse : StatusMessage
    {
        public Updates Update;
        
        public LeftBracketResponse()
        {

        }

        public LeftBracketResponse(Updates update)
        {
            Update = update;
        }
        
    }
}