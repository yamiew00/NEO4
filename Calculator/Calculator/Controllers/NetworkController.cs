using Calculator.Setting;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Calculator.Extensions;
using Calculator.Networks.Request;
using Calculator.Networks;
using Calculator.Networks.Response;

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
        /// 網路請求
        /// </summary>
        /// <param name="bond">功能組合(Feature, Content)</param>
        /// <returns>Response</returns>
        public async Task<Response> Request(Bond bond)
        {
            var uri = Path("/api/integrated/" + Global.USER_ID);
            string jsonString = bond.ToJson<Bond>();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(jsonString, Encoding.UTF8, JSON_MEDIA_TYPE)
            };

            var result = await Client.SendAsync(request);
            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadAsAsync<Response>();

            return response;
        }
    }
}
