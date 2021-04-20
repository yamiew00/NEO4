using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Response;

namespace WebApi.Models
{
    public class FrameAttribute
    {
        public FrameResponse FrameResponse;
        public string CompleteExpression;

        public FrameAttribute()
        {
            FrameResponse = new FrameResponse();
            CompleteExpression = string.Empty;
        }

        public FrameAttribute(FrameResponse frameResponse, string completeExpression)
        {
            FrameResponse = frameResponse;
            CompleteExpression = completeExpression;
        }
    }
}