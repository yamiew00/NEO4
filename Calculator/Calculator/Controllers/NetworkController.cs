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
    /// <summary>
    /// 網路控制
    /// </summary>
    public class NetworkController
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static NetworkController _instance = new NetworkController();

        /// <summary>
        /// 實體的get方法
        /// </summary>
        public static NetworkController Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// HttpClient實體
        /// </summary>
        private HttpClient Client;

        /// <summary>
        /// 基本網址
        /// </summary>
        private string BASE_URI_PATH;

        /// <summary>
        /// Json字樣
        /// </summary>
        private const string JSON_MEDIA_TYPE = "application/json";

        /// <summary>
        /// 私有建構子
        /// </summary>
        private NetworkController()
        {
            Client = new HttpClient();
            BASE_URI_PATH = "http://localhost:" + Global.PORT;
        }

        /// <summary>
        /// Uri製作
        /// </summary>
        /// <param name="str">新增的路徑後半段</param>
        /// <returns>完整網址</returns>
        private string Path(string str)
        {
            return BASE_URI_PATH + str;
        }

        /// <summary>
        /// 根據狀態碼決定更新內容
        /// </summary>
        /// <param name="update">更新內容</param>
        /// <param name="status">狀態碼</param>
        /// <returns>更新的內容</returns>
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

        /// <summary>
        /// 發出數字的請求
        /// </summary>
        /// <param name="number">數字</param>
        /// <returns>更新內容</returns>
        public async Task<Update> NumberRequest(char number)
        {
            var uri = Path($"/api/number/{Global.USER_ID}");
            RequestNumberJson requestNumberJson = new RequestNumberJson(number);
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
            return GetUpdateByCode(msg.Update, msg.Status);
        }

        /// <summary>
        /// 發出雙元運算子的請求
        /// </summary>
        /// <param name="binary">雙元運算子</param>
        /// <returns>更新內容</returns>
        public async Task<Update> BinaryRequest(char binary)
        {
            var uri = Path($"/api/binary/{Global.USER_ID}");
            RequestBinaryJson requestBinaryJson = new RequestBinaryJson(binary);
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

        /// <summary>
        /// 發出等號的請求
        /// </summary>
        /// <returns>更新內容</returns>
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

        /// <summary>
        /// 發出左括號的請求
        /// </summary>
        /// <returns>更新內容</returns>
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

        /// <summary>
        /// 發出右括號的請求
        /// </summary>
        /// <returns>更新內容</returns>
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

        /// <summary>
        /// 發出Clear的請求
        /// </summary>
        /// <returns>更新內容</returns>
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

        /// <summary>
        /// 發出ClearError的請求
        /// </summary>
        /// <returns>更新內容</returns>
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

        /// <summary>
        /// 發出返回鍵的請求
        /// </summary>
        /// <returns>更新內容</returns>
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

        /// <summary>
        /// 發出單元運算子的請求
        /// </summary>
        /// <param name="unary">單元運算子</param>
        /// <returns>更新內容</returns>
        public async Task<Update> UnaryRequest(char unary)
        {
            var uri = Path($"/api/unary/{Global.USER_ID}");
            RequestUnaryJson requestUnaryJson = new RequestUnaryJson(unary);
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

        /// <summary>
        /// 連線初始化
        /// </summary>
        /// <returns>無回傳值</returns>
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
