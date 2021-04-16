using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class BackSpaceResponse: StatusMessage
    {
        public Updates Update;

        public BackSpaceResponse()
        {

        }

        public BackSpaceResponse(Updates update)
        {
            Update = update;
        }
        
    }
}