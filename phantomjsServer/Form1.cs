using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace phantomjsServer
{
    public partial class Form1 : Form
    {
        Process p = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 启动进程
            p = new Process();
            p.StartInfo.FileName = Environment.CurrentDirectory + "//phantomjs//phantomjs_1.9V.exe";
           
            string ExcuteArg = Environment.CurrentDirectory + "//script//highcharts-convert.js -host 127.0.0.1 -port 3003";
            p.StartInfo.Arguments = string.Format(ExcuteArg);
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.UseShellExecute = false;
            //重定向标准输出 
            p.StartInfo.RedirectStandardOutput = true;
            //重定向错误输出 
            p.StartInfo.RedirectStandardError = false; ;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            string[] result = { };
            if (!p.Start())
            {
                throw new Exception("无法启动Headless测试引擎.");
            }
          
            result = p.StandardOutput.ReadToEnd().Split(new char[] { '\r', '\n' });
            if (result.Length == 0)
            {
                result[0] = "已成功启动，但无数据";
            }
            foreach (string s in result)
            {
                list_Msg.Items.Add(s);
            }
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (p != null)
            {
                p.Dispose();
                p.Close();
            }
        }
    }
}
