using Calculator.Objects;
using Calculator.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Controllers
{
    public class NetworkController
    {
        private static NetworkController Instance;
        private const string OPERATOR_STRING = "BinaryOperator";
        private const string NUMBER_STRING = "number";
        private const string LEFTBRACKET_STRING = "LeftBracket";
        private const string RIGHTBRACKET_STRING = "RightBracket";
        private const string UNARYLIST_STRING = "UnaryList";

        private const string PORT = "53104";

        //private const string uri = "http://localhost:53104/api/postwithbody/1";
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
        }

        HttpClient Client;

        public async void OperatorRequest(Expression expression)
        {
            JsonBuilder jsonBuilder = new JsonBuilder();

            Console.WriteLine($"BinaryOperatorMark is null = {expression.BinaryOperatorMark == null}");
            Console.WriteLine($"Number is null = {expression.Number == null}");
            Console.WriteLine($"UnaryList is null = {expression.UnaryList == null}");

            //string sample = "{\"Operator\" : \"x\",\"number\" : 3,\"LeftBracket\" : false,\"RightBracket\" : false}";
            string jsonString = jsonBuilder.SetStringValue(OPERATOR_STRING, expression.BinaryOperatorMark)
                                           .SetObjectValue(NUMBER_STRING, expression.Number)
                                           .SetObjectValue(LEFTBRACKET_STRING, expression.LeftBracket)
                                           .SetObjectValue(RIGHTBRACKET_STRING, expression.RightBracket)
                                           .SetStringListValue(UNARYLIST_STRING, expression.UnaryList)
                                           .ToString();

            

            var uri = "http://localhost:" + PORT + "/api/postwithbody/1";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            //Debug區
            var msg = await response.Content.ReadAsStringAsync();
            Console.WriteLine(msg);
        }

        public async void EqualRequest(EqualExpression equalExpression, Form1 form)
        {
            JsonBuilder jsonBuilder = new JsonBuilder();
            string jsonString = jsonBuilder.SetObjectValue(NUMBER_STRING, equalExpression.Number)
                                           .SetObjectValue(RIGHTBRACKET_STRING, equalExpression.RightBracket)
                                           .SetStringListValue(UNARYLIST_STRING, equalExpression.UnaryList)
                                           .ToString();

            var uri = "http://localhost:" + PORT + "/api/getanswer/1";

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
                form.SubPanelShow(result);
            }
            else
            {
                form.SubPanelShow("運算式錯誤");
            }
            
        }

        //新的
        public async Task EqualRequest(EqualExpression equalExpression, FrameObject frameObject)
        {
            JsonBuilder jsonBuilder = new JsonBuilder();
            string jsonString = jsonBuilder.SetObjectValue(NUMBER_STRING, equalExpression.Number)
                                           .SetObjectValue(RIGHTBRACKET_STRING, equalExpression.RightBracket)
                                           .SetStringListValue(UNARYLIST_STRING, equalExpression.UnaryList)
                                           .ToString();

            var uri = "http://localhost:" + PORT + "/api/getanswer/1";

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
            var uri = "http://localhost:" + PORT + "/api/clear/1";
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
