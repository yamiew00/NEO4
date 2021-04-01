using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Setting;
using Calculator.CalculateObjects;
using System.Windows.Forms;
using Calculator.Tools;
using Calculator.Tools.Lambdas;

namespace Calculator.Controllers
{
    public class OperatorController
    {
        private static OperatorController Instance;

        public static OperatorController GetInstance()
        {
            if (Instance == null)
            {
                Instance = new OperatorController();
                return Instance;
            }
            return Instance;
        }

        //運算符與運算規則
        private Dictionary<string, BinaryOperator> BinaryDic;

        private Dictionary<string, UnaryOperator> UnaryDic;

        private Dictionary<string, WildSymbol> WildDic;

        //私有建構子
        private OperatorController()
        {
            //現有字典
            BinaryDic = Operators.BinaryDic;
            UnaryDic = Operators.UnaryDic;
            WildDic = Operators.WildDic;
        }

        //傳入Form，檢查所有contorl設定
        public void SetRules(Form1 form)
        {
            foreach(var control in form.Controls)
            {
                if (control is Button)
                {
                    var button = (Button)control;
                    var tag = button.Tag;
                    if (tag != null && tag.ToString().Split(',').Length == 2)
                    {
                        var priorityStr = tag.ToString().Split(',')[0];
                        var rule = tag.ToString().Split(',')[1];

                        //priority轉成數字
                        int.TryParse(priorityStr, out int priority);

                        //把rule轉換成Func
                        IEvaluator ie = new IEvaluator();
                        ITree it = new ITree();
                        var infix = it.GetPostFix(rule);
                        var func = ie.InfixToDelegate(infix);

                        //這裡開始更新字典
                        Operators.UpdateBinary(button.Text, priority, func);

                        //把更新後的字典傳回這裡存起來
                        BinaryDic = Operators.BinaryDic;
                    }
                }
            }
        }
        

        public Func<decimal, decimal, decimal> GetBinaryFormula(string mark)
        {
            if (BinaryDic.Keys.Contains(mark))
            {
                return BinaryDic[mark].Formula;
            }
            else
            {
                throw new Exception("No such rule");
            }
        }

        public Func<decimal, decimal> GetUnaryFormula(string mark)
        {
            if (UnaryDic.Keys.Contains(mark))
            {
                return UnaryDic[mark].Formula;
            }
            else
            {
                throw new Exception("No such rule");
            }
        }

        public List<string> GetBinaryMarks()
        {
            return BinaryDic.Keys.ToList();
        }

        public List<string> GetUnaryMarks()
        {
            return UnaryDic.Keys.ToList();
        }

        //共用
        public int GetPriority(string mark)
        {
            if (BinaryDic.Keys.Contains(mark))
            {
                return BinaryDic[mark].Priority;
            }
            else if (UnaryDic.Keys.Contains(mark))
            {
                return UnaryDic[mark].Priority;
            }
            else if (WildDic.Keys.Contains(mark))
            {
                return WildDic[mark].Priority;
            }
            else
            {
                throw new Exception("No such rule");
            }
        }
    }
}
