using Calculator.Frames;
using Calculator.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Extensions;

namespace Calculator.Tags
{
    /// <summary>
    /// 畫面處理，經由tag與text來處理frameObject
    /// </summary>
    public class FrameManager : ITag
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static FrameManager _instance = new FrameManager();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static FrameManager Instance
        {
            get
            {
                return _instance; 
            }
        }

        /// <summary>
        /// 私有建構子
        /// </summary>
        private FrameManager()
        {
        }

        /// <summary>
        /// 要動作的ITag
        /// </summary>
        private ITag ITag;

        /// <summary>
        /// 上一次使用的ITag;
        /// </summary>
        private ITag LastITag;
        
        /// <summary>
        /// 變更要處理的ITag
        /// </summary>
        /// <param name="tag">tag名稱</param>
        public void ChangeTag(string tag)
        {
            ITag = ITagFactory.CreateITag(tag);
        }

        /// <summary>
        /// 回傳啟用表
        /// </summary>
        /// <returns>啟用表</returns>
        public List<string> GetEnableList()
        {
            return LastITag.GetEnableList();
        }

        /// <summary>
        /// 畫面設定
        /// </summary>
        /// <param name="text">Control的text</param>
        /// <param name="frameObject">畫面物件</param>
        /// <returns>Task</returns>
        public Task SetFrame(string text, FrameObject frameObject)
        {
            return Task.Run(async () =>
            {
                //執行特定順序的Tag時該處理的特別狀況
                TagCombo(frameObject);

                await ITag.SetFrame(text, frameObject);

                //記錄這次的Tag
                RecordThisItag();
            });
        }

        /// <summary>
        /// 當執行特定順序的Tag時該處理的特別狀況
        /// </summary>
        /// <param name="frameObject">畫面物件</param>
        private void TagCombo(FrameObject frameObject)
        {
            if (ITag == Operator.Instance && LastITag == Operator.Instance)
            {
                frameObject.PanelString = frameObject.PanelString.RemoveLast(1);
            }
            else if (LastITag == Equal.Instance)
            {
                frameObject.PanelString = string.Empty;
                frameObject.SubPanelString = string.Empty;
            }
        }

        /// <summary>
        /// 記錄這次的Tag
        /// </summary>
        private void RecordThisItag()
        {
            LastITag = ITag;
        }
    }
}
