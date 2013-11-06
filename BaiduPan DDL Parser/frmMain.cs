using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;

namespace BaiduPan_DDL_Parser {
	public partial class frmMain : Form {
		public frmMain() {
			InitializeComponent();
		}

		public HtmlDocument logDoc = null;
		public IHTMLDocument2 logDocRaw = null;
		void webLogs_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			logDoc = webLogs.Document;
			logDocRaw = (IHTMLDocument2)logDoc.DomDocument;
			IHTMLStyleSheet ss = logDocRaw.createStyleSheet("", 0);
			ss.cssText = Properties.Resources.defaultTemplate;

			var myScript = logDoc.CreateElement("script");
			((IHTMLScriptElement)myScript.DomElement).text = Properties.Resources.hoverJavaScript;

			// logDoc.Body.AppendChild(myStyle);
			logDoc.Body.AppendChild(myScript);
			Debug.WriteLine(logDoc.Body.InnerHtml);
		}

		private HtmlElement appendLog (string log, string className = "", bool makeNewLine = true) {
			if (logDoc == null)
				return null;

			var newString = logDoc.CreateElement(makeNewLine ? "p" : "span");
			newString.SetAttribute("class", className);

			newString.InnerHtml = log;
			logDoc.Body.AppendChild(newString);
			logDoc.InvokeScript(Properties.Resources.hoverJavaScript);
			return newString;
		}

		private void frmMain_Load(object sender, EventArgs e) {
			this.webLogs.DocumentCompleted += webLogs_DocumentCompleted;
		}

		private void frmMain_Shown(object sender, EventArgs e) {
			appendLog("百度盘直链解析工具~");
			appendLog(string.Format(
				@"版本号: <span style=""color: red"">{0}</span>",
				Assembly.GetExecutingAssembly().GetName().Version
			));
			getData.DoWork += getData_DoWork;
			getData.RunWorkerCompleted += getData_RunWorkerCompleted;
		}

		void getData_DoWork(object sender, DoWorkEventArgs e) {
			var client = new System.Net.WebClient();
			client.Encoding = System.Text.Encoding.UTF8;
			var data = client.DownloadString(editURL.Text);
			var m = Regex.Matches(data, "_dlink=\"(h.+?)\"");
			if (m.Count == 0) {
				appendLog("解析错误: 无法匹配到链接", "red");
				return;
			}

			var m2 = Regex.Match(data, @"server_filename=""(.+?)""");

			appendLog(string.Format("档案 <span class=\"file\">{1}</span> 共发现 <span class=\"red\">{0}</span> 个下载线路:",
				m.Count.ToString(),
				m2.Groups[1].Value
			));

			for (int i = 0; i < m.Count; i++) {
				HtmlElement tmp = logDoc.CreateElement("a");
				HtmlElement inp = logDoc.CreateElement("input");
				((IHTMLInputElement)inp.DomElement).readOnly = true;

				var link = m[i].Groups[1].Value;
				tmp.SetAttribute("href", link);
				// tmp.SetAttribute("target", "_blank");
				tmp.InnerText = string.Format("下载线路{0}", (i + 1).ToString());
				inp.InnerText = link;
				logDoc.Body.AppendChild(tmp);
				logDoc.Body.AppendChild(inp);
				logDoc.Body.AppendChild(logDoc.CreateElement("br"));
				Debug.WriteLine( m[i].Groups[1].Value );
			}
		}

		void getData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			btnParse.Enabled = true;
		}

		private BackgroundWorker getData = new BackgroundWorker();

		private void btnParse_Click(object sender, EventArgs e) {
			btnParse.Enabled = false;
			getData.RunWorkerAsync();
			// Debug.WriteLine(data);
		}

		private void webLogs_Navigating(object sender, WebBrowserNavigatingEventArgs e) {
			string url = e.Url.AbsoluteUri;
			if (!url.ToLower().StartsWith("http://"))
				return;
			e.Cancel = true;
			Process.Start(url);
		}


	}
}
