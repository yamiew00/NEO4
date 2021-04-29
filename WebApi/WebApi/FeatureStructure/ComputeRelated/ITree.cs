using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.FeatureStructure.ComputeRelated
{
    public interface ITree
    {
        FrameUpdate GetFrameUpdate(Type lastFeatureType, ComputeObject computeObject);
    }
}