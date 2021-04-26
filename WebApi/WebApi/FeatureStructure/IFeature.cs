using System;
using System.Collections.Generic;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    public abstract class IFeature : FeatureObject
    {
        protected HashSet<Feature> FeatureSet;

        private readonly Dictionary<Type, Func<FrameObject, int, FrameObject>> ExceptionDic = new Dictionary<Type, Func<FrameObject, int, FrameObject>>()
        {
            {
                //BracketException:括號數量不正確→畫面維持原狀
                typeof(BracketException),  (frameObject, userId) => frameObject
            },
            {
                //TooLongDigitException:輸入位數限制→畫面維持原狀
                typeof(TooLongDigitException), (frameObject, userId) => frameObject
            },
            {
                typeof(SquareRootException), (frameObject, userId) =>
                {
                   //對負號做根號運算→回傳錯訊
                    var subpanel = frameObject.SubPanel;

                    //清除
                    new Clear(userId).GetFrameObject();
                    

                    return new FrameObject(panel: "無效的輸入", subPanel: subpanel);
                }
            },
            {
                typeof(DivideByZeroException), (frameObject, userId) =>
                {
                    //零在分母→回傳錯訊
                    var subpanel = frameObject.SubPanel;

                    //清除
                    new Clear(userId).GetFrameObject();

                    return new FrameObject(panel: "無法除以零。", subPanel: subpanel);
                }
            }
        };
        
        public IFeature(int userId) : base(userId)
        {
        }

        public IFeature(int userId, char content) : base(userId, content)
        {
        }

        public FrameObject GetFrameObject()
        {
            //若順序不對則返回原畫面
            if (!FeatureSet.Contains(PreviousFeature))
            {
                return FrameObject;
            }

            
            //其他exception的情況
            try
            {
                return CreateFrameObject();
            }
            catch(Exception exception)
            {
                if (ExceptionDic.ContainsKey(exception.GetType()))
                {
                    return ExceptionDic[exception.GetType()].Invoke(FrameObject, UserId);
                }
                else
                {
                    //沒處理到的情況→直接死機
                    throw new Exception("未處理狀況");
                }
            }

        }

        protected abstract FrameObject CreateFrameObject();

        protected abstract FrameUpdate OrderingDealer();

        protected abstract FrameUpdate Tree();
    }
}
