using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Response
{
    /// <summary>
    /// 應更新內容
    /// </summary>
    public class Updates
    {
        public static readonly int REMOVE_ALL = -1;

        /// <summary>
        /// 要往前移除的長度
        /// </summary>
        public int RemoveLength { get; set; }

        /// <summary>
        /// 要更新的字串
        /// </summary>
        public string UpdateString;

        public Updates()
        {

        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="removeLength">要往前移除的長度</param>
        /// <param name="updateString">要更新的字串</param>
        public Updates(int removeLength, string updateString)
        {
            RemoveLength = removeLength;
            UpdateString = updateString;
        }
        
        public string Refresh(string str)
        {
            if (str == null)
            {
                str = string.Empty;
            }

            //-1代表
            if (RemoveLength == REMOVE_ALL)
            {
                RemoveLength = str.Length;
            }

            //remove
            str = str.Substring(0, str.Length - RemoveLength);

            //update
            str += UpdateString;

            return str;
        }
    }
}