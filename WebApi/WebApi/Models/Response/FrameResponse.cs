using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    public class FrameResponse
    {
        public string SubPanel;
        public string Panel;

        public FrameResponse()
        {
            SubPanel = string.Empty;
            Panel = string.Empty;
        }

        public FrameResponse(string subPanel, string panel)
        {
            SubPanel = subPanel;
            Panel = panel;
        }
    }
}