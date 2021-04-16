using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class ClearResponse : StatusMessage
    {
        public ClearResponse(int code): base(code)
        {
            
        }
    }
}