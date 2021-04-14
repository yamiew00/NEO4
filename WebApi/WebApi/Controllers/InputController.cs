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

namespace WebApi.Controllers
{
    /// <summary>
    /// 輸入控制
    /// </summary>
    public class InputController : ApiController
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
            
            decimal number = expression.Number ?? 0;

            //處理四種case
            switch (expression.Type())
            {
                case ExpType.OP:
                    expController.Modify(binaryOperator);
                    break;
                case ExpType.NUM_OP:
                    expController.Add(number, unaryList);
                    expController.Add(binaryOperator);
                    break;
                case ExpType.LB_NUM_OP:
                    expController.LeftBracket();
                    expController.Add(number, unaryList);
                    expController.Add(binaryOperator);
                    break;
                case ExpType.NUM_RB_OP:
                    expController.Add(number, unaryList);
                    expController.RightBracket();
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
            decimal ans = 0;
            try
            {
                switch (equalexpression.Type())
                {
                    case EqualType.NUM_EQUAL:
                        expController.Add(number.Value, unaryList);
                        ans = expController.GetResult();
                        break;
                    case EqualType.NUM_RB_EQUAL:
                        expController.Add(number.Value, unaryList);
                        expController.RightBracket();
                        ans = expController.GetResult();
                        break;
                    default:
                        throw new Exception("EqualType錯誤");
                }
            }
            catch (Exception)
            {
                expController.Clear();
                return Ok("fail");
            }
            return Ok(ans);
        }

        /// <summary>
        /// Clear事件
        /// </summary>
        /// <param name="userIdForClear">用戶ID</param>
        /// <returns>Response</returns>
        public IHttpActionResult GetClear(int userIdForClear)
        {
            Users.GetExpressionController(userIdForClear).Clear(); 
            return Ok("success");
        }
    }
}
