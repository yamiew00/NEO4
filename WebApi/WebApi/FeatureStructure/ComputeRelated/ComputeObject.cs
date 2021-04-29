using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Evaluate.Tree;

namespace WebApi.FeatureStructure.ComputeRelated
{
    public class ComputeObject
    {
        public Type LastFeature { get; set; }

        public NumberField NumberField { get; set; }

        public Stack<ExpressionTree> TreeStack { get; set; }

        public decimal CurrentAnswer { get; set; }

        public string CurrentUnaryString { get; set; }
    }
}