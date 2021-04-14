using Calculator.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Frames
{
    /// <summary>
    /// Tag介面，擁有決定畫面表現與決定啟用表的方法
    /// </summary>
    public interface ITag
    {
        /// <summary>
        /// 根據text內容決定畫面
        /// </summary>
        /// <param name="text">text內容</param>
        /// <param name="frameObject">畫面</param>
        /// <returns>Task</returns>
        Task SetFrame(string text, FrameObject frameObject);

        /// <summary>
        /// 回傳啟用表
        /// </summary>
        /// <returns></returns>
        List<string> GetEnableList();
    }
}
