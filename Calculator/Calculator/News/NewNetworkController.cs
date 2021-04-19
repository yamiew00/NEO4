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
using System.Windows.Forms;
using Calculator.Exceptions;

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


        private Update GetUpdateByCode(Update update, Status status)
        {
            if (status.Code == 200)
            {
                return update;
            }
            else if (status.Code == 205)
            {
                return new Update();
            }
            else if (status.Code == 400)
            {
                throw new Exception400("運算錯誤");
            }
            else if (status.Code == 999)
            {
                return new Update(-1, update.UpdateString);
            }
            else
            {
                //未處理
                throw new NotImplementedException();
            }
        }


        //數字
        public async Task<Update> NumberRequest(char text)
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
            var msg = await response.Content.ReadAsAsync<ResponseNumber>();
            //if (msg.Status.Code == 200)
            //{
            //    return msg.Update;
            //}
            //else if (msg.Status.Code == 205)
            //{
            //    return new Update();
            //}
            //else
            //{
            //    //未處理
            //    return null;
            //}
            return GetUpdateByCode(msg.Update, msg.Status);
        }

        public async Task<Update> BinaryRequest(char text)
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
            return GetUpdateByCode(msg.Update, msg.Status);
        }

        public async Task<ResponseEqual> EqualRequest()
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
            var msg = await response.Content.ReadAsAsync<ResponseEqual>();
            if (msg.Status.Code == 200)
            {
                return msg;
            }
            else if (msg.Status.Code == 205)
            {
                return new ResponseEqual(new Update(), answer: null);
            }
            else if (msg.Status.Code == 400)
            {
                throw new Exception400("運算錯誤");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<Update> LeftBracketRequest()
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
            var msg = await response.Content.ReadAsAsync<ResponseLeftBracket>();
            return GetUpdateByCode(msg.Update, msg.Status);
        }

        public async Task<Update> RightBracketRequest()
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
            var msg = await response.Content.ReadAsAsync<ResponseRightBracket>();
            return GetUpdateByCode(msg.Update, msg.Status);
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
            var msg = await response.Content.ReadAsAsync<ResponseClear>();
            return (msg.Status != null) && (msg.Status.Code == 204);
        }

        public async Task<Update> ClearErrorRequest()
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
            var msg = await response.Content.ReadAsAsync<ResponseClearError>();
            return GetUpdateByCode(msg.Update, msg.Status);
        }


        public async Task<Update> BackSpaceRequest()
        {
            var uri = Path($"/api/backspace/{Global.USER_ID}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var msg = await response.Content.ReadAsAsync<ResponseBackSpace>();
            return GetUpdateByCode(msg.Update, msg.Status);
        }


        public async Task<Update> UnaryRequest(char text)
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
            return GetUpdateByCode(msg.Update, msg.Status);
        }


        //連線好像不該擺這
        public async Task InitRequest()
        {
            var uri = Path("/api/init/" + Global.USER_ID);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };
            try
            {
                HttpResponseMessage response = await Client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                if (exception is HttpRequestException)
                {
                    MessageBox.Show("無法連線，請確認網路或伺服器");
                    System.Windows.Forms.Application.Exit();
                    return;
                }
                throw;
            }
        }

    }
}
