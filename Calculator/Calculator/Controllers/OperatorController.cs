using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //雙元運算子
        private class BinaryOperator
        {
            public int Priority;
            public Func<decimal, decimal, decimal> Formula;

            public BinaryOperator(int priority, Func<decimal, decimal, decimal> formula)
            {
                Priority = priority;
                Formula = formula;
            }

            public BinaryOperator(int priority)
            {
                Priority = priority;
            }
        }

        //單元運算子
        private class UnaryOperator
        {
            public int Priority;
            public Func<decimal, decimal> Formula;

            public UnaryOperator(int priority, Func<decimal, decimal> formula)
            {
                Priority = priority;
                Formula = formula;
            }
        }

        //特殊符號(無)
        private class WildSymbol
        {
            public int Priority;

            public WildSymbol(int priority)
            {
                Priority = priority;
            }
        }

        //運算符與運算規則
        private Dictionary<string, BinaryOperator> BinaryDic;

        private Dictionary<string, UnaryOperator> UnaryDic;

        private Dictionary<string, WildSymbol> WildDic;

        //私有建構子
        private OperatorController()
        {
            BinaryDic = new Dictionary<string, BinaryOperator>();
            //Binary Default
            BinaryDic.Add("+", new BinaryOperator(1, (number1, number2) => number1 + number2));
            BinaryDic.Add("-", new BinaryOperator(1, (number1, number2) => number1 - number2));
            BinaryDic.Add("x", new BinaryOperator(2, (number1, number2) => number1 * number2));
            BinaryDic.Add("÷", new BinaryOperator(2, (number1, number2) => number1 / number2));
            

            //Unary Default
            UnaryDic = new Dictionary<string, UnaryOperator>();
            UnaryDic.Add("±", new UnaryOperator(1, (number) => -1 * number));
            UnaryDic.Add("√", new UnaryOperator(3, (number) => (decimal)Math.Pow((double)number, 0.5)));

            //Wild Default
            WildDic = new Dictionary<string, WildSymbol>();
            WildDic.Add("(", new WildSymbol(-99));
            WildDic.Add(")", new WildSymbol(-99));

        }

        //雙元的
        public void SetOperator(string mark, int priority, Func<decimal, decimal, decimal> formula)
        {
            if (BinaryDic.Keys.Contains(mark))
            {
                BinaryDic[mark] = new BinaryOperator(priority, formula);
            }
            else
            {
                BinaryDic.Add(mark, new BinaryOperator(priority, formula));
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
