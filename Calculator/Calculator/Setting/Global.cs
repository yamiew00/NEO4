using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Calculator.Setting
{
    /// <summary>
    /// Global Setting
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// 用戶ID。從app.config找
        /// </summary>
        public static readonly int USER_ID = int.Parse(ConfigurationManager.AppSettings["UserId"]);

        /// <summary>
        /// 連接埠號。從app.config找
        /// </summary>
        public static readonly string PORT = ConfigurationManager.AppSettings["Port"];
    }
}
