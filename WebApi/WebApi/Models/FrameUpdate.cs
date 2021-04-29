using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Response;

namespace WebApi.Models
{
    /// <summary>
    /// ExpUpdate的子類，除了更新運算式之外，也需要更新目前運算出的數字。
    /// </summary>
    public class FrameUpdate
    {
        /// <summary>
        /// 這代表將要移除全部
        /// </summary>
        public static readonly int REMOVE_ALL = -1;

        /// <summary>
        /// 要往前移除的長度
        /// </summary>
        public int RemoveLength { get; set; }

        /// <summary>
        /// 要更新的字串
        /// </summary>
        public string UpdateString { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer{ get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="answer">答案</param>
        public FrameUpdate(string answer)
        {
            Answer = answer;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="removeLength">要移除的長度</param>
        /// <param name="updateString">更新字串</param>
        public FrameUpdate(int removeLength, string updateString)
        {
            RemoveLength = removeLength;
            UpdateString = updateString;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="answer">顯示答案</param>
        /// <param name="removeLength">要移除的長度</param>
        /// <param name="updateString">更新字串</param>
        public FrameUpdate(string answer, int removeLength, string updateString) : this(removeLength, updateString)
        {
            Answer = answer;
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

            //RemoveALL代表要刪除整個字串
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