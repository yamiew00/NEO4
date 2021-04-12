using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator.Objects
{
    /// <summary>
    /// 畫面物件
    /// </summary>
    public class FrameObject
    {
        /// <summary>
        /// 啟用表
        /// </summary>
        public List<string> EnableList { get; set; }

        /// <summary>
        /// 主畫面字串
        /// </summary>
        public string PanelString { get; set; }

        /// <summary>
        /// 副畫面字串
        /// </summary>
        public string SubPanelString { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="panel">主畫面TextBox</param>
        /// <param name="subPanel">副畫面TextBox</param>
        public FrameObject(TextBox panel, TextBox subPanel)
        {
            EnableList = new List<string>();
            PanelString = panel.Text;
            SubPanelString = subPanel.Text;
        }

        /// <summary>
        /// 主畫面字串擴展
        /// </summary>
        /// <param name="str">新增字串</param>
        public void AppendPanel(string str)
        {
            PanelString = new StringBuilder().Append(PanelString)
                                             .Append(str)
                                             .ToString();
        }

        /// <summary>
        /// 設定啟用表
        /// </summary>
        /// <param name="enableList">啟用表</param>
        public void SetEnable(params string[] enableList)
        {
            EnableList = enableList.ToList();
        }
    }
}
