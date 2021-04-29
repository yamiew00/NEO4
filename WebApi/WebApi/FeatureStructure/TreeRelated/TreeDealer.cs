using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DataBase;
using WebApi.FeatureStructure.TreeRelated;
using WebApi.Models;

namespace WebApi.FeatureStructure
{
    public class TreeDealer : ITree
    {
        private Feature Feature;

        private int UserId;

        public TreeDealer(int userId)
        {
            UserId = userId;
        }
        
        public FrameUpdate UseTree(ComputeObject computeObject)
        {
            var frameUpdate = Feature.UseTree(computeObject);
            Users.GetUserInfo2(UserId).ComputeObject = computeObject;
            return frameUpdate;
        }
    }
}