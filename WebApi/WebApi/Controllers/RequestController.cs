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
using WebApi.Models.Request;

namespace WebApi.Controllers
{
    /// <summary>
    /// 網路請求處理
    /// </summary>
    public class RequestController : ApiController
    {
        /// <summary>
        /// 狀態碼檢查。若執行的過程中有拋出錯誤，則賦予相對應的狀態碼。
        /// </summary>
        /// <typeparam name="T">要回傳的TResult</typeparam>
        /// <param name="func">正常使用情況下的執行過程</param>
        /// <returns>TResult</returns>
        private T SetStatusCode<T>(Func<T> func) where T : StatusMessage, new()
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

        /// <summary>
        /// 初始化該用戶的功能執行者
        /// </summary>
        /// <param name="userIdForInit">用戶Id</param>
        /// <returns>網路回應</returns>
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

        /// <summary>
        /// Post。使用者輸入數字
        /// </summary>
        /// <param name="userIdWithNumber">用戶Id</param>
        /// <returns>網路回應</returns>
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

        /// <summary>
        /// 使用者輸入雙元運算子
        /// </summary>
        /// <param name="userIdWithBinary">用戶Id</param>
        /// <returns>網路回應</returns>
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

        /// <summary>
        /// 使用者輸入等號
        /// </summary>
        /// <param name="userIdWithEqual">用戶Id</param>
        /// <returns>網路回應</returns>
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

        /// <summary>
        /// 使用者輸入左括號
        /// </summary>
        /// <param name="userIdWithLeftBracket">用戶Id</param>
        /// <returns>網路回應</returns>
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

        /// <summary>
        /// 使用者輸入右括號
        /// </summary>
        /// <param name="userIdWithRightBracket">用戶Id</param>
        /// <returns>網路回應</returns>
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

        /// <summary>
        /// 使用者按下Clear
        /// </summary>
        /// <param name="userIdWithClear">用戶Id</param>
        /// <returns>網路回應</returns>
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

        /// <summary>
        /// 使用者按下ClearError
        /// </summary>
        /// <param name="userIdWithClearError">用戶Id</param>
        /// <returns>網路回應</returns>
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

        /// <summary>
        /// 使用者使用返回鍵
        /// </summary>
        /// <param name="userIdWithBackSpace">用戶Id</param>
        /// <returns>網路回應</returns>
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

        /// <summary>
        /// 使用者輸入單元運算子
        /// </summary>
        /// <param name="userIdWithUnary">用戶Id</param>
        /// <returns>網路回應</returns>
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

        public IHttpActionResult PostIntegrate(int userId)
        {
            Integrated integrated = Request.Content.ReadAsAsync<Integrated>().Result;
            var response = Features.ActionDic[integrated.Feature].Invoke(integrated.Content, userId);
            var json = response.ToJson<FrameResponse>();
            return Ok(json);
            
        }
    }
}
