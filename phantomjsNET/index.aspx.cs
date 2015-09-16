using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phantomjsNET
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //peng8
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:3003/";
            string param= "{\"infile\":\"{ xAxis: { categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']},series: [{ data: [29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]}]}; \",\"callback\":\"function(chart) { chart.renderer.arc(200, 150, 100, 50, -Math.PI, 0).attr({ fill: '#FCFFC5',stroke: 'black','stroke-width' : 1}).add(); } \",\"constr\":\"Chart\"}";
            TextBox1.Text= HttpPostNew(url, param);
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(TextBox1.Text));
            Bitmap bmp = new Bitmap(stream);
            string randomName = System.DateTime.Now.ToString("yyyyMMddhhssmm") + ".png";
            string saveUrl = Server.MapPath("/images/")+randomName;
            bmp.Save(saveUrl, ImageFormat.Png);
            stream.Dispose();
            stream.Close();
            bmp.Dispose();
            Image1.ImageUrl = "~/images/"+ randomName;
        }
        private string HttpPostNew(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
           
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        private Hashtable HandleImages(List<string> ListChartJsonPath)
        {
            #region 启动进程保存图片
            Hashtable ht = new Hashtable();
            for (int i = 0; i < ListChartJsonPath.Count; i++)
            {
                Process p = new Process();
                p.StartInfo.FileName = Server.MapPath(@"\GenerateImage\phantomjs.exe");
                //定义图片名称
                string filename = "divchart" + (i + 1).ToString() + Guid.NewGuid().ToString();
                ht.Add("divchart" + (i + 1).ToString(), filename);
                string outfile = Server.MapPath(@"\ImageTemp\" + filename + ".png");
                string infile = ListChartJsonPath[i];
                string ExcuteArg = Server.MapPath(@"\GenerateImage\highcharts-convert.js") + " -infile " + infile + " -outfile " + outfile + " -scale 2.5 -width 700 -constr Chart";
                p.StartInfo.Arguments = string.Format(ExcuteArg);
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                //重定向标准输出 
                p.StartInfo.RedirectStandardOutput = true;
                //重定向错误输出 
                p.StartInfo.RedirectStandardError = false; ;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                if (!p.Start())
                { throw new Exception("无法启动Headless测试引擎."); }
                //string[] result = p.StandardOutput.ReadToEnd().Split(new char[] { '\r', '\n' });
                p.WaitForExit();
                p.Close();

            }
            return ht;
            #endregion
        }

        
    }
}