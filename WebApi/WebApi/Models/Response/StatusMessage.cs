using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class StatusMessage
    {
        public Status Status;

        public StatusMessage()
        {

        }
        public StatusMessage(int code)
        {
            Status = new Status(code);
        }

        public void SetStatus(int code)
        {
            Status = new Status(code);
        }
    }
}