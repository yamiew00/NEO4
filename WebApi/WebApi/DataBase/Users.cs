using System.Collections.Generic;
using System.Linq;
using WebApi.Evaluate;
using WebApi.Evaluate.Tree;
using WebApi.Frames;
using static WebApi.Setting.FeatureRule;

namespace WebApi.DataBase
{
    /// <summary>
    /// 用戶
    /// </summary>
    public static class Users
    {
        private static Dictionary<int, FrameObjectFactory> FrameResponseFactoryDic = new Dictionary<int, FrameObjectFactory>();

        public static FrameObjectFactory GetFreamObjectFactory(int userId)
        {
            if (FrameResponseFactoryDic.Keys.Contains(userId))
            {
                return FrameResponseFactoryDic[userId];
            }
            FrameResponseFactoryDic.Add(userId, new FrameObjectFactory());
            return FrameResponseFactoryDic[userId];
        }



        //新
        private static Dictionary<int, OrderingChecker> OrderingCheckerDic = new Dictionary<int, OrderingChecker>();

        public static OrderingChecker GetOrderingChecker(int userId)
        {
            if (OrderingCheckerDic.Keys.Contains(userId))
            {
                return OrderingCheckerDic[userId];
            }
            OrderingCheckerDic.Add(userId, new OrderingChecker());
            return OrderingCheckerDic[userId];
        }

        //completeExpression
        private static Dictionary<int, string> CompleteExpressionDic = new Dictionary<int, string>();

        public static string GetCompleteExpression(int userId)
        {
            if (CompleteExpressionDic.Keys.Contains(userId))
            {
                return CompleteExpressionDic[userId];
            }
            CompleteExpressionDic.Add(userId, string.Empty);
            return CompleteExpressionDic[userId];
        }

        public static void SetCompleteExpression(int userId, string newString)
        {
            if (CompleteExpressionDic.Keys.Contains(userId))
            {
                CompleteExpressionDic[userId] = newString;
                return ;
            }
            else
            {
                throw new System.Exception("無此用戶");
            }
        }

        //PreviousFeature
        private static Dictionary<int, Feature> PreviousFeatureDic = new Dictionary<int, Feature>();

        public static Feature GetPreviousFeature(int userId)
        {
            if (PreviousFeatureDic.Keys.Contains(userId))
            {
                return PreviousFeatureDic[userId];
            }
            PreviousFeatureDic.Add(userId, Feature.Null);
            return PreviousFeatureDic[userId];
        }

        public static void SetPreviousFeature(int userId, Feature newFeature)
        {
            if (PreviousFeatureDic.Keys.Contains(userId))
            {
                PreviousFeatureDic[userId] = newFeature;
                return;
            }
            else
            {
                throw new System.Exception("無此用戶");
            }
        }

        //numbermachine
        private static Dictionary<int, NumberMachine> NumberMachineDic = new Dictionary<int, NumberMachine>();

        public static NumberMachine GetNumberMachine(int userId)
        {
            if (NumberMachineDic.Keys.Contains(userId))
            {
                return NumberMachineDic[userId];
            }
            NumberMachineDic.Add(userId, new NumberMachine());
            return NumberMachineDic[userId];
        }

        //numberField
        private static Dictionary<int, NumberField> NumberFieldDic = new Dictionary<int, NumberField>();

        public static NumberField GetNumberField(int userId)
        {
            if (NumberFieldDic.Keys.Contains(userId))
            {
                return NumberFieldDic[userId];
            }
            NumberFieldDic.Add(userId, new NumberField());
            return NumberFieldDic[userId];
        }

        public static void SetNumberField(int userId, NumberField numberField)
        {
            if (NumberFieldDic.Keys.Contains(userId))
            {
                NumberFieldDic[userId] = numberField;
                return;
            }
            else
            {
                throw new System.Exception("無此用戶");
            }
        }

        //ExpressionTreeManager
        private static Dictionary<int, ExpressionTreeManager> ExpressionTreeManagerDic = new Dictionary<int, ExpressionTreeManager>();

        public static ExpressionTreeManager GetExpressionTreeManager(int userId)
        {
            if (ExpressionTreeManagerDic.Keys.Contains(userId))
            {
                return ExpressionTreeManagerDic[userId];
            }
            ExpressionTreeManagerDic.Add(userId, new ExpressionTreeManager());
            return ExpressionTreeManagerDic[userId];
        }

        //CurrentAnswer
        private static Dictionary<int, decimal> CurrentAnswerDic = new Dictionary<int, decimal>();

        public static decimal GetCurrentAnswer(int userId)
        {
            if (CurrentAnswerDic.Keys.Contains(userId))
            {
                return CurrentAnswerDic[userId];
            }
            CurrentAnswerDic.Add(userId, 0);
            return CurrentAnswerDic[userId];
        }

        public static void SetCurrentAnswer(int userId, decimal currentAnswer)
        {
            if (CurrentAnswerDic.Keys.Contains(userId))
            {
                CurrentAnswerDic[userId] = currentAnswer;
                return;
            }
            else
            {
                CurrentAnswerDic.Add(userId, currentAnswer);
            }
        }

    }
}