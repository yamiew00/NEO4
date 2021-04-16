using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Objects;
using WebApi.Models;
using WebApi.DataBase;
using WebApi.Setting;
using WebApi.NewThing;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{
    /// <summary>
    /// 輸入控制
    /// </summary>
    public class RequestController : ApiController
    {
        /// <summary>
        /// 輸入用戶id。body可能會有數字(int)、運算符(char)、左括號(bool)、右括號(bool)、單元運算列(List(char))
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns>Response</returns>
        public IHttpActionResult PostInputOperator(int userId)
        {
            //讀取body
            OperatorExpression expression = Request.Content.ReadAsAsync<OperatorExpression>().Result;
            ExpressionController expController = Users.GetExpressionController(userId);
            
            char BinaryName = expression.BinaryOperator.Value;
            
            //引入字典
            //BinaryOperator binaryOperator = Operators.BinaryDic[BinaryName];
            BinaryOperator binaryOperator = Operators.GetBinary(BinaryName);
            List<UnaryOperator> unaryList = (expression.UnaryList == null) ? new List<UnaryOperator>() 
                                                                           : expression.UnaryList.Select(x => Operators.GetUnary(x)).ToList();
            
            //處理四種case
            switch (expression.Type())
            {
                case ExpType.OP:
                    expController.Modify(binaryOperator);
                    break;
                case ExpType.NUM_OP:
                    expController.Add(expression.Number.Value, unaryList);
                    expController.Add(binaryOperator);
                    break;
                case ExpType.LB_NUM_OP:
                    expController.LeftBracket();
                    expController.Add(expression.Number.Value, unaryList);
                    expController.Add(binaryOperator);
                    break;
                case ExpType.NUM_RB_OP:
                    expController.Add(expression.Number.Value, unaryList);
                    expController.RightBracket();
                    expController.Add(binaryOperator);
                    break;
                case ExpType.LB_NUM_RB_OP:
                    expController.Add(expression.Number.Value, unaryList);
                    expController.Add(binaryOperator);
                    break;
                default:
                    throw new Exception("運算表達格式錯誤");
            }
            
            return Ok(new { msg = "success"});   
        }

        /// <summary>
        /// 取得運算結果。body可能會有數字(int)、右括號(bool)、單元運算列(List(char))
        /// </summary>
        /// <param name="userIdForAns">用戶ID</param>
        /// <returns>運算結果</returns>
        public IHttpActionResult PostAnswer(int userIdForAns)
        {
            //讀取body
            EqualExpression equalexpression = Request.Content.ReadAsAsync<EqualExpression>().Result;
            ExpressionController expController = Users.GetExpressionController(userIdForAns);

            List<UnaryOperator> unaryList = (equalexpression.UnaryList == null) ?
                new List<UnaryOperator>()
                : equalexpression.UnaryList.Select(x => Operators.GetUnary(x)).ToList();

            decimal? number = equalexpression.Number;
            try
            {
                switch (equalexpression.Type())
                {
                    case EqualType.NUM_EQUAL:
                        expController.Add(number.Value, unaryList);
                        break;
                    case EqualType.NUM_RB_EQUAL:
                        expController.Add(number.Value, unaryList);
                        expController.RightBracket();
                        break;
                    case EqualType.LB_NUM_RB_EQUAL:
                        expController.Add(number.Value, unaryList);
                        break;
                    default:
                        throw new Exception("EqualType錯誤");
                }
            }
            catch (Exception ex)
            {
                expController.Clear();
                return Ok(ex.Message);
            }
            return Ok(expController.GetResult());
        }



        /// <summary>
        /// Clear事件
        /// </summary>
        /// <param name="userIdForClear">用戶ID</param>
        /// <returns>Response</returns>
        //public IHttpActionResult GetClear(int userIdForClear)
        //{
        //    Users.GetExpressionController(userIdForClear).Clear(); 
        //    return Ok("success");
        //}










        //以下新的
        public IHttpActionResult PostNumber(int userIdWithNumber)
        {
            //讀取body
            char number = (Request.Content.ReadAsAsync<NumberJson>().Result).Number;
            //var updateString = Newcontroller.Instance.AddNumber(number);
            var updateString = ComboController.Instance.Number(number);

            JObject jObject = JObject.FromObject(new
            {
                update = new
                {
                    updateString
                }
            });
            return Ok(jObject);
        }

        public IHttpActionResult PostBinary(int userIdWithBinary)
        {
            //讀取body
            char binary = (Request.Content.ReadAsAsync<BinaryJson>().Result).BinaryName;
            //var result = Newcontroller.Instance.AddBinary(binary);
            var result = ComboController.Instance.AddBinary(binary);

            JObject jObject = JObject.FromObject(new
            {
                update = result
            });

            return Ok(jObject);
        }

        public IHttpActionResult GetEqual(int userIdWithEqual)
        {
            //var result = Newcontroller.Instance.Equal();
            var result = ComboController.Instance.Equal();

            JObject jObject = JObject.FromObject(new
            {
                update = new
                {
                    updateString = "=",
                    answer = result
                }
            });
            return Ok(jObject);
        }

        public IHttpActionResult GetLeftBracket(int userIdWithLeftBracket)
        {
            //Newcontroller.Instance.LeftBracket();
            ComboController.Instance.LeftBracket();

            JObject jObject = JObject.FromObject(new
            {
                update = new
                {
                    updateString = "("
                }
            });

            return Ok(jObject);
        }

        public IHttpActionResult GetRightBracket(int userIdWithRightBracket)
        {
            //Newcontroller.Instance.RightBracket();
            ComboController.Instance.RightBracket();
            JObject jObject = JObject.FromObject(new
            {
                update = new
                {
                    updateString = ")"
                }
            });

            return Ok(jObject);
        }

        public IHttpActionResult GetClear(int userIdWithClear)
        {
            //Newcontroller.Instance.Clear();
            ComboController.Instance.Clear();
            JObject jObject = JObject.FromObject(new
            {
                message = "success"
            });

            return Ok(jObject);
        }

        //可以再多加一個updateString = 0
        public IHttpActionResult GetClearError(int userIdWithClearError)
        {
            //var RemoveLength = Newcontroller.Instance.ClearError();
            var RemoveLength = ComboController.Instance.ClearError();

            JObject jObject = JObject.FromObject(new
            {
                update = new
                {
                    RemoveLength
                }
            });

            return Ok(jObject);
        }

        public IHttpActionResult GetBackSpace(int userIdWithBackSpace)
        {
            //var backSpaceJson = Newcontroller.Instance.BackSpace();
            var backSpaceJson = ComboController.Instance.BackSpace();

            JObject jObject = JObject.FromObject(new
            {
                update = backSpaceJson
            });

            return Ok(jObject);
        }

        public IHttpActionResult PostUnary(int userIdWithUnary)
        {
            char unary = (Request.Content.ReadAsAsync<UnaryJson>().Result).UnaryName;
            //var result = Newcontroller.Instance.AddUnary(unary);
            var result = ComboController.Instance.AddUnary(unary);

            JObject jObject = JObject.FromObject(new
            {
                Update = result
            });

            return Ok(jObject);
        }

    }
}
