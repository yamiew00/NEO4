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
            ExpressionController expIC = Users.GetExpressionController(userId);
            
            char BinaryName = expression.BinaryOperator ?? ' ';
            
            //引入字典
            //BinaryOperator binaryOperator = Operators.BinaryDic[BinaryName];
            BinaryOperator binaryOperator = Operators.GetBinary(BinaryName);
            List<UnaryOperator> unaryList = (expression.UnaryList == null) ? 
                new List<UnaryOperator>() 
                : expression.UnaryList.Select(x => Operators.GetUnary(x)).ToList();
            
            decimal number = expression.Number ?? 0;

            //處理四種case
            switch (expression.Type())
            {
                case ExpType.OP:
                    expIC.Modify(binaryOperator);
                    break;
                case ExpType.NUM_OP:
                    expIC.Add(number, unaryList);
                    expIC.Add(binaryOperator);
                    break;
                case ExpType.LB_NUM_OP:
                    expIC.LeftBracket();
                    expIC.Add(number, unaryList);
                    expIC.Add(binaryOperator);
                    break;
                case ExpType.NUM_RB_OP:
                    expIC.Add(number, unaryList);
                    expIC.RightBracket();
                    expIC.Add(binaryOperator);
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
            ExpressionController expIC = Users.GetExpressionController(userIdForAns);

            List<UnaryOperator> unaryList = (equalexpression.UnaryList == null) ?
                new List<UnaryOperator>()
                : equalexpression.UnaryList.Select(x => Operators.GetUnary(x)).ToList();

            decimal number = equalexpression.Number ?? 0;
            decimal ans = 0;
            try
            {
                switch (equalexpression.Type())
                {
                    case EqualType.NUM_EQUAL:
                        expIC.Add(number, unaryList);
                        ans = expIC.GetResult();
                        break;
                    case EqualType.NUM_RB_EQUAL:
                        expIC.Add(number, unaryList);
                        expIC.RightBracket();
                        ans = expIC.GetResult();
                        break;
                    default:
                        throw new Exception("EqualType錯誤");
                }
            }
            catch (Exception)
            {
                expIC.Clear();
                System.Diagnostics.Debug.WriteLine("運算式錯誤");
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
