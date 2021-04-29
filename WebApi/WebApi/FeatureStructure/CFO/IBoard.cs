using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure.CFO
{
    public interface IBoard
    {
        FrameObject CreateFrameObject(FrameUpdate frameUpdate);
    }
}
