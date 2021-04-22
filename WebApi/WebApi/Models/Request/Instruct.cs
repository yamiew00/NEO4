using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Request
{
    /// <summary>
    /// 前端的按鈕功能組合
    /// </summary>
    public class Instruct
    {
        /// <summary>
        /// 功能種類
        /// </summary>
        private string _Feature;

        /// <summary>
        /// 功能種類
        /// </summary>
        public string Feature
        {
            get
            {
                return _Feature;
            }

            set
            {
                _Feature = value;
            }
        }

        /// <summary>
        /// 命令內容
        /// </summary>
        private char _Content;

        /// <summary>
        /// 命令內容
        /// </summary>
        public char Content
        {
            get
            {
                return _Content;
            }

            set
            {
                _Content = value;
            }
        }
    }
}