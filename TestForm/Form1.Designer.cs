namespace TestForm
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbShowLog = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lbConnectStateChange = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnS1F1 = new System.Windows.Forms.Button();
            this.listboxSmlFile = new System.Windows.Forms.ListBox();
            this.pnLeftFrame = new System.Windows.Forms.Panel();
            this.lbShowSMLFile = new System.Windows.Forms.Label();
            this.tbShowSML = new System.Windows.Forms.TextBox();
            this.btnSent = new System.Windows.Forms.Button();
            this.edMDLN = new System.Windows.Forms.TextBox();
            this.lbMDLN = new System.Windows.Forms.Label();
            this.lbSOFTREV = new System.Windows.Forms.Label();
            this.edSOFTREV = new System.Windows.Forms.TextBox();
            this.lbSVID = new System.Windows.Forms.Label();
            this.comSVID = new System.Windows.Forms.ComboBox();
            this.btnS1F13 = new System.Windows.Forms.Button();
            this.lblControlState = new System.Windows.Forms.Label();
            this.btnEqpOffineLine = new System.Windows.Forms.Button();
            this.btnEqpRequestOnline = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnEqpOnLineLocate = new System.Windows.Forms.Button();
            this.btnEqpOnLineRemote = new System.Windows.Forms.Button();
            this.btnAlarmSetTest = new System.Windows.Forms.Button();
            this.btnAlarmresetTest = new System.Windows.Forms.Button();
            this.btnTerminalMessage = new System.Windows.Forms.Button();
            this.pnLeftFrame.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbShowLog
            // 
            this.lbShowLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbShowLog.Location = new System.Drawing.Point(0, 0);
            this.lbShowLog.Name = "lbShowLog";
            this.lbShowLog.Size = new System.Drawing.Size(379, 16);
            this.lbShowLog.TabIndex = 0;
            this.lbShowLog.Text = "Show Log";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(397, 68);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(95, 42);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "連線";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(498, 67);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(95, 43);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "連線中斷";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // lbConnectStateChange
            // 
            this.lbConnectStateChange.BackColor = System.Drawing.Color.Red;
            this.lbConnectStateChange.Location = new System.Drawing.Point(394, 16);
            this.lbConnectStateChange.Name = "lbConnectStateChange";
            this.lbConnectStateChange.Size = new System.Drawing.Size(95, 28);
            this.lbConnectStateChange.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox1.Location = new System.Drawing.Point(0, 16);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(379, 437);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // btnS1F1
            // 
            this.btnS1F1.Location = new System.Drawing.Point(394, 252);
            this.btnS1F1.Name = "btnS1F1";
            this.btnS1F1.Size = new System.Drawing.Size(95, 43);
            this.btnS1F1.TabIndex = 4;
            this.btnS1F1.Text = "F1S1";
            this.btnS1F1.UseVisualStyleBackColor = true;
            this.btnS1F1.Click += new System.EventHandler(this.btnS1F1_Click);
            // 
            // listboxSmlFile
            // 
            this.listboxSmlFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listboxSmlFile.FormattingEnabled = true;
            this.listboxSmlFile.ItemHeight = 16;
            this.listboxSmlFile.Location = new System.Drawing.Point(0, 469);
            this.listboxSmlFile.Name = "listboxSmlFile";
            this.listboxSmlFile.Size = new System.Drawing.Size(379, 177);
            this.listboxSmlFile.TabIndex = 5;
            this.listboxSmlFile.SelectedIndexChanged += new System.EventHandler(this.listboxSmlFile_SelectedIndexChanged);
            this.listboxSmlFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listboxSmlFile_MouseDoubleClick);
            // 
            // pnLeftFrame
            // 
            this.pnLeftFrame.Controls.Add(this.listboxSmlFile);
            this.pnLeftFrame.Controls.Add(this.lbShowSMLFile);
            this.pnLeftFrame.Controls.Add(this.richTextBox1);
            this.pnLeftFrame.Controls.Add(this.lbShowLog);
            this.pnLeftFrame.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeftFrame.Location = new System.Drawing.Point(0, 0);
            this.pnLeftFrame.Name = "pnLeftFrame";
            this.pnLeftFrame.Size = new System.Drawing.Size(379, 646);
            this.pnLeftFrame.TabIndex = 6;
            // 
            // lbShowSMLFile
            // 
            this.lbShowSMLFile.AutoSize = true;
            this.lbShowSMLFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbShowSMLFile.Location = new System.Drawing.Point(0, 453);
            this.lbShowSMLFile.Name = "lbShowSMLFile";
            this.lbShowSMLFile.Size = new System.Drawing.Size(89, 16);
            this.lbShowSMLFile.TabIndex = 4;
            this.lbShowSMLFile.Text = "Show SML File";
            // 
            // tbShowSML
            // 
            this.tbShowSML.Location = new System.Drawing.Point(385, 466);
            this.tbShowSML.Multiline = true;
            this.tbShowSML.Name = "tbShowSML";
            this.tbShowSML.ReadOnly = true;
            this.tbShowSML.Size = new System.Drawing.Size(276, 119);
            this.tbShowSML.TabIndex = 7;
            // 
            // btnSent
            // 
            this.btnSent.Location = new System.Drawing.Point(385, 591);
            this.btnSent.Name = "btnSent";
            this.btnSent.Size = new System.Drawing.Size(95, 43);
            this.btnSent.TabIndex = 8;
            this.btnSent.Text = "發送";
            this.btnSent.UseVisualStyleBackColor = true;
            this.btnSent.Click += new System.EventHandler(this.btnSent_Click);
            // 
            // edMDLN
            // 
            this.edMDLN.Location = new System.Drawing.Point(498, 134);
            this.edMDLN.Name = "edMDLN";
            this.edMDLN.Size = new System.Drawing.Size(95, 23);
            this.edMDLN.TabIndex = 9;
            this.edMDLN.Text = "MDLN";
            this.edMDLN.TextChanged += new System.EventHandler(this.edMDLN_TextChanged);
            // 
            // lbMDLN
            // 
            this.lbMDLN.AutoSize = true;
            this.lbMDLN.Location = new System.Drawing.Point(394, 137);
            this.lbMDLN.Name = "lbMDLN";
            this.lbMDLN.Size = new System.Drawing.Size(45, 16);
            this.lbMDLN.TabIndex = 10;
            this.lbMDLN.Text = "MDLN";
            // 
            // lbSOFTREV
            // 
            this.lbSOFTREV.AutoSize = true;
            this.lbSOFTREV.Location = new System.Drawing.Point(394, 176);
            this.lbSOFTREV.Name = "lbSOFTREV";
            this.lbSOFTREV.Size = new System.Drawing.Size(61, 16);
            this.lbSOFTREV.TabIndex = 11;
            this.lbSOFTREV.Text = "SOFTREV";
            // 
            // edSOFTREV
            // 
            this.edSOFTREV.Location = new System.Drawing.Point(498, 173);
            this.edSOFTREV.Name = "edSOFTREV";
            this.edSOFTREV.Size = new System.Drawing.Size(95, 23);
            this.edSOFTREV.TabIndex = 12;
            this.edSOFTREV.Text = "SOFTREV";
            this.edSOFTREV.TextChanged += new System.EventHandler(this.edSOFTREV_TextChanged);
            // 
            // lbSVID
            // 
            this.lbSVID.AutoSize = true;
            this.lbSVID.Location = new System.Drawing.Point(396, 224);
            this.lbSVID.Name = "lbSVID";
            this.lbSVID.Size = new System.Drawing.Size(38, 16);
            this.lbSVID.TabIndex = 13;
            this.lbSVID.Text = "SV ID";
            // 
            // comSVID
            // 
            this.comSVID.FormattingEnabled = true;
            this.comSVID.Location = new System.Drawing.Point(498, 221);
            this.comSVID.Name = "comSVID";
            this.comSVID.Size = new System.Drawing.Size(95, 24);
            this.comSVID.TabIndex = 14;
            // 
            // btnS1F13
            // 
            this.btnS1F13.Location = new System.Drawing.Point(498, 252);
            this.btnS1F13.Name = "btnS1F13";
            this.btnS1F13.Size = new System.Drawing.Size(95, 43);
            this.btnS1F13.TabIndex = 15;
            this.btnS1F13.Text = "F1S13";
            this.btnS1F13.UseVisualStyleBackColor = true;
            this.btnS1F13.Click += new System.EventHandler(this.btnS1F3_Click);
            // 
            // lblControlState
            // 
            this.lblControlState.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblControlState.Location = new System.Drawing.Point(394, 410);
            this.lblControlState.Name = "lblControlState";
            this.lblControlState.Size = new System.Drawing.Size(267, 43);
            this.lblControlState.TabIndex = 16;
            this.lblControlState.Text = "Ini";
            this.lblControlState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEqpOffineLine
            // 
            this.btnEqpOffineLine.Location = new System.Drawing.Point(688, 68);
            this.btnEqpOffineLine.Name = "btnEqpOffineLine";
            this.btnEqpOffineLine.Size = new System.Drawing.Size(95, 43);
            this.btnEqpOffineLine.TabIndex = 17;
            this.btnEqpOffineLine.Text = "EQP Offline";
            this.btnEqpOffineLine.UseVisualStyleBackColor = true;
            this.btnEqpOffineLine.Click += new System.EventHandler(this.btnEqpOffineLine_Click);
            // 
            // btnEqpRequestOnline
            // 
            this.btnEqpRequestOnline.Location = new System.Drawing.Point(688, 117);
            this.btnEqpRequestOnline.Name = "btnEqpRequestOnline";
            this.btnEqpRequestOnline.Size = new System.Drawing.Size(95, 43);
            this.btnEqpRequestOnline.TabIndex = 18;
            this.btnEqpRequestOnline.Text = "EQP Request Online";
            this.btnEqpRequestOnline.UseVisualStyleBackColor = true;
            this.btnEqpRequestOnline.Click += new System.EventHandler(this.btnEqpRequestOnline_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(566, 591);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(95, 43);
            this.btnClear.TabIndex = 19;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnEqpOnLineLocate
            // 
            this.btnEqpOnLineLocate.Location = new System.Drawing.Point(688, 166);
            this.btnEqpOnLineLocate.Name = "btnEqpOnLineLocate";
            this.btnEqpOnLineLocate.Size = new System.Drawing.Size(95, 43);
            this.btnEqpOnLineLocate.TabIndex = 20;
            this.btnEqpOnLineLocate.Text = "Locate";
            this.btnEqpOnLineLocate.UseVisualStyleBackColor = true;
            this.btnEqpOnLineLocate.Click += new System.EventHandler(this.btnEqpOnLineLocate_Click);
            // 
            // btnEqpOnLineRemote
            // 
            this.btnEqpOnLineRemote.Location = new System.Drawing.Point(688, 215);
            this.btnEqpOnLineRemote.Name = "btnEqpOnLineRemote";
            this.btnEqpOnLineRemote.Size = new System.Drawing.Size(95, 43);
            this.btnEqpOnLineRemote.TabIndex = 21;
            this.btnEqpOnLineRemote.Text = "Remote";
            this.btnEqpOnLineRemote.UseVisualStyleBackColor = true;
            this.btnEqpOnLineRemote.Click += new System.EventHandler(this.btnEqpOnLineRemote_Click);
            // 
            // btnAlarmSetTest
            // 
            this.btnAlarmSetTest.Location = new System.Drawing.Point(394, 319);
            this.btnAlarmSetTest.Name = "btnAlarmSetTest";
            this.btnAlarmSetTest.Size = new System.Drawing.Size(95, 43);
            this.btnAlarmSetTest.TabIndex = 22;
            this.btnAlarmSetTest.Text = "Alarm Set";
            this.btnAlarmSetTest.UseVisualStyleBackColor = true;
            this.btnAlarmSetTest.Click += new System.EventHandler(this.btnAlarmSetTest_Click);
            // 
            // btnAlarmresetTest
            // 
            this.btnAlarmresetTest.Location = new System.Drawing.Point(498, 319);
            this.btnAlarmresetTest.Name = "btnAlarmresetTest";
            this.btnAlarmresetTest.Size = new System.Drawing.Size(95, 43);
            this.btnAlarmresetTest.TabIndex = 23;
            this.btnAlarmresetTest.Text = "Alarm Reset";
            this.btnAlarmresetTest.UseVisualStyleBackColor = true;
            this.btnAlarmresetTest.Click += new System.EventHandler(this.btnAlarmresetTest_Click);
            // 
            // btnTerminalMessage
            // 
            this.btnTerminalMessage.Location = new System.Drawing.Point(688, 319);
            this.btnTerminalMessage.Name = "btnTerminalMessage";
            this.btnTerminalMessage.Size = new System.Drawing.Size(95, 43);
            this.btnTerminalMessage.TabIndex = 24;
            this.btnTerminalMessage.Text = "Terminal MSG";
            this.btnTerminalMessage.UseVisualStyleBackColor = true;
            this.btnTerminalMessage.Click += new System.EventHandler(this.btnTerminalMessage_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 646);
            this.Controls.Add(this.btnTerminalMessage);
            this.Controls.Add(this.btnAlarmresetTest);
            this.Controls.Add(this.btnAlarmSetTest);
            this.Controls.Add(this.btnEqpOnLineRemote);
            this.Controls.Add(this.btnEqpOnLineLocate);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnEqpRequestOnline);
            this.Controls.Add(this.btnEqpOffineLine);
            this.Controls.Add(this.lblControlState);
            this.Controls.Add(this.btnS1F13);
            this.Controls.Add(this.comSVID);
            this.Controls.Add(this.lbSVID);
            this.Controls.Add(this.edSOFTREV);
            this.Controls.Add(this.lbSOFTREV);
            this.Controls.Add(this.lbMDLN);
            this.Controls.Add(this.edMDLN);
            this.Controls.Add(this.btnSent);
            this.Controls.Add(this.tbShowSML);
            this.Controls.Add(this.pnLeftFrame);
            this.Controls.Add(this.btnS1F1);
            this.Controls.Add(this.lbConnectStateChange);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnLeftFrame.ResumeLayout(false);
            this.pnLeftFrame.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbShowLog;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lbConnectStateChange;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnS1F1;
        private System.Windows.Forms.ListBox listboxSmlFile;
        private System.Windows.Forms.Panel pnLeftFrame;
        private System.Windows.Forms.Label lbShowSMLFile;
        private System.Windows.Forms.TextBox tbShowSML;
        private System.Windows.Forms.Button btnSent;
        private System.Windows.Forms.TextBox edMDLN;
        private System.Windows.Forms.Label lbMDLN;
        private System.Windows.Forms.Label lbSOFTREV;
        private System.Windows.Forms.TextBox edSOFTREV;
        private System.Windows.Forms.Label lbSVID;
        private System.Windows.Forms.ComboBox comSVID;
        private System.Windows.Forms.Button btnS1F13;
        private System.Windows.Forms.Label lblControlState;
        private System.Windows.Forms.Button btnEqpOffineLine;
        private System.Windows.Forms.Button btnEqpRequestOnline;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnEqpOnLineLocate;
        private System.Windows.Forms.Button btnEqpOnLineRemote;
        private System.Windows.Forms.Button btnAlarmSetTest;
        private System.Windows.Forms.Button btnAlarmresetTest;
        private System.Windows.Forms.Button btnTerminalMessage;
    }
}

