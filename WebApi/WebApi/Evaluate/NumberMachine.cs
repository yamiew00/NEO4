using System;
using WebApi.Evaluate.Operators;
using WebApi.Evaluate.Tree;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.Evaluate
{
    /// <summary>
    /// 數字處理器
    /// </summary>
    public class NumberMachine
    {
        /// <summary>
        /// 表示式控制器，此類別的回傳值仰賴此物件的回傳值。
        /// </summary>
        private ExpressionTreeManager ExpTreeManager;

        /// <summary>
        /// 數字物件
        /// </summary>
        private NumberField _NumberField;

        /// <summary>
        /// 數字物件
        /// </summary>
        public NumberField NumberField
        {
            get
            {
                return _NumberField;
            }

            set
            {
                _NumberField = value;
            }
        }

        /// <summary>
        /// 目前的計算答案
        /// </summary>
        private decimal CurrentAnswer;

        /// <summary>
        /// 最後一次輸入的數字
        /// </summary>
        private decimal? LastKeyInNumber;

        /// <summary>
        /// 第一個按下的雙元運算
        /// </summary>
        private BinaryOperator FirstBinaryOperator;

        /// <summary>
        /// 建構子
        /// </summary>
        public NumberMachine()
        {
            ExpTreeManager = new ExpressionTreeManager();
        }
        
        /// <summary>
        /// 新增一個位數
        /// </summary>
        /// <param name="number">一個位數</param>
        /// <returns>數字更新</returns>
        public FrameUpdate AddNumber(char number)
        {
            //小數點case
            if (number.Equals('.'))
            {
                //若這時沒有值→視為已經輸入零
                if (NumberField == null)
                {
                    NumberField = new NumberField();
                    NumberField.AddDigit(number);
                    return new FrameUpdate("0.", new ExpUpdate(removeLength: 0, updateString: "0."));
                }
                //若有值→判斷是否已為小數
                if (NumberField.IsNumeric)
                {
                    //已經是小數→不回傳值
                    return new FrameUpdate(NumberField.Number.ToString() + ".", new ExpUpdate(removeLength: 0, updateString: string.Empty));
                }
                else
                {
                    //不是小數→更新小數點
                    NumberField.AddDigit(number);
                    return new FrameUpdate(NumberField.Number.ToString() + ".", new ExpUpdate(removeLength: 0, updateString: "."));
                }
            }

            //防呆
            if (!char.IsNumber(number))
            {
                throw new Exception("NewControll.Add該輸入數字");
            }

            //正常的數字case
            if (NumberField == null)
            {
                NumberField = new NumberField(number);
            }
            else
            {
                NumberField.AddDigit(number);
            }
            return new FrameUpdate(NumberField.Number.ToString(), new ExpUpdate(removeLength: 0, updateString: number.ToString()));
        }

        /// <summary>
        /// 更改一個雙元運算子
        /// </summary>
        /// <param name="binary">更換的雙元運算子</param>
        /// <returns>雙元運算子響應</returns>
        public FrameUpdate ModifyBinary(char binary)
        {
            BinaryOperator binaryOperator = Setting.Operators.GetBinary(binary);
            if (NumberField != null)
            {
                throw new Exception("ModifyBinary時Number不能有值");
            }
            ExpTreeManager.Modify(binaryOperator);
            return new FrameUpdate(CurrentAnswer.ToString(), new ExpUpdate(removeLength: 1, updateString: binary.ToString()));
        }

        /// <summary>
        /// 新增一個雙元運算子。成功執行後Number會變null。
        /// </summary>
        /// <param name="binary">雙元運算子</param>
        /// <returns>雙元運算子的響應</returns>
        public FrameUpdate AddBinary(char binary)
        {
            BinaryOperator binaryOperator = Setting.Operators.GetBinary(binary);

            if (NumberField == null)
            {
                //防呆。理想中不應走到這條路
                ExpTreeManager.Add(binaryOperator);
            }
            else
            {
                //記下最後一次輸入完畢後的數字
                LastKeyInNumber = NumberField.Number.Value;

                //記下「第一個」運算元
                if (FirstBinaryOperator == null)
                {
                    FirstBinaryOperator = binaryOperator;
                }

                ExpTreeManager.Add(NumberField.Number.Value);
                NumberField = null;
                //算出一個暫時的結果並存下
                CurrentAnswer = ExpTreeManager.TryGetTmpResult();
                
                ExpTreeManager.Add(binaryOperator);
            }
            return new FrameUpdate(CurrentAnswer.ToString(), new ExpUpdate(removeLength: 0, updateString: binary.ToString()));
        }

        /// <summary>
        /// 首次輸入的運算符初始化
        /// </summary>
        private void InitFirstBinaryOperator()
        {
            FirstBinaryOperator = null;
        }

        /// <summary>
        /// 暫存的單元運算字串
        /// </summary>
        private string TmpUnaryString;

        /// <summary>
        /// 新增一個單元運算子
        /// </summary>
        /// <param name="unary">單元運算子</param>
        /// <returns>單元運算子響應</returns>
        public FrameUpdate AddUnary(char unary)
        {   
            //拿到單元運算的公式
            UnaryOperator unaryOperator = Setting.Operators.GetUnary(unary);
            var formula = unaryOperator.Formula;

            int removeLength = 0;
            if (NumberField == null)
            {
                return new FrameUpdate("0", new ExpUpdate(removeLength: removeLength, updateString: string.Empty));
            }

            removeLength = NumberField.Number.Value.ToString().Length;
            if (NumberField.IsEndWithPoint())
            {
                removeLength += 1;
            }

            //暫存
            var OriginNumberString = NumberField.Number.ToString();
            string updateString = string.Empty;

            //必須case by case處理
            if (unary == '√')
            {
                updateString = $"√({OriginNumberString})";
            }
            else if (unary == '±')
            {
                updateString = $"negate({OriginNumberString})";
            }
            else
            {
                throw new Exception("無此運算元");
            }

            //計算後將結果存起來
            NumberField.Number = formula(NumberField.Number.Value);

            //記住這次的結果
            TmpUnaryString = updateString;
            return new FrameUpdate(NumberField.Number.ToString(), new ExpUpdate(removeLength, updateString));
        }
        
        /// <summary>
        /// 新增一個單元運算子
        /// </summary>
        /// <param name="unary">單元運算子</param>
        /// <returns>單元運算子響應</returns>
        public FrameUpdate AddUnaryCombo(char unary)
        {
            //取得單元運算子
            UnaryOperator unaryOperator = Setting.Operators.GetUnary(unary);
            var formula = unaryOperator.Formula;

            int removeLength = TmpUnaryString.Length;
            
            //必須case by case處理
            if (unary == '√')
            {
                TmpUnaryString = $"√({TmpUnaryString})";
                if (NumberField.Number.HasValue && NumberField.Number.Value <= 0)
                {
                    throw new SquareRootException("無效的輸入");
                }
            }
            else if (unary == '±')
            {
                TmpUnaryString = $"negate({TmpUnaryString})";
            }
            else
            {
                throw new Exception("無此運算元");
            }

            //計算後將結果存起來
            NumberField.Number = formula(NumberField.Number.Value);

            return new FrameUpdate(NumberField.Number.ToString(), new ExpUpdate(removeLength, TmpUnaryString));
        }

        /// <summary>
        /// 等號事件
        /// </summary>
        /// <returns>等號響應</returns>
        public FrameUpdate Equal()
        {
            //這裡的case應該還有更多
            if (NumberField != null)
            {
                ExpTreeManager.Add(NumberField.Number.Value);
                LastKeyInNumber = NumberField.Number.Value;
                NumberField = null;
            }

            //result處理
            var result = ExpTreeManager.TryGetResult();
            var updateString = string.Empty;
            for (int i = 0; i < result.ExtraRightBracketCount; i++)
            {
                updateString += ")";
            }

            //送出結果
            updateString += "=";
            var ans = result.Answer;
            LastKeyInNumber = ans;
            return new FrameUpdate(ans.ToString(), new ExpUpdate(removeLength: 0, updateString: updateString));
        }

        /// <summary>
        /// 雙元運算後直接按等號的處理方法
        /// </summary>
        /// <returns>FrameUpdate</returns>
        public FrameUpdate BinaryAndEqualCombo()
        {
            //CurrentAnswer已經被Binary算g好了
            ExpTreeManager.Add(CurrentAnswer);
            
            //result處理
            var result = ExpTreeManager.TryGetResult();
            var ans = result.Answer;

            //做更新字串
            var updateString = CurrentAnswer.ToString();
            for (int i = 0; i < result.ExtraRightBracketCount; i++)
            {
                updateString += ")";
            }
            
            //送出結果
            updateString += "=";
            
            LastKeyInNumber = ans;
            return new FrameUpdate(ans.ToString(), new ExpUpdate(removeLength: 0, updateString: updateString));
        }

        /// <summary>
        /// 左括號事件
        /// </summary>
        /// <returns>ExpUpdate</returns>
        public ExpUpdate LeftBracket()
        {
            ExpTreeManager.LeftBracket();
            return new ExpUpdate(removeLength: 0, updateString: "(");
        }

        /// <summary>
        /// 右括號事件
        /// </summary>
        /// <returns>FrameObject</returns>
        public FrameUpdate RightBracket()
        {
            string updateString = string.Empty;

            if (NumberField != null)
            {
                ExpTreeManager.Add(NumberField.Number.Value);
            }
            else if (NumberField == null)
            {
                ExpTreeManager.Add(CurrentAnswer);
                updateString += CurrentAnswer.ToString();
            }

            ExpTreeManager.RightBracket();
            updateString += ")";

            NumberField = null;

            CurrentAnswer = ExpTreeManager.TryGetTmpResult();
            
            return new FrameUpdate(CurrentAnswer.ToString(), new ExpUpdate(removeLength: 0, updateString: updateString));
        }

        /// <summary>
        /// Clear事件
        /// </summary>
        public void Clear()
        {
            NumberField = null;
            ExpTreeManager.Clear();
        }

        /// <summary>
        /// ClearError事件
        /// </summary>
        /// <returns>ExpUpdate</returns>
        public ExpUpdate ClearError()
        {
            if (NumberField == null)
            {
                return new ExpUpdate(removeLength: 0, updateString: string.Empty);
            }
            int length = NumberField.Number.Value.ToString().Length;
            NumberField = new NumberField();

            return new ExpUpdate(removeLength: length, updateString: "0");
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <returns>ExpUpdate</returns>
        public ExpUpdate BackSpace()
        {
            int RemoveLength = 0;
            if (NumberField == null)
            {
                return new ExpUpdate(removeLength: RemoveLength, updateString: string.Empty);
            }

            RemoveLength = NumberField.Number.Value.ToString().Length;
            if (NumberField.IsEndWithPoint())
            {
                RemoveLength += 1;
            }
            NumberField.ClearOneDigit();
            string updateString = NumberField.Number.ToString();
            if (NumberField.IsEndWithPoint())
            {
                updateString += ".";
            }
            return new ExpUpdate(RemoveLength, updateString);
        }
    }
}