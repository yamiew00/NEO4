using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator.Objects
{
    public class FrameObject
    {
        public List<string> EnableList { get; set; }
        public string PanelString { get; set; }
        public string SubPanelString { get; set; }

        public FrameObject()
        {
            PanelString = string.Empty;
            PanelString = string.Empty;
            EnableList = new List<string>();
        }

        public FrameObject(TextBox panel, TextBox subPanel)
        {
            EnableList = new List<string>();
            PanelString = panel.Text;
            SubPanelString = subPanel.Text;
        }

        public void AppendPanel(string str)
        {
            PanelString = new StringBuilder().Append(PanelString)
                                             .Append(str)
                                             .ToString();
        }

        public void SetEnable(params string[] enableList)
        {
            EnableList = enableList.ToList();
        }
    }
}
