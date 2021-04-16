using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class UnaryResponse: StatusMessage
    {
        public Updates Update;

        public UnaryResponse()
        {

        }

        public UnaryResponse(Updates update)
        {
            Update = update;
        }
        
    }
}