using System;
using System.Collections;
using System.Collections.Generic;
using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public class UserInfo
    {
        //User-Related。
        public string CompleteExpression { get; set; }

        //User-Related
        public FrameObject FrameObject { get; set; }

        //User-Related
        public Feature PreviousFeature { get; set; }

        public Type LastFeature { get; set; }

        //User-Related。
        public NumberField NumberField { get; set; }
        
        public Stack<ExpressionTree> TreeStack { get; set; }

        //User-Related。
        public decimal CurrentAnswer { get; set; }

        public string CurrentUnaryString { get; set; }

        public UserInfo()
        {
            Init();
        }

        public void Init()
        {
            CompleteExpression = string.Empty;
            FrameObject = new FrameObject();
            PreviousFeature = Feature.Null;
            //初始預設為Clear
            LastFeature = typeof(Clear); 
            NumberField = new NumberField();
            CurrentAnswer = 0;
            TreeStack = new Stack<ExpressionTree>();
            TreeStack.Push(new ExpressionTree());
            CurrentUnaryString = string.Empty;
        }
    }
}