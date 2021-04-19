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
        /// 用戶ID
        /// </summary>
        public static readonly int USER_ID = int.Parse(ConfigurationManager.AppSettings["UserId"]);

        /// <summary>
        /// 連接埠號
        /// </summary>
        public static readonly string PORT = ConfigurationManager.AppSettings["Port"];

        /// <summary>
        /// 更新用戶Id
        /// </summary>
        public static void UpdateUserId()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var updatedId = Global.USER_ID + 1;

            if (USER_ID == int.MaxValue)
            {
                updatedId = -1 * int.MaxValue;
            }

            configuration.AppSettings.Settings["UserId"].Value = updatedId.ToString();
            configuration.Save();
        }
    }
}
