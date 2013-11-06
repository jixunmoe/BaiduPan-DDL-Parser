namespace BaiduPan_DDL_Parser {
	partial class frmMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.editURL = new System.Windows.Forms.TextBox();
			this.btnParse = new System.Windows.Forms.Button();
			this.webLogs = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// editURL
			// 
			this.editURL.Location = new System.Drawing.Point(12, 12);
			this.editURL.Name = "editURL";
			this.editURL.Size = new System.Drawing.Size(260, 20);
			this.editURL.TabIndex = 0;
			this.editURL.Text = "http://pan.baidu.com/s/1ssdNj";
			// 
			// btnParse
			// 
			this.btnParse.Location = new System.Drawing.Point(197, 38);
			this.btnParse.Name = "btnParse";
			this.btnParse.Size = new System.Drawing.Size(75, 23);
			this.btnParse.TabIndex = 1;
			this.btnParse.Text = "解析";
			this.btnParse.UseVisualStyleBackColor = true;
			this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
			// 
			// webLogs
			// 
			this.webLogs.Location = new System.Drawing.Point(12, 67);
			this.webLogs.MinimumSize = new System.Drawing.Size(20, 20);
			this.webLogs.Name = "webLogs";
			this.webLogs.Size = new System.Drawing.Size(260, 182);
			this.webLogs.TabIndex = 2;
			this.webLogs.Url = new System.Uri("about:blank", System.UriKind.Absolute);
			this.webLogs.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webLogs_Navigating);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.webLogs);
			this.Controls.Add(this.btnParse);
			this.Controls.Add(this.editURL);
			this.Name = "frmMain";
			this.Text = "度娘盘直链解析";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.Shown += new System.EventHandler(this.frmMain_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox editURL;
		private System.Windows.Forms.Button btnParse;
		private System.Windows.Forms.WebBrowser webLogs;
	}
}

