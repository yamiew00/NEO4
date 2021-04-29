using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.FeatureStructure.OrderRelated
{
    public interface IOrder
    {
        FrameUpdate Ordering(Type lastFeature);
    }
}
