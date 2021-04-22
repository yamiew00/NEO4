using System.Net.Http;
using System.Web.Http;
using WebApi.Setting;
using Newtonsoft.Json.Linq;
using WebApi.Extensions;
using WebApi.Models.Response;
using WebApi.Models.Request;

namespace WebApi.Controllers
{
    /// <summary>
    /// 網路請求處理
    /// </summary>
    public class RequestController : ApiController
    {
        /// <summary>
        /// 拿取(Feature, Content)的api接口。回傳Panel與SubPanel的物件
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>FrameResponse物件的JSON: 帶有Panel和SubPanel的字串</returns>
        public IHttpActionResult PostIntegrate(int userId)
        {
            //拿body中的Feature與Content。並存為Instruct物件
            Instruct instruct = Request.Content.ReadAsAsync<Instruct>().Result;

            //向Feature類別索取FrameObject
            FrameObject frameObject = Features.GetFrameObject(userId, instruct);

            //將response包成Json格式
            JObject json = frameObject.ToJson<FrameObject>();
            return Ok(json);
        }
    }
}
