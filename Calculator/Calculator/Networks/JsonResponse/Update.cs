using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.News.JsonResponse
{
    /// <summary>
    /// 更新內容
    /// </summary>
    public class Update
    {
        /// <summary>
        /// 要取消(移除)的長度
        /// </summary>
        public int RemoveLength;

        /// <summary>
        /// 新增的字串
        /// </summary>
        public string UpdateString;

        /// <summary>
        /// 建構子
        /// </summary>
        public Update()
        {
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="removeLength">要取消(移除)的長度</param>
        /// <param name="updateString">新增的字串</param>
        public Update(int removeLength, string updateString)
        {
            RemoveLength = removeLength;
            UpdateString = updateString;
        }
    }
}
