using Calculator.News.JsonObject;
using Calculator.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Calculator.Extensions;
using Calculator.News.JsonRequest;
using Calculator.News.JsonResponse;

namespace Calculator.News
{
    public class NewNetworkController
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static NewNetworkController _instance = new NewNetworkController();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static NewNetworkController Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 私有建構子
        /// </summary>
        private NewNetworkController()
        {
            Client = new HttpClient();
            BASE_URI_PATH = "http://localhost:" + Global.PORT;
        }

        private HttpClient Client;
        private string BASE_URI_PATH;
        /// <summary>
        /// Json字樣
        /// </summary>
        private const string JSON_MEDIA_TYPE = "application/json";

        private string Path(string str)
        {
            return BASE_URI_PATH + str;
        }

        //數字
        public async Task<string> NumberRequest(char text)
        {
            var uri = Path($"/api/number/{Global.USER_ID}");
            RequestNumberJson requestNumberJson = new RequestNumberJson(text);
            string jsonString = requestNumberJson.ToJson<RequestNumberJson>();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(jsonString, Encoding.UTF8, JSON_MEDIA_TYPE)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //接住response
            var msg = await response.Content.ReadAsAsync<ResponseNumberJson>();
            var updateString = msg.Update.updateString;
            return updateString;
        }

        public async Task<ResponseBinaryJson> BinaryRequest(char text)
        {
            var uri = Path($"/api/binary/{Global.USER_ID}");
            RequestBinaryJson requestBinaryJson = new RequestBinaryJson(text);
            string jsonString = requestBinaryJson.ToJson<RequestBinaryJson>();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(jsonString, Encoding.UTF8, JSON_MEDIA_TYPE)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //接住response
            var msg = await response.Content.ReadAsAsync<ResponseBinaryJson>();            
            return msg;
        }

        public async Task<ResponseEqualJson> EqualRequest()
        {
            var uri = Path($"/api/equal/{Global.USER_ID}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //接住response
            var msg = await response.Content.ReadAsAsync<ResponseEqualJson>();
            
            return msg;
        }

        public async Task<string> LeftBracketRequest()
        {
            var uri = Path($"/api/LeftBracket/{Global.USER_ID}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //接住response
            var msg = await response.Content.ReadAsAsync<ResponseLeftBracketJson>();
            var updateSring = msg.Update.UpdateString;

            return updateSring;
        }

        public async Task<string> RightBracketRequest()
        {
            var uri = Path($"/api/RightBracket/{Global.USER_ID}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //接住response
            var msg = await response.Content.ReadAsAsync<ResponseRightBracketJson>();
            var updateSring = msg.Update.UpdateString;

            return updateSring;
        }

        public async Task<bool> ClearRequest()
        {
            var uri = Path($"/api/clear/{Global.USER_ID}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //接住response
            var msg = await response.Content.ReadAsAsync<ResponseClearJson>();
            var IsSuccess = msg.message;
            if (IsSuccess.Equals("success"))
            {
                return true;
            }
            return false;
        }

        public async Task<int> ClearErrorRequest()
        {
            var uri = Path($"/api/clearerror/{Global.USER_ID}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //接住response
            var msg = await response.Content.ReadAsAsync<ResponseClearErrorJson>();
            int removeLength = msg.Update.RemoveLength;

            return removeLength;
        }


        public async Task<ResponseBackSpaceJson> BackSpaceRequest()
        {
            var uri = Path($"/api/backspace/{Global.USER_ID}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var msg = await response.Content.ReadAsAsync<ResponseBackSpaceJson>();

            return msg;
        }


        public async Task<ResponseUnaryJson> UnaryRequest(char text)
        {
            var uri = Path($"/api/unary/{Global.USER_ID}");
            RequestUnaryJson requestUnaryJson = new RequestUnaryJson(text);
            string jsonString = requestUnaryJson.ToJson<RequestUnaryJson>();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(jsonString, Encoding.UTF8, JSON_MEDIA_TYPE)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var msg = await response.Content.ReadAsAsync<ResponseUnaryJson>();
            return msg;
        }


    }
}
