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
        public IHttpActionResult PostIntegrate(int userId)
        {
            Integrated integrated = Request.Content.ReadAsAsync<Integrated>().Result;
            var response = Features.ActionDic[integrated.Feature].Invoke(integrated.Content, userId);
            var json = response.ToJson<FrameResponse>();
            return Ok(json);
        }
    }
}
