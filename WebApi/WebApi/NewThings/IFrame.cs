using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.NewThings
{
    interface IFrame
    {
        FrameObject CreateFrameObject();

        FrameObject CheckOrder();

        FrameObject Tree();
    }
}
