using Calculator.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Calculator.Setting;
using Calculator.Extensions;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

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
        /// 私有建構子
        /// </summary>
        private NetworkController()
        {
            Client = new HttpClient();
            BASE_URI_PATH = "http://localhost:" + Global.PORT;
        }

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
        /// <param name="expression">運算表達式</param>
        /// <param name="frameObject">畫面相關物件</param>
        /// <returns></returns>
        public async Task OperatorRequest(OperatorExpression expression, FrameObject frameObject)
        {
            string jsonString = expression.ToJson<OperatorExpression>();

            var uri = Path("/api/postwithbody/" + Global.USER_ID);
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(jsonString, Encoding.UTF8, JSON_MEDIA_TYPE)
            };

            HttpResponseMessage response;
            try
            {
                response = await Client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                //接住response
                var msg = await response.Content.ReadAsStringAsync();

            }
            catch (HttpRequestException)
            {
                frameObject.SubPanelString = "運算式錯誤";
                frameObject.PanelString = string.Empty;
                ClearRequest();
            }
        }
        
        /// <summary>
        /// 等號API
        /// </summary>
        /// <param name="equalExpression">等號表達式</param>
        /// <param name="frameObject">畫面處理</param>
        /// <returns>等待回傳結果</returns>
        public async Task EqualRequest(EqualExpression equalExpression, FrameObject frameObject)
        {
            string jsonString = equalExpression.ToJson<EqualExpression>();

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
                Console.WriteLine($"errorMessage = {result}");
            }
        }

        /// <summary>
        /// 清除的API
        /// </summary>
        public async void ClearRequest()
        {
            var uri = Path("/api/clear/" + Global.USER_ID);
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
            //連線不應該由這裡處理
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
