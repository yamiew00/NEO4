using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class Status
    {
        public int Code { get; private set; }

        public Status()
        {

        }

        public Status(int code)
        {
            Code = code;
        }


        public void SetStatus(int code)
        {
            Code = code;
        }
    }
}