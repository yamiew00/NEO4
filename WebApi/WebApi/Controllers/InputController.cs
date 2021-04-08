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
        private decimal a;
        public InputController()
        {
            ExpressionController ec = new ExpressionController();
            ec.Add(1);
            ec.Add(new BinaryOperator(1, '+', (num1, num2) => num1 + num2));
            ec.Add(1);
            var ans = ec.GetResult();
            System.Diagnostics.Debug.WriteLine($"ans = {ans}");
            


        }

        //測試用
        public IHttpActionResult GettestApi(int id)
        {
            Console.WriteLine(123);

            object obj = new { 答案 = id };
            return Ok(a);
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

            char OperatorName = expression.Operator ?? ' ';
            BinaryOperator Operator = Operators.Dic[OperatorName];
            decimal number = expression.Number ?? 0;

            //處理三種case
            switch (expression.Type())
            {
                case ExpType.NUM_OP:
                    expIC.Add(number);
                    expIC.Add(Operator);
                    break;
                case ExpType.LB_NUM_OP:
                    expIC.LeftBracket();
                    expIC.Add(number);
                    expIC.Add(Operator);
                    break;
                case ExpType.NUM_RB_OP:
                    expIC.Add(number);
                    expIC.RightBracket();
                    expIC.Add(Operator);
                    break;
                default:
                    throw new Exception("ExpressionType錯誤");
            }



            //if (expression.IsNumber())
            //{
            //    expIC.Add((decimal)expression.number);
            //}
            //else if (expression.IsOperator())
            //{
            //    expIC.Add(Operators.Dic[expression.Operator ?? 'A']);   //必不為空，A是為了讓編譯器當過
            //}
            
            return Ok(new { msg = "success"});   
        }

        public IHttpActionResult PostAnswer(int userIdForAns)
        {
            //讀取body
            EqualExpression equalexpression = Request.Content.ReadAsAsync<EqualExpression>().Result;
            ExpressionController expIC = Users.Dic[userIdForAns];

            decimal number = equalexpression.Number ?? 0;
            decimal ans = 0;
            try
            {
                switch (equalexpression.Type())
                {
                    case EqualType.NUM_EQUAL:
                        expIC.Add(number);
                        ans = expIC.GetResult();
                        break;
                    case EqualType.NUM_RB_EQUAL:
                        expIC.Add(number);
                        expIC.RightBracket();
                        ans = expIC.GetResult();
                        break;
                    default:
                        throw new Exception("EqualType錯誤");
                }
            }catch(Exception ex)
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
