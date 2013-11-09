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
using System.IO;

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

			var fsWatcher = new FileSystemWatcher(@"Z:\Game\osu!\Songs\", "*.*");
			fsWatcher.Changed += fsWatcher_Changed;
			fsWatcher.Created += fsWatcher_Created;
			fsWatcher.IncludeSubdirectories = true;
			fsWatcher.NotifyFilter = NotifyFilters.LastAccess;
			fsWatcher.EnableRaisingEvents = true;
		}

		void fsWatcher_Created(object sender, FileSystemEventArgs e) {
			Debug.WriteLine("fsWatcher_Created: " + e.FullPath);
		}

		void fsWatcher_Changed(object sender, FileSystemEventArgs e) {
			Debug.WriteLine("fsWatcher_Changed: " + e.FullPath);
			//throw new NotImplementedException();
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
			string dlLink, serverFilename;

			try {
				serverFilename = Regex.Match(data, @"server_filename=""(.+?)""").Groups[1].ToString();
				var jsonData = client.DownloadString(string.Format(
					"http://pan.baidu.com/share/download?bdstoken={0}&uk={1}&shareid={2}&fid_list=%5B{3}%5D",
					Regex.Match(data, @"bdstoken=""(.+?)""").Groups[1].ToString(),
					Regex.Match(data, @"share_uk=""(.+?)""").Groups[1].ToString(),
					Regex.Match(data, @"share_id=""(.+?)""").Groups[1].ToString(),
					Regex.Match(data, @"fsId=""(.+?)""").Groups[1].ToString()
				));
				dlLink = Regex.Match(jsonData, @"dlink"":""(h.+?)""").Groups[1].ToString().Replace(@"\", "");
				if (dlLink.Substring(0, 1) != "h")
					throw new Exception("链接无效");
			} catch (Exception) {
				appendLog("解析错误: 无匹配链接", "red");
				return;
			}

			HtmlElement tmp = logDoc.CreateElement("a");
			HtmlElement inp = logDoc.CreateElement("input");
			((IHTMLInputElement)inp.DomElement).readOnly = true;

			tmp.SetAttribute("href", dlLink);
			// tmp.SetAttribute("target", "_blank");
			tmp.InnerHtml = string.Format("点我下载 <span class=\"file\">{0}</span>", serverFilename);
			inp.InnerText = dlLink;
			logDoc.Body.AppendChild(tmp);
			logDoc.Body.AppendChild(inp);
			logDoc.Body.AppendChild(logDoc.CreateElement("br"));
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
