using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.FeatureStructure.OrderRelated
{
    public class OrderDealer : IOrder
    {
        private Feature Feature;
        
        private int UserId;

        public OrderDealer(int userId)
        {
            UserId = userId;
        }

        public FrameUpdate Ordering(Type lastFeature)
        {
            throw new NotImplementedException();
        }
    }
}