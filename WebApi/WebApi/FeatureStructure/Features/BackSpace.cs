using System;
using System.Collections.Generic;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure
{
    /// <summary>
    /// 返回鍵:Concrete IFeature物件
    /// </summary>
    public class BackSpace : Feature
    {
        //private static HashSet<Type> LegitAfterWardTypeS = new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(RightBracket), typeof(Clear), typeof(BackSpace), typeof(Unary) };

        //public override HashSet<Type> LegitAfterWardType2 { get => LegitAfterWardTypeS; }
        

        /// <summary>
        /// 空建構子。反射用的
        /// </summary>
        public BackSpace()
        {
        }

        /// <summary>
        /// 根據OrderingDealer方法的回傳值，製造畫面物件。
        /// </summary>
        /// <returns>畫面面件</returns>
        protected override FrameObject CreateFrameObject()
        {
            FrameUpdate frameUpdate = OrderingDealer();

            //完整運算式的刷新
            CompleteExpression = frameUpdate.Refresh(CompleteExpression);

            //panel, subpanel設定。
            FrameObject.SubPanel = CompleteExpression;
            FrameObject.Panel = frameUpdate.Refresh(FrameObject.Panel);

            return FrameObject;
        }

        /// <summary>
        /// 回傳此功能後面可以接的功能集
        /// </summary>
        /// <returns>後面可以接的功能集</returns>
        public override HashSet<Type> LegitAfterWardType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(Binary), typeof(Equal), typeof(LeftBracket), typeof(RightBracket), typeof(Clear), typeof(BackSpace), typeof(Unary) };
        }

        /// <summary>
        /// 回傳此功能前面可以接的功能集
        /// </summary>
        /// <returns>前面可以接的功能集</returns>
        public override HashSet<Type> LegitPreviousType()
        {
            return new HashSet<Type>() { typeof(Number), typeof(BackSpace) };
        }

        /// <summary>
        /// 根據Tree方法的回傳值，製造畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate OrderingDealer()
        {
            FrameUpdate frameUpdate = Tree();
            return frameUpdate;
        }

        /// <summary>
        /// 將運算結果，製成畫面更新。
        /// </summary>
        /// <returns>畫面更新</returns>
        protected override FrameUpdate Tree()
        {
            int RemoveLength = 0;
            if (NumberField == null)
            {
                return new FrameUpdate(removeLength: RemoveLength, updateString: string.Empty);
            }

            RemoveLength = NumberField.Number.Value.ToString().Length;
            if (NumberField.IsEndWithPoint())
            {
                RemoveLength += 1;
            }
            NumberField.ClearOneDigit();
            string updateString = NumberField.Number.ToString();
            if (NumberField.IsEndWithPoint())
            {
                updateString += ".";
            }
            return new FrameUpdate(RemoveLength, updateString);
        }

        /// <summary>
        /// 回傳新增物件的方法
        /// </summary>
        /// <returns>委派</returns>
        public override Func<char, Feature> Create()
        {
            return (content) => new BackSpace();
        }
    }
}