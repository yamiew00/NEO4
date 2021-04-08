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
using Calculator.Extensions;

namespace Calculator.Controllers
{
    /// <summary>
    /// 運算符控制器
    /// </summary>
    public class OperatorController
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static OperatorController Instance;

        /// <summary>
        /// 取得唯一實體
        /// </summary>
        /// <returns></returns>
        public static OperatorController GetInstance()
        {
            if (Instance == null)
            {
                Instance = new OperatorController();
                return Instance;
            }
            return Instance;
        }

        /// <summary>
        /// 雙元運算符規則
        /// </summary>
        private Dictionary<string, BinaryOperator> BinaryDic;

        /// <summary>
        /// 單元運算符規則
        /// </summary>
        private Dictionary<string, UnaryOperator> UnaryDic;

        /// <summary>
        /// 特殊符號
        /// </summary>
        private Dictionary<string, WildSymbol> WildDic;

        /// <summary>
        /// 私有建構子
        /// </summary>
        private OperatorController()
        {
            //現有字典
            BinaryDic = Operators.BinaryDic;
            UnaryDic = Operators.UnaryDic;
            WildDic = Operators.WildDic;
        }

        /// <summary>
        /// 傳入Form，檢查所有contorl設定(特殊接口)
        /// </summary>
        /// <param name="form"></param>
        public void SetRules(Form1 form)
        {
            foreach (var control in form.Controls)
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
                        var postfix = it.GetPostFix(rule);
                        var func = ie.InfixToDelegate(postfix);

                        //這裡開始更新字典
                        Operators.UpdateBinary(button.Text, priority, func);

                        //把更新後的字典傳回這裡存起來
                        BinaryDic = Operators.BinaryDic;
                    }
                }
            }
        }

        /// <summary>
        /// 取得雙元運算符規則
        /// </summary>
        /// <param name="mark">雙元運算符符號</param>
        /// <returns>運算規則</returns>
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

        /// <summary>
        /// 取得單元運算符規則
        /// </summary>
        /// <param name="mark">單元運算符符號</param>
        /// <returns>運算規則</returns>
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

        /// <summary>
        /// 取得所有雙元運算符符號
        /// </summary>
        /// <returns>所有雙元運算符符號</returns>
        public List<string> GetBinaryMarks()
        {
            return BinaryDic.Keys.ToList();
        }

        /// <summary>
        /// 取得所有單元運算符符號
        /// </summary>
        /// <returns>所有單元運算符符號</returns>
        public List<string> GetUnaryMarks()
        {
            return UnaryDic.Keys.ToList();
        }

        /// <summary>
        /// 取得優先度
        /// </summary>
        /// <param name="mark">運算符符號</param>
        /// <returns>優先度</returns>
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
