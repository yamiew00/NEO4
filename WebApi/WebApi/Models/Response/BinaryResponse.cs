using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class BinaryResponse: StatusMessage
    {
        public Updates Update;
        
        public BinaryResponse()
        {
        }

        public BinaryResponse(Updates update)
        {
            Update = update;
        }
    }
}