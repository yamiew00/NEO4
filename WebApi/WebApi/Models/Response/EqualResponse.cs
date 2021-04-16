using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class EqualResponse: StatusMessage
    {
        public Updates Update;
        public decimal? Answer;

        public EqualResponse()
        {

        }

        public EqualResponse(Updates update, decimal? answer)
        {
            Update = update;
            Answer = answer;
        }
    }
}