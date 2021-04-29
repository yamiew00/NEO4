using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.FeatureStructure.Computes;
using WebApi.FeatureStructure.Frames;

namespace WebApi.DataBase
{
    /// <summary>
    /// 用戶資訊
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 畫面資料
        /// </summary>
        public BoardObject BoardObject { get; set; }

        /// <summary>
        /// 計算資料
        /// </summary>
        public ComputeObject ComputeObject { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public UserInfo()
        {
            BoardObject = new BoardObject();
            ComputeObject = new ComputeObject();
        }

        /// <summary>
        /// 全內容初始化
        /// </summary>
        public void Init()
        {
            BoardObject.Init();
            ComputeObject.Init();
        }
    }
}