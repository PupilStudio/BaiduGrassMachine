using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrassMachine
{
    public static class TranslateHelper
    {
        private static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

        static void Delay(long millSec)
        {
            long time = System.DateTime.UtcNow.Ticks;
            while ((System.DateTime.UtcNow.Ticks - time) / 10000 < millSec)
                Application.DoEvents();

        }

        public const string APP_ID = "20200426000430735";
        public const string APP_KEY = "TF3ufAP2qLR2lBcUR17v";
        public static string Grass(string doc, int times, TextBox view)
        {
            if (times <= 1 || times > 500)
                throw new Exception();

            string cur = doc;
            string from = "zh";

            string[] lang = new string[] {
                "zh", "en", "yue", "wyw", "jp", "kor", "fra", "spa", "th", "ara", "ru", "pt",
                "de", "it", "el", "nl", "pl", "bul", "est", "dan", "fin", "cs", "rom", "slo",
                "swe", "hu", "vie"
            };

            
            var random = new Random();
            
            for (int i = 1; i <= times; ++i)
            {
                BEGIN:
                string salt = random.Next(100000).ToString();
                string to = lang[random.Next(0, lang.Length)];
                string retJson;
                if (i == times)
                    to = "zh";
                Application.DoEvents();
                string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?"
                    + "q=" + Uri.EscapeDataString(cur)
                    + "&from=" + from
                    + "&to=" + to
                    + "&appid=" + APP_ID
                    + "&salt=" + salt
                    + "&sign=" + EncryptString(APP_ID + cur + salt + APP_KEY);
                Application.DoEvents();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.UserAgent = null;
                request.Timeout = 6000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retJson = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                Application.DoEvents();
                JObject jParser = JObject.Parse(retJson);

                if ()
                cur = Uri.UnescapeDataString(jParser["trans_result"][0]["dst"].ToString());

                Delay(1500);
                System.Diagnostics.Debug.WriteLine(cur);
                from = to;

                view.Text = "生草中...第" + i.ToString() + "次\r\n" + cur;
                Application.DoEvents();
            }
            return cur;
        }
    }
}
