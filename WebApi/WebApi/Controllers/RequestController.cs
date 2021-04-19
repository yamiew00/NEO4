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
        private T SetStatusCode<T>(Func<T> func) where T: StatusMessage, new()
        {
            try
            {
                //沒有丟出exception,正常使用的case則直接回傳
                return func.Invoke();
            }
            catch (Exception exception)
            {
                if (exception is OrderException)
                {
                    T response = new T();
                    response.SetStatus(205);
                    return response;
                }
                else
                {
                    T response = new T();
                    response.SetStatus(400);
                    return response;
                }
            }
        }

        public IHttpActionResult GetInit(int userIdForInit)
        {
            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdForInit);
            commandCaster.Init();

            return Ok(JObject.FromObject(new
            {
                Status = new
                {
                    Code = 204
                }
            }));
        }

        //以下新的
        public IHttpActionResult PostNumber(int userIdWithNumber)
        {
            //讀取body
            char number = (Request.Content.ReadAsAsync<NumberRequest>().Result).Number;

            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdWithNumber);

            NumberResponse response = SetStatusCode<NumberResponse>(() => 
            {
                var successResponse = commandCaster.AddNumber(number);
                if (successResponse.Status == null)
                {
                    successResponse.SetStatus(200);
                }
                return successResponse;
            });

            return Ok(response.ToJson<NumberResponse>());
        }

        public IHttpActionResult PostBinary(int userIdWithBinary)
        {
            //讀取body
            char binary = (Request.Content.ReadAsAsync<BinaryRequest>().Result).BinaryName;

            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdWithBinary);

            BinaryResponse response = SetStatusCode<BinaryResponse>(() => 
            {
                var successResponse = commandCaster.AddBinary(binary);
                successResponse.SetStatus(200);
                return successResponse;
            });

            return Ok(response.ToJson<BinaryResponse>());
        }

        public IHttpActionResult GetEqual(int userIdWithEqual)
        {
            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdWithEqual);


            EqualResponse response = SetStatusCode<EqualResponse>(() =>
            {
                var successResponse = commandCaster.Equal();
                successResponse.SetStatus(200);
                return successResponse;
            });

            return Ok(response.ToJson<EqualResponse>());
        }

        public IHttpActionResult GetLeftBracket(int userIdWithLeftBracket)
        {
            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdWithLeftBracket);

            LeftBracketResponse response = SetStatusCode<LeftBracketResponse>(() => 
            {
                commandCaster.LeftBracket();
                var successResponse = new LeftBracketResponse(new Updates(removeLength: 0, updateString: "("));
                successResponse.SetStatus(200);
                return successResponse;
            });

            return Ok(response.ToJson<LeftBracketResponse>());
        }

        public IHttpActionResult GetRightBracket(int userIdWithRightBracket)
        {
            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdWithRightBracket);

            RightBracketResponse response = SetStatusCode<RightBracketResponse>(() => 
            {
                commandCaster.RightBracket();
                var successResponse = new RightBracketResponse(new Updates(removeLength: 0, updateString: ")"));
                successResponse.SetStatus(200);
                return successResponse;
            });
            return Ok(response.ToJson<RightBracketResponse>());
        }

        public IHttpActionResult GetClear(int userIdWithClear)
        {
            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdWithClear);

            ClearResponse response = SetStatusCode<ClearResponse>(() => 
            {
                commandCaster.Clear();
                var successResponse = new ClearResponse();
                successResponse.SetStatus(204);
                return successResponse;
            });
            return Ok(response.ToJson<ClearResponse>());
        }

        //可以再多加一個updateString = 0
        public IHttpActionResult GetClearError(int userIdWithClearError)
        {
            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdWithClearError);

            ClearErrorResponse response = SetStatusCode<ClearErrorResponse>(() => 
            {
                var successResponse = commandCaster.ClearError();
                successResponse.SetStatus(200);
                return successResponse;
            });
            return Ok(response.ToJson<ClearErrorResponse>());
        }

        public IHttpActionResult GetBackSpace(int userIdWithBackSpace)
        {
            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdWithBackSpace);

            BackSpaceResponse response = SetStatusCode<BackSpaceResponse>(() => 
            {
                var successResponse = commandCaster.BackSpace();
                successResponse.SetStatus(200);
                return successResponse;
            });
            return Ok(response);
        }

        public IHttpActionResult PostUnary(int userIdWithUnary)
        {
            char unary = (Request.Content.ReadAsAsync<UnaryRequest>().Result).UnaryName;

            //拿到該用戶的caster
            CommandCaster commandCaster = Users.GetCommandCaster(userIdWithUnary);

            UnaryResponse response = SetStatusCode<UnaryResponse>(() => 
            {
                var successResponse = commandCaster.AddUnary(unary);
                successResponse.SetStatus(200);
                return successResponse;
            });
            return Ok(response.ToJson<UnaryResponse>());
        }

    }
}
