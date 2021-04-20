namespace Calculator.Extensions
{
    /// <summary>
    /// Json擴充字元
    /// </summary>
    public static class ToJsons
    {
        /// <summary>
        /// 轉換成Json的方法
        /// </summary>
        /// <typeparam name="T">特定類別</typeparam>
        /// <param name="someObject">類別物件</param>
        /// <returns>Json格式的字串</returns>
        public static string ToJson<T>(this T someObject)
        {
            if (someObject == null)
            {
                return string.Empty;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(someObject);
        }
    }
}
