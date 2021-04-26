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
    public class FrameUpdate : ExpUpdate
    {
        /// <summary>
        /// 答案
        /// </summary>
        private string _Answer;

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }

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
        /// <param name="answer">答案</param>
        /// <param name="expUpdate">運算式更新內容</param>
        public FrameUpdate(string answer, ExpUpdate expUpdate) : base(expUpdate.RemoveLength, expUpdate.UpdateString)
        {
            Answer = answer;
        }
    }
}