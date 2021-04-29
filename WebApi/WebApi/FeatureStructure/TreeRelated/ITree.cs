using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.FeatureStructure.TreeRelated;
using WebApi.Models;

namespace WebApi.FeatureStructure
{
    public interface ITree
    {
        FrameUpdate UseTree(ComputeObject computeObject);
    }
}