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
using WebApi.Extensions;
using WebApi.Models.Response;
using WebApi.Exceptions;

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
        //public IHttpActionResult PostInputOperator(int userId)
        //{
        //    //讀取body
        //    OperatorExpression expression = Request.Content.ReadAsAsync<OperatorExpression>().Result;
        //    ExpressionController expController = Users.GetExpressionController(userId);
            
        //    char BinaryName = expression.BinaryOperator.Value;
            
        //    //引入字典
        //    //BinaryOperator binaryOperator = Operators.BinaryDic[BinaryName];
        //    BinaryOperator binaryOperator = Operators.GetBinary(BinaryName);
        //    List<UnaryOperator> unaryList = (expression.UnaryList == null) ? new List<UnaryOperator>() 
        //                                                                   : expression.UnaryList.Select(x => Operators.GetUnary(x)).ToList();
            
        //    //處理四種case
        //    switch (expression.Type())
        //    {
        //        case ExpType.OP:
        //            expController.Modify(binaryOperator);
        //            break;
        //        case ExpType.NUM_OP:
        //            expController.Add(expression.Number.Value, unaryList);
        //            expController.Add(binaryOperator);
        //            break;
        //        case ExpType.LB_NUM_OP:
        //            expController.LeftBracket();
        //            expController.Add(expression.Number.Value, unaryList);
        //            expController.Add(binaryOperator);
        //            break;
        //        case ExpType.NUM_RB_OP:
        //            expController.Add(expression.Number.Value, unaryList);
        //            expController.RightBracket();
        //            expController.Add(binaryOperator);
        //            break;
        //        case ExpType.LB_NUM_RB_OP:
        //            expController.Add(expression.Number.Value, unaryList);
        //            expController.Add(binaryOperator);
        //            break;
        //        default:
        //            throw new Exception("運算表達格式錯誤");
        //    }
            
        //    return Ok(new { msg = "success"});   
        //}

        ///// <summary>
        ///// 取得運算結果。body可能會有數字(int)、右括號(bool)、單元運算列(List(char))
        ///// </summary>
        ///// <param name="userIdForAns">用戶ID</param>
        ///// <returns>運算結果</returns>
        //public IHttpActionResult PostAnswer(int userIdForAns)
        //{
        //    //讀取body
        //    EqualExpression equalexpression = Request.Content.ReadAsAsync<EqualExpression>().Result;
        //    ExpressionController expController = Users.GetExpressionController(userIdForAns);

        //    List<UnaryOperator> unaryList = (equalexpression.UnaryList == null) ?
        //        new List<UnaryOperator>()
        //        : equalexpression.UnaryList.Select(x => Operators.GetUnary(x)).ToList();

        //    decimal? number = equalexpression.Number;
        //    try
        //    {
        //        switch (equalexpression.Type())
        //        {
        //            case EqualType.NUM_EQUAL:
        //                expController.Add(number.Value, unaryList);
        //                break;
        //            case EqualType.NUM_RB_EQUAL:
        //                expController.Add(number.Value, unaryList);
        //                expController.RightBracket();
        //                break;
        //            case EqualType.LB_NUM_RB_EQUAL:
        //                expController.Add(number.Value, unaryList);
        //                break;
        //            default:
        //                throw new Exception("EqualType錯誤");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        expController.Clear();
        //        return Ok(ex.Message);
        //    }
        //    return Ok(expController.GetResult());
        //}



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

            
        public IHttpActionResult GetInit(int userIdForInit)
        {
            ComboController.Instance.Init();
            var jObject = JObject.FromObject(new
            {
                Status = new
                {
                    Code = 204
                }
            });
            return Ok(jObject);
        }

        //以下新的
        public IHttpActionResult PostNumber(int userIdWithNumber)
        {
            //讀取body
            char number = (Request.Content.ReadAsAsync<NumberRequest>().Result).Number;

            NumberResponse response;

            try
            {
                response = ComboController.Instance.AddNumber(number);
                response.SetStatus(200);
            }
            catch(Exception exception)
            {
                if (exception is OrderException)
                {
                    response = new NumberResponse();
                    response.SetStatus(205);
                }
                else
                {
                    response = new NumberResponse();
                    response.SetStatus(400);
                }
            }

            return Ok(response.ToJson<NumberResponse>());
        }

        public IHttpActionResult PostBinary(int userIdWithBinary)
        {
            //讀取body
            char binary = (Request.Content.ReadAsAsync<BinaryRequest>().Result).BinaryName;

            //
            BinaryResponse response;
            try
            {
                response = ComboController.Instance.AddBinary(binary);
                response.SetStatus(200);
            }
            catch (Exception exception)
            {
                if (exception is OrderException)
                {
                    response = new BinaryResponse();
                    response.SetStatus(205);
                }
                else
                {
                    response = new BinaryResponse();
                    response.SetStatus(400);
                }
            }

            return Ok(response.ToJson<BinaryResponse>());
        }

        public IHttpActionResult GetEqual(int userIdWithEqual)
        {
            //var result = Newcontroller.Instance.Equal();
            EqualResponse response;
            try
            {
                response = ComboController.Instance.Equal();
                response.SetStatus(200);
            }
            catch(Exception exception)
            {
                if (exception is OrderException)
                {
                    response = new EqualResponse();
                    response.SetStatus(205);
                }
                else
                {
                    response = new EqualResponse();
                    response.SetStatus(400);
                }
            }

            return Ok(response.ToJson<EqualResponse>());
        }

        public IHttpActionResult GetLeftBracket(int userIdWithLeftBracket)
        {
            LeftBracketResponse response;
            try
            {
                ComboController.Instance.LeftBracket();
                response = new LeftBracketResponse(new Updates(removeLength: 0, updateString: "("));
                response.SetStatus(200);
                
            }
            catch (Exception exception)
            {
                if (exception is OrderException)
                {
                    response = new LeftBracketResponse();
                    response.SetStatus(205);
                }
                else
                {
                    response = new LeftBracketResponse();
                    response.SetStatus(400);
                }
            }
            return Ok(response.ToJson<LeftBracketResponse>());
        }

        public IHttpActionResult GetRightBracket(int userIdWithRightBracket)
        {
            //Newcontroller.Instance.RightBracket();
            RightBracketResponse response;
            try
            {
                ComboController.Instance.RightBracket();
                response = new RightBracketResponse(new Updates(removeLength: 0, updateString: ")"));
                response.SetStatus(200);
            }
            catch(Exception exception)
            {
                if (exception is OrderException)
                {
                    response = new RightBracketResponse();
                    response.SetStatus(205);
                }
                else
                {
                    response = new RightBracketResponse();
                    response.SetStatus(400);
                }       
            }
            return Ok(response.ToJson<RightBracketResponse>());
        }

        public IHttpActionResult GetClear(int userIdWithClear)
        {
            ClearResponse response;
            try
            {
                ComboController.Instance.Clear();
                response = new ClearResponse(code:204);
            }
            catch(Exception exception)
            {
                if (exception is OrderException)
                {
                    response = new ClearResponse(code: 205);
                }
                else
                {
                    response = new ClearResponse(code: 400);
                }
            }
            return Ok(response.ToJson<ClearResponse>());
        }

        //可以再多加一個updateString = 0
        public IHttpActionResult GetClearError(int userIdWithClearError)
        {
            ClearErrorResponse response;
            try
            {
                response = ComboController.Instance.ClearError();
                response.SetStatus(200);
            }
            catch(Exception exception)
            {
                if (exception is OrderException)
                {
                    response = new ClearErrorResponse();
                    response.SetStatus(205);
                }
                else
                {
                    response = new ClearErrorResponse();
                    response.SetStatus(400);
                }
            }
            return Ok(response.ToJson<ClearErrorResponse>());
        }

        public IHttpActionResult GetBackSpace(int userIdWithBackSpace)
        {
            BackSpaceResponse response;
            try
            {
                response = ComboController.Instance.BackSpace();
                response.SetStatus(200);
            }
            catch(Exception exception)
            {
                if (exception is OrderException)
                {
                    response = new BackSpaceResponse();
                    response.SetStatus(205);
                }
                else
                {
                    response = new BackSpaceResponse();
                    response.SetStatus(400);
                }
            }
            return Ok(response);
        }

        public IHttpActionResult PostUnary(int userIdWithUnary)
        {
            char unary = (Request.Content.ReadAsAsync<UnaryRequest>().Result).UnaryName;
            UnaryResponse response;
            try
            {
                response = ComboController.Instance.AddUnary(unary);
                response.SetStatus(200);
            }
            catch (Exception exception)
            {
                if (exception is OrderException)
                {
                    response = new UnaryResponse();
                    response.SetStatus(205);
                }
                else
                {
                    response = new UnaryResponse();
                    response.SetStatus(400);
                }
            }
            return Ok(response.ToJson<UnaryResponse>());
        }

    }
}
