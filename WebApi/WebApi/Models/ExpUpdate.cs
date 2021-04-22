using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// Expression的待更新內容
    /// </summary>
    public class ExpUpdate
    {
        /// <summary>
        /// 這代表將要移除全部
        /// </summary>
        public static readonly int REMOVE_ALL = -1;

        /// <summary>
        /// 要往前移除的長度
        /// </summary>
        private int _RemoveLength;

        /// <summary>
        /// 要往前移除的長度
        /// </summary>
        public int RemoveLength
        {
            get
            {
                return _RemoveLength;
            }

            set
            {
                _RemoveLength = value;
            }
        }

        /// <summary>
        /// 要更新的字串
        /// </summary>
        private string _UpdateString;

        /// <summary>
        /// 要更新的字串
        /// </summary>
        public string UpdateString
        {
            get
            {
                return _UpdateString;
            }

            set
            {
                _UpdateString = value;
            }
        }

        /// <summary>
        /// 空建構子(必須要存在)
        /// </summary>
        public ExpUpdate()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="removeLength">要往前移除的長度</param>
        /// <param name="updateString">要更新的字串</param>
        public ExpUpdate(int removeLength, string updateString)
        {
            RemoveLength = removeLength;
            UpdateString = updateString;
        }

        /// <summary>
        /// 刷新運算式內容
        /// </summary>
        /// <param name="expression">運算式</param>
        /// <returns>新的運算式</returns>
        public string Refresh(string expression)
        {
            if (expression == null)
            {
                expression = string.Empty;
            }

            //-1代表
            if (RemoveLength == REMOVE_ALL)
            {
                RemoveLength = expression.Length;
            }

            //remove
            expression = expression.Substring(0, expression.Length - RemoveLength);

            //update
            expression += UpdateString;

            return expression;
        }
    }
}