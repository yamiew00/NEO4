using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Evaluate.Tree;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 數字鍵:Concrete IFeature物件
    /// </summary>
    public class Number : Feature
    {

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userid">用戶id</param>
        /// <param name="content">功能內容</param>
        public Number(char content) : base(content)
        {
        }

        /// <summary>
        /// 空建構子(給反射用的)
        /// </summary>
        public Number()
        {
        }
        

        /// <summary>
        /// 根據OrderingDealer方法的回傳值，製造畫面物件。
        /// </summary>
        /// <returns>畫面面件</returns>
        protected override FrameObject CreateFrameObject()
        {
            FrameUpdate frameUpdate = OrderingDealer();
            string answer = frameUpdate.Answer;

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定
            FrameObject.SubPanel = CompleteExpression.Substring(0, CompleteExpression.Length - answer.Length);
            FrameObject.Panel = answer;
            return FrameObject;
        }

        /// <summary>
        /// 回傳此功能後面可以接的功能集
        /// </summary>
        /// <returns>後面可以接的功能集</returns>
        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(RightBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace), typeof(Unary) };
        }

        /// <summary>
        /// 回傳此功能前面可以接的功能集
        /// </summary>
        /// <returns>前面可以接的功能集</returns>
        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(Clear), typeof(ClearError), typeof(BackSpace) };
        }

        /// <summary>
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate;

            //等號後輸入數字
            if (LastFeature == typeof(Equal))
            {
                //必須要把之前的運算全部清空
                InitAllInfo();

                frameUpdate = Tree();
                frameUpdate.RemoveLength = FrameUpdate.REMOVE_ALL;
                return frameUpdate;
            }

            //backspace或clearerror之後的數字處理
            if ((LastFeature == typeof(BackSpace) || LastFeature == typeof(ClearError)) && NumberField.Number == 0)
            {
                frameUpdate = Tree();
                frameUpdate.RemoveLength += 1;
                
                return frameUpdate;
            }

            frameUpdate = Tree();
            return frameUpdate;
        }

        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate Tree()
        {
            //小數點case
            if (Content.Equals('.'))
            {
                //若這時沒有值→視為已經輸入零
                if (NumberField == null)
                {
                    NumberField = new NumberField();
                    NumberField.AddDigit(Content);
                    return new FrameUpdate("0.", removeLength: 0, updateString: "0.");
                }
                //若有值→判斷是否已為小數
                if (NumberField.IsNumeric)
                {
                    //已經是小數→不回傳值
                    return new FrameUpdate(NumberField.Number.ToString() + ".", removeLength: 0, updateString: string.Empty);
                }
                else
                {
                    //不是小數→更新小數點
                    NumberField.AddDigit(Content);
                    return new FrameUpdate(NumberField.Number.ToString() + ".", removeLength: 0, updateString: ".");
                }
            }

            //防呆
            if (!char.IsNumber(Content))
            {
                throw new Exception("該輸入數字");
            }

            //正常的數字case
            if (NumberField == null)
            {
                NumberField = new NumberField(Content);
            }
            else
            {
                NumberField.AddDigit(Content);
            }
            return new FrameUpdate(NumberField.Number.ToString(), removeLength: 0, updateString: Content.ToString());
        }

        /// <summary>
        /// 回傳新增物件的方法
        /// </summary>
        /// <returns>委派</returns>
        public override Func<char, Feature> Create()
        {
            return (content) => new Number(content);
        }
    }
}