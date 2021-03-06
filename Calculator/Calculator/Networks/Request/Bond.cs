using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Networks.Request
{
    /// <summary>
    /// 功能組合
    /// </summary>
    public class Bond
    {   
        /// <summary>
        /// 功能種類
        /// </summary>
        private static string _Feature;

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
        /// 功能內容
        /// </summary>
        private char _Content;

        /// <summary>
        /// 功能內容
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

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="feature">功能種類</param>
        /// <param name="content">功能內容</param>
        public Bond(string feature, char content)
        {
            Feature = feature;
            Content = content;
        }
    }
}
