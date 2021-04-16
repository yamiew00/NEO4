using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class ClearErrorResponse: StatusMessage
    {
        public Updates Update;

        public ClearErrorResponse()
        {

        }

        public ClearErrorResponse(Updates update)
        {
            Update = update;
        }
        
    }
}