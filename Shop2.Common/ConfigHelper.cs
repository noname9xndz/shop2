using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Common
{
    public class ConfigHelper
    {
        // lấy ra key ở app setting
        public static string GetByKey(string key)
        {// nhớ add references System.Configuration;
            return ConfigurationManager.AppSettings[key].ToString();
        }
    }
}
