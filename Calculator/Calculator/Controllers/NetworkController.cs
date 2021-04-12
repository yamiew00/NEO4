using Calculator.Objects;
using Calculator.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Calculator.Setting;
using Calculator.Extensions;

namespace Calculator.Controllers
{
    /// <summary>
    /// 網路控制
    /// </summary>
    public class NetworkController
    {
        /// <summary>
        /// 唯一實體
        /// </summary>
        private static NetworkController Instance;

        /// <summary>
        /// Json字樣
        /// </summary>
        private const string JSON_MEDIA_TYPE = "application/json";

        /// <summary>
        /// 初始路徑
        /// </summary>
        private readonly string BASE_URI_PATH;

        /// <summary>
        /// HttpClient實體
        /// </summary>
        private HttpClient Client;

        /// <summary>
        /// 取得唯一實體
        /// </summary>
        /// <returns>實體</returns>
        public static NetworkController GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NetworkController();
                return Instance;
            }
            return Instance;
        }
        
        /// <summary>
        /// 建構子
        /// </summary>
        private NetworkController()
        {
            Client = new HttpClient();
            BASE_URI_PATH = "http://localhost:" + Global.PORT;
        }
        
        /// <summary>
        /// 產生完整網址
        /// </summary>
        /// <param name="str">延伸路徑</param>
        /// <returns>完整網址</returns>
        private string Path(string str)
        {
            return BASE_URI_PATH + str;
        }

        /// <summary>
        /// 按下運算符後的api，向後端送出當前輸入結果。
        /// </summary>
        /// <param name="expression">當前運算式</param>
        public async void OperatorRequest(OperatorExpression expression)
        {
            string jsonString = expression.ToJson();
            
            var uri = Path("/api/postwithbody/" + Global.USER_ID);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(jsonString, Encoding.UTF8, JSON_MEDIA_TYPE)
            };

            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //印出訊息
            var msg = await response.Content.ReadAsStringAsync();
        }
        
        /// <summary>
        /// 等號API
        /// </summary>
        /// <param name="equalExpression">等號表達式</param>
        /// <param name="frameObject">畫面處理</param>
        /// <returns>等待回傳結果</returns>
        public async Task EqualRequest(EqualExpression equalExpression, FrameObject frameObject)
        {
            string jsonString = equalExpression.ToJson();

            var uri = Path("/api/getanswer/" + Global.USER_ID);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            if (decimal.TryParse(result, out decimal d))
            {
                frameObject.SubPanelString = result;
            }
            else
            {
                frameObject.SubPanelString = "運算式錯誤";
            }
        }

        /// <summary>
        /// 清除API
        /// </summary>
        public async void ClearRequest()
        {
            var uri = Path("/api/clear/" + Global.USER_ID);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
