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
    public class InputController : ApiController
    {
        public InputController()
        {

        }
        

        /// <summary>
        /// 直接用這個。輸入用戶id。body應該會有數字(int)或運算符(char)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public IHttpActionResult PostWithBody(int userId)
        {
            //讀取body
            Expression expression = Request.Content.ReadAsAsync<Expression>().Result;
            ExpressionController expIC = Users.GetExpressionController(userId);

            char BinaryName = expression.BinaryOperator ?? ' ';

            BinaryOperator binaryOperator = Operators.BinaryDic[BinaryName];
            List<UnaryOperator> unaryList = (expression.UnaryList == null) ? 
                new List<UnaryOperator>() 
                : expression.UnaryList.Select(x => Operators.UnaryDic[x]).ToList();
            
            decimal number = expression.Number ?? 0;

            //處理三種case
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
                    throw new Exception("ExpressionType錯誤");
            }
            
            return Ok(new { msg = "success"});   
        }

        public IHttpActionResult PostAnswer(int userIdForAns)
        {
            //讀取body
            EqualExpression equalexpression = Request.Content.ReadAsAsync<EqualExpression>().Result;
            ExpressionController expIC = Users.Dic[userIdForAns];

            List<UnaryOperator> unaryList = (equalexpression.UnaryList == null) ?
                new List<UnaryOperator>()
                : equalexpression.UnaryList.Select(x => Operators.UnaryDic[x]).ToList();

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
            }catch(Exception)
            {
                expIC.Clear();
                System.Diagnostics.Debug.WriteLine("運算式錯誤");
                return Ok("fail");
            }

            return Ok(ans);
        }

        public IHttpActionResult GetClear(int userIdForClear)
        {
            if (Users.Dic.Keys.Contains(userIdForClear))
            {
                ExpressionController expIC = Users.Dic[userIdForClear];
                expIC.Clear();
            }
            
            return Ok("success");
        }
        
    }
}
