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
    public class NetworkController
    {
        private static NetworkController Instance;

        private const string JSON_MEDIA_TYPE = "application/json";

        private string BASE_URI_PATH;

        HttpClient Client;

        public static NetworkController GetInstance()
        {
            if (Instance == null)
            {
                Instance = new NetworkController();
                return Instance;
            }
            return Instance;
        }
        
        private NetworkController()
        {
            Client = new HttpClient();
            BASE_URI_PATH = "http://localhost:" + Global.PORT;
        }
        
        private string Path(string str)
        {
            return BASE_URI_PATH + str;
        }

        /// <summary>
        /// 按下運算符後，向後端送出當前輸入結果。
        /// </summary>
        /// <param name="expression">當前運算式</param>
        public async void OperatorRequest(Expression expression)
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
        

        //新的
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
