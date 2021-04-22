using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// 畫面物件
    /// </summary>
    public class FrameObject
    {
        /// <summary>
        /// 子畫面(上方畫面)
        /// </summary>
        private string _SubPanel;

        /// <summary>
        /// 子畫面(上方畫面)
        /// </summary>
        public string SubPanel
        {
            get
            {
                return _SubPanel;
            }

            set
            {
                _SubPanel = value;
            }
        }

        /// <summary>
        /// 主畫面(下方畫面)
        /// </summary>
        private string _Panel;

        /// <summary>
        /// 主畫面(下方畫面)
        /// </summary>
        public string Panel
        {
            get
            {
                return _Panel;
            }

            set
            {
                _Panel = value;
            }
        }

        /// <summary>
        /// 建構子
        /// </summary>
        public FrameObject()
        {
            SubPanel = string.Empty;
            Panel = string.Empty;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="subPanel">子畫面字串</param>
        /// <param name="panel">主畫面字串</param>
        public FrameObject(string subPanel, string panel)
        {
            SubPanel = subPanel;
            Panel = panel;
        }
    }
}