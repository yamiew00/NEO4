using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure.Frames
{
    /// <summary>
    /// 面板物件
    /// </summary>
    public class BoardObject
    {
        /// <summary>
        /// 完整表達式
        /// </summary>
        public string CompleteExpression { get; set; }

        /// <summary>
        /// 畫面物件
        /// </summary>
        public FrameObject FrameObject { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public BoardObject()
        {
            Init();
        }

        /// <summary>
        /// 全體初始化
        /// </summary>
        public void Init()
        {
            CompleteExpression = string.Empty;
            FrameObject = new FrameObject();
        }
    }
}