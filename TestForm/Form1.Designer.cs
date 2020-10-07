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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.btnEqpOnLineLocal = new System.Windows.Forms.Button();
            this.btnEqpOnLineRemote = new System.Windows.Forms.Button();
            this.btnAlarmSetTest = new System.Windows.Forms.Button();
            this.btnAlarmresetTest = new System.Windows.Forms.Button();
            this.btnTerminalMessage = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.pageGeneral = new System.Windows.Forms.TabPage();
            this.gbCommand = new System.Windows.Forms.GroupBox();
            this.btnS2F17 = new System.Windows.Forms.Button();
            this.pageAlarm = new System.Windows.Forms.TabPage();
            this.lbSendTerminalMSG = new System.Windows.Forms.Label();
            this.rtbSendTerminalMSG = new System.Windows.Forms.RichTextBox();
            this.rtbRecieveTerminalMSG = new System.Windows.Forms.RichTextBox();
            this.lbReceiveTerminalMSG = new System.Windows.Forms.Label();
            this.lbAlarm = new System.Windows.Forms.Label();
            this.listAlarmID = new System.Windows.Forms.ListBox();
            this.pageEvent = new System.Windows.Forms.TabPage();
            this.cbAnnotated = new System.Windows.Forms.CheckBox();
            this.btnSetEvent = new System.Windows.Forms.Button();
            this.listVIDItem = new System.Windows.Forms.ListBox();
            this.lbVIDItem = new System.Windows.Forms.Label();
            this.listReportItem = new System.Windows.Forms.ListBox();
            this.lbReportItem = new System.Windows.Forms.Label();
            this.listEventItem = new System.Windows.Forms.ListBox();
            this.lbEventItem = new System.Windows.Forms.Label();
            this.pageCustomizeSetup = new System.Windows.Forms.TabPage();
            this.rtbCustomize = new System.Windows.Forms.RichTextBox();
            this.pageCMS = new System.Windows.Forms.TabPage();
            this.btnE87NR1 = new System.Windows.Forms.Button();
            this.btnLP2 = new System.Windows.Forms.Button();
            this.btnLP1 = new System.Windows.Forms.Button();
            this.ctrlCMS = new System.Windows.Forms.TabControl();
            this.pageLPT = new System.Windows.Forms.TabPage();
            this.lbShowpreviousLPTS = new System.Windows.Forms.Label();
            this.lbShowCurrentLPTS = new System.Windows.Forms.Label();
            this.btnReadyToUnload = new System.Windows.Forms.Button();
            this.btnReadyToLoad = new System.Windows.Forms.Button();
            this.btnTranferBlocked = new System.Windows.Forms.Button();
            this.btnTransferReady = new System.Windows.Forms.Button();
            this.btnOutOfService = new System.Windows.Forms.Button();
            this.btnInService = new System.Windows.Forms.Button();
            this.lbCurrentLPTS = new System.Windows.Forms.Label();
            this.lbPreviousLPTS = new System.Windows.Forms.Label();
            this.picArrow = new System.Windows.Forms.PictureBox();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectReadyToUpload = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectReadyToLoad = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectTransferReady = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectTranferBlacked = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectInService = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectOutOfService = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.picLPTSM = new System.Windows.Forms.PictureBox();
            this.pageAccessMode = new System.Windows.Forms.TabPage();
            this.btnReserved = new System.Windows.Forms.Button();
            this.btnNotReserved = new System.Windows.Forms.Button();
            this.lbAccessMode = new System.Windows.Forms.Label();
            this.btnAuto = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectReserved = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectNotReserved = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.lineDenyAccessMode2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineDenyAccessMode1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.rectAuto = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectManual = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.picAccessMode = new System.Windows.Forms.PictureBox();
            this.picReservation = new System.Windows.Forms.PictureBox();
            this.pageAssociated = new System.Windows.Forms.TabPage();
            this.lbAssociatedCarrierID = new System.Windows.Forms.Label();
            this.lbShowCarrierID = new System.Windows.Forms.Label();
            this.shapeContainer3 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectAssociated = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectNotAssociated = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.picAssociated = new System.Windows.Forms.PictureBox();
            this.pageCarrierRequest = new System.Windows.Forms.TabPage();
            this.btnAddCarrierID = new System.Windows.Forms.Button();
            this.lbCID = new System.Windows.Forms.Label();
            this.txtCarrierID = new System.Windows.Forms.TextBox();
            this.btnClearRequest = new System.Windows.Forms.Button();
            this.lbSecondRequest = new System.Windows.Forms.Label();
            this.lbMainRequest = new System.Windows.Forms.Label();
            this.rtxtSecondRequest = new System.Windows.Forms.RichTextBox();
            this.rtxtMainRequest = new System.Windows.Forms.RichTextBox();
            this.pageCarrierStatus = new System.Windows.Forms.TabPage();
            this.btnSlopMapVerifOK = new System.Windows.Forms.Button();
            this.btnIDVerifOK = new System.Windows.Forms.Button();
            this.shapeContainer5 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rtCarrierStopped = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtCarrierComplete = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtInAccessed = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtNotAccessed = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtSlopMapVerificationFail = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtSlopMapVerificationOK = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtWaitingForHostSlotMap = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtSlotMapNotRead = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtIDverificationFail = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtIDVerificationOK = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtWaitingForHostID = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtIDNotRead = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtCarrierAccessingStatus = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtCarrierSlotMapStatus = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rtCarrierInStatus = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.picCarrierStatus = new System.Windows.Forms.PictureBox();
            this.lbNowLoadPort = new System.Windows.Forms.Label();
            this.shapeContainer4 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectLP2 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectLP1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timeUIUpdate = new System.Windows.Forms.Timer(this.components);
            this.btnIDVerifFail = new System.Windows.Forms.Button();
            this.btnE87NR3 = new System.Windows.Forms.Button();
            this.pnLeftFrame.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.pageGeneral.SuspendLayout();
            this.gbCommand.SuspendLayout();
            this.pageAlarm.SuspendLayout();
            this.pageEvent.SuspendLayout();
            this.pageCustomizeSetup.SuspendLayout();
            this.pageCMS.SuspendLayout();
            this.ctrlCMS.SuspendLayout();
            this.pageLPT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLPTSM)).BeginInit();
            this.pageAccessMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAccessMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReservation)).BeginInit();
            this.pageAssociated.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAssociated)).BeginInit();
            this.pageCarrierRequest.SuspendLayout();
            this.pageCarrierStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCarrierStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lbShowLog
            // 
            this.lbShowLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbShowLog.Location = new System.Drawing.Point(0, 0);
            this.lbShowLog.Name = "lbShowLog";
            this.lbShowLog.Size = new System.Drawing.Size(386, 16);
            this.lbShowLog.TabIndex = 0;
            this.lbShowLog.Text = "Show Log";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(1084, 568);
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
            this.btnDisconnect.Location = new System.Drawing.Point(1084, 621);
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
            this.lbConnectStateChange.Location = new System.Drawing.Point(983, 568);
            this.lbConnectStateChange.Name = "lbConnectStateChange";
            this.lbConnectStateChange.Size = new System.Drawing.Size(95, 42);
            this.lbConnectStateChange.TabIndex = 2;
            this.lbConnectStateChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox1.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 16);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(386, 528);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // btnS1F1
            // 
            this.btnS1F1.Location = new System.Drawing.Point(17, 22);
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
            this.listboxSmlFile.Location = new System.Drawing.Point(0, 560);
            this.listboxSmlFile.Name = "listboxSmlFile";
            this.listboxSmlFile.Size = new System.Drawing.Size(386, 117);
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
            this.pnLeftFrame.Size = new System.Drawing.Size(386, 677);
            this.pnLeftFrame.TabIndex = 6;
            // 
            // lbShowSMLFile
            // 
            this.lbShowSMLFile.AutoSize = true;
            this.lbShowSMLFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbShowSMLFile.Location = new System.Drawing.Point(0, 544);
            this.lbShowSMLFile.Name = "lbShowSMLFile";
            this.lbShowSMLFile.Size = new System.Drawing.Size(89, 16);
            this.lbShowSMLFile.TabIndex = 4;
            this.lbShowSMLFile.Text = "Show SML File";
            // 
            // tbShowSML
            // 
            this.tbShowSML.Location = new System.Drawing.Point(388, 567);
            this.tbShowSML.Multiline = true;
            this.tbShowSML.Name = "tbShowSML";
            this.tbShowSML.ReadOnly = true;
            this.tbShowSML.Size = new System.Drawing.Size(354, 100);
            this.tbShowSML.TabIndex = 7;
            // 
            // btnSent
            // 
            this.btnSent.Enabled = false;
            this.btnSent.Location = new System.Drawing.Point(755, 567);
            this.btnSent.Name = "btnSent";
            this.btnSent.Size = new System.Drawing.Size(95, 43);
            this.btnSent.TabIndex = 8;
            this.btnSent.Text = "發送";
            this.btnSent.UseVisualStyleBackColor = true;
            this.btnSent.Click += new System.EventHandler(this.btnSent_Click);
            // 
            // edMDLN
            // 
            this.edMDLN.Location = new System.Drawing.Point(99, 21);
            this.edMDLN.Name = "edMDLN";
            this.edMDLN.Size = new System.Drawing.Size(95, 23);
            this.edMDLN.TabIndex = 9;
            this.edMDLN.Text = "MDLN";
            this.edMDLN.TextChanged += new System.EventHandler(this.edMDLN_TextChanged);
            // 
            // lbMDLN
            // 
            this.lbMDLN.AutoSize = true;
            this.lbMDLN.Location = new System.Drawing.Point(20, 24);
            this.lbMDLN.Name = "lbMDLN";
            this.lbMDLN.Size = new System.Drawing.Size(45, 16);
            this.lbMDLN.TabIndex = 10;
            this.lbMDLN.Text = "MDLN";
            // 
            // lbSOFTREV
            // 
            this.lbSOFTREV.AutoSize = true;
            this.lbSOFTREV.Location = new System.Drawing.Point(20, 53);
            this.lbSOFTREV.Name = "lbSOFTREV";
            this.lbSOFTREV.Size = new System.Drawing.Size(61, 16);
            this.lbSOFTREV.TabIndex = 11;
            this.lbSOFTREV.Text = "SOFTREV";
            // 
            // edSOFTREV
            // 
            this.edSOFTREV.Location = new System.Drawing.Point(99, 50);
            this.edSOFTREV.Name = "edSOFTREV";
            this.edSOFTREV.Size = new System.Drawing.Size(95, 23);
            this.edSOFTREV.TabIndex = 12;
            this.edSOFTREV.Text = "SOFTREV";
            this.edSOFTREV.TextChanged += new System.EventHandler(this.edSOFTREV_TextChanged);
            // 
            // lbSVID
            // 
            this.lbSVID.AutoSize = true;
            this.lbSVID.Location = new System.Drawing.Point(806, 400);
            this.lbSVID.Name = "lbSVID";
            this.lbSVID.Size = new System.Drawing.Size(38, 16);
            this.lbSVID.TabIndex = 13;
            this.lbSVID.Text = "SV ID";
            this.lbSVID.Visible = false;
            // 
            // comSVID
            // 
            this.comSVID.FormattingEnabled = true;
            this.comSVID.Location = new System.Drawing.Point(836, 497);
            this.comSVID.Name = "comSVID";
            this.comSVID.Size = new System.Drawing.Size(95, 24);
            this.comSVID.TabIndex = 14;
            this.comSVID.Visible = false;
            // 
            // btnS1F13
            // 
            this.btnS1F13.Location = new System.Drawing.Point(118, 22);
            this.btnS1F13.Name = "btnS1F13";
            this.btnS1F13.Size = new System.Drawing.Size(95, 43);
            this.btnS1F13.TabIndex = 15;
            this.btnS1F13.Text = "F1S13";
            this.btnS1F13.UseVisualStyleBackColor = true;
            this.btnS1F13.Click += new System.EventHandler(this.btnS1F3_Click);
            // 
            // lblControlState
            // 
            this.lblControlState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblControlState.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblControlState.Location = new System.Drawing.Point(6, 90);
            this.lblControlState.Name = "lblControlState";
            this.lblControlState.Size = new System.Drawing.Size(398, 43);
            this.lblControlState.TabIndex = 16;
            this.lblControlState.Text = "Ini";
            this.lblControlState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEqpOffineLine
            // 
            this.btnEqpOffineLine.Location = new System.Drawing.Point(6, 136);
            this.btnEqpOffineLine.Name = "btnEqpOffineLine";
            this.btnEqpOffineLine.Size = new System.Drawing.Size(95, 43);
            this.btnEqpOffineLine.TabIndex = 17;
            this.btnEqpOffineLine.Text = "EQP Offline";
            this.btnEqpOffineLine.UseVisualStyleBackColor = true;
            this.btnEqpOffineLine.Click += new System.EventHandler(this.btnEqpOffineLine_Click);
            // 
            // btnEqpRequestOnline
            // 
            this.btnEqpRequestOnline.Location = new System.Drawing.Point(107, 136);
            this.btnEqpRequestOnline.Name = "btnEqpRequestOnline";
            this.btnEqpRequestOnline.Size = new System.Drawing.Size(95, 43);
            this.btnEqpRequestOnline.TabIndex = 18;
            this.btnEqpRequestOnline.Text = "EQP Request Online";
            this.btnEqpRequestOnline.UseVisualStyleBackColor = true;
            this.btnEqpRequestOnline.Click += new System.EventHandler(this.btnEqpRequestOnline_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(755, 624);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(95, 43);
            this.btnClear.TabIndex = 19;
            this.btnClear.Text = "清除LOG";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnEqpOnLineLocal
            // 
            this.btnEqpOnLineLocal.Location = new System.Drawing.Point(208, 136);
            this.btnEqpOnLineLocal.Name = "btnEqpOnLineLocal";
            this.btnEqpOnLineLocal.Size = new System.Drawing.Size(95, 43);
            this.btnEqpOnLineLocal.TabIndex = 20;
            this.btnEqpOnLineLocal.Text = "Local";
            this.btnEqpOnLineLocal.UseVisualStyleBackColor = true;
            this.btnEqpOnLineLocal.Click += new System.EventHandler(this.btnEqpOnLineLocate_Click);
            // 
            // btnEqpOnLineRemote
            // 
            this.btnEqpOnLineRemote.Location = new System.Drawing.Point(309, 136);
            this.btnEqpOnLineRemote.Name = "btnEqpOnLineRemote";
            this.btnEqpOnLineRemote.Size = new System.Drawing.Size(95, 43);
            this.btnEqpOnLineRemote.TabIndex = 21;
            this.btnEqpOnLineRemote.Text = "Remote";
            this.btnEqpOnLineRemote.UseVisualStyleBackColor = true;
            this.btnEqpOnLineRemote.Click += new System.EventHandler(this.btnEqpOnLineRemote_Click);
            // 
            // btnAlarmSetTest
            // 
            this.btnAlarmSetTest.Location = new System.Drawing.Point(360, 35);
            this.btnAlarmSetTest.Name = "btnAlarmSetTest";
            this.btnAlarmSetTest.Size = new System.Drawing.Size(95, 43);
            this.btnAlarmSetTest.TabIndex = 22;
            this.btnAlarmSetTest.Text = "Alarm Set";
            this.btnAlarmSetTest.UseVisualStyleBackColor = true;
            this.btnAlarmSetTest.Click += new System.EventHandler(this.btnAlarmSetTest_Click);
            // 
            // btnAlarmresetTest
            // 
            this.btnAlarmresetTest.Location = new System.Drawing.Point(360, 84);
            this.btnAlarmresetTest.Name = "btnAlarmresetTest";
            this.btnAlarmresetTest.Size = new System.Drawing.Size(95, 43);
            this.btnAlarmresetTest.TabIndex = 23;
            this.btnAlarmresetTest.Text = "Alarm Reset";
            this.btnAlarmresetTest.UseVisualStyleBackColor = true;
            this.btnAlarmresetTest.Click += new System.EventHandler(this.btnAlarmresetTest_Click);
            // 
            // btnTerminalMessage
            // 
            this.btnTerminalMessage.Location = new System.Drawing.Point(682, 410);
            this.btnTerminalMessage.Name = "btnTerminalMessage";
            this.btnTerminalMessage.Size = new System.Drawing.Size(95, 43);
            this.btnTerminalMessage.TabIndex = 24;
            this.btnTerminalMessage.Text = "Terminal MSG";
            this.btnTerminalMessage.UseVisualStyleBackColor = true;
            this.btnTerminalMessage.Click += new System.EventHandler(this.btnTerminalMessage_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.pageGeneral);
            this.tabControl1.Controls.Add(this.pageAlarm);
            this.tabControl1.Controls.Add(this.pageEvent);
            this.tabControl1.Controls.Add(this.pageCustomizeSetup);
            this.tabControl1.Controls.Add(this.pageCMS);
            this.tabControl1.Location = new System.Drawing.Point(388, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(793, 561);
            this.tabControl1.TabIndex = 25;
            // 
            // pageGeneral
            // 
            this.pageGeneral.Controls.Add(this.gbCommand);
            this.pageGeneral.Controls.Add(this.lbMDLN);
            this.pageGeneral.Controls.Add(this.lblControlState);
            this.pageGeneral.Controls.Add(this.lbSOFTREV);
            this.pageGeneral.Controls.Add(this.edMDLN);
            this.pageGeneral.Controls.Add(this.edSOFTREV);
            this.pageGeneral.Controls.Add(this.btnEqpOnLineRemote);
            this.pageGeneral.Controls.Add(this.btnEqpOffineLine);
            this.pageGeneral.Controls.Add(this.btnEqpOnLineLocal);
            this.pageGeneral.Controls.Add(this.btnEqpRequestOnline);
            this.pageGeneral.Location = new System.Drawing.Point(4, 25);
            this.pageGeneral.Name = "pageGeneral";
            this.pageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.pageGeneral.Size = new System.Drawing.Size(785, 532);
            this.pageGeneral.TabIndex = 0;
            this.pageGeneral.Text = "General";
            this.pageGeneral.UseVisualStyleBackColor = true;
            // 
            // gbCommand
            // 
            this.gbCommand.Controls.Add(this.btnS2F17);
            this.gbCommand.Controls.Add(this.btnS1F1);
            this.gbCommand.Controls.Add(this.btnS1F13);
            this.gbCommand.Location = new System.Drawing.Point(6, 205);
            this.gbCommand.Name = "gbCommand";
            this.gbCommand.Size = new System.Drawing.Size(771, 254);
            this.gbCommand.TabIndex = 22;
            this.gbCommand.TabStop = false;
            this.gbCommand.Text = "Command";
            // 
            // btnS2F17
            // 
            this.btnS2F17.Location = new System.Drawing.Point(17, 76);
            this.btnS2F17.Name = "btnS2F17";
            this.btnS2F17.Size = new System.Drawing.Size(95, 43);
            this.btnS2F17.TabIndex = 16;
            this.btnS2F17.Text = "F2S17";
            this.btnS2F17.UseVisualStyleBackColor = true;
            this.btnS2F17.Click += new System.EventHandler(this.btnS2F17_Click);
            // 
            // pageAlarm
            // 
            this.pageAlarm.Controls.Add(this.lbSendTerminalMSG);
            this.pageAlarm.Controls.Add(this.rtbSendTerminalMSG);
            this.pageAlarm.Controls.Add(this.rtbRecieveTerminalMSG);
            this.pageAlarm.Controls.Add(this.lbReceiveTerminalMSG);
            this.pageAlarm.Controls.Add(this.lbAlarm);
            this.pageAlarm.Controls.Add(this.listAlarmID);
            this.pageAlarm.Controls.Add(this.btnAlarmSetTest);
            this.pageAlarm.Controls.Add(this.btnTerminalMessage);
            this.pageAlarm.Controls.Add(this.btnAlarmresetTest);
            this.pageAlarm.Location = new System.Drawing.Point(4, 25);
            this.pageAlarm.Name = "pageAlarm";
            this.pageAlarm.Padding = new System.Windows.Forms.Padding(3);
            this.pageAlarm.Size = new System.Drawing.Size(785, 532);
            this.pageAlarm.TabIndex = 1;
            this.pageAlarm.Text = "Alarm & Terminal";
            this.pageAlarm.UseVisualStyleBackColor = true;
            // 
            // lbSendTerminalMSG
            // 
            this.lbSendTerminalMSG.AutoSize = true;
            this.lbSendTerminalMSG.Location = new System.Drawing.Point(360, 266);
            this.lbSendTerminalMSG.Name = "lbSendTerminalMSG";
            this.lbSendTerminalMSG.Size = new System.Drawing.Size(62, 16);
            this.lbSendTerminalMSG.TabIndex = 30;
            this.lbSendTerminalMSG.Text = "sent MSG";
            // 
            // rtbSendTerminalMSG
            // 
            this.rtbSendTerminalMSG.Location = new System.Drawing.Point(360, 285);
            this.rtbSendTerminalMSG.Name = "rtbSendTerminalMSG";
            this.rtbSendTerminalMSG.Size = new System.Drawing.Size(316, 168);
            this.rtbSendTerminalMSG.TabIndex = 29;
            this.rtbSendTerminalMSG.Text = "Terminal Message!!";
            // 
            // rtbRecieveTerminalMSG
            // 
            this.rtbRecieveTerminalMSG.Location = new System.Drawing.Point(19, 285);
            this.rtbRecieveTerminalMSG.Name = "rtbRecieveTerminalMSG";
            this.rtbRecieveTerminalMSG.ReadOnly = true;
            this.rtbRecieveTerminalMSG.Size = new System.Drawing.Size(335, 168);
            this.rtbRecieveTerminalMSG.TabIndex = 28;
            this.rtbRecieveTerminalMSG.Text = "";
            // 
            // lbReceiveTerminalMSG
            // 
            this.lbReceiveTerminalMSG.AutoSize = true;
            this.lbReceiveTerminalMSG.Location = new System.Drawing.Point(16, 266);
            this.lbReceiveTerminalMSG.Name = "lbReceiveTerminalMSG";
            this.lbReceiveTerminalMSG.Size = new System.Drawing.Size(79, 16);
            this.lbReceiveTerminalMSG.TabIndex = 27;
            this.lbReceiveTerminalMSG.Text = "receive MSG";
            // 
            // lbAlarm
            // 
            this.lbAlarm.AutoSize = true;
            this.lbAlarm.Location = new System.Drawing.Point(14, 16);
            this.lbAlarm.Name = "lbAlarm";
            this.lbAlarm.Size = new System.Drawing.Size(62, 16);
            this.lbAlarm.TabIndex = 25;
            this.lbAlarm.Text = "Alarm List";
            // 
            // listAlarmID
            // 
            this.listAlarmID.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listAlarmID.FormattingEnabled = true;
            this.listAlarmID.Location = new System.Drawing.Point(17, 35);
            this.listAlarmID.Name = "listAlarmID";
            this.listAlarmID.Size = new System.Drawing.Size(337, 226);
            this.listAlarmID.TabIndex = 24;
            this.listAlarmID.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listAlarmID_DrawItem);
            // 
            // pageEvent
            // 
            this.pageEvent.Controls.Add(this.cbAnnotated);
            this.pageEvent.Controls.Add(this.btnSetEvent);
            this.pageEvent.Controls.Add(this.listVIDItem);
            this.pageEvent.Controls.Add(this.lbVIDItem);
            this.pageEvent.Controls.Add(this.listReportItem);
            this.pageEvent.Controls.Add(this.lbReportItem);
            this.pageEvent.Controls.Add(this.listEventItem);
            this.pageEvent.Controls.Add(this.lbEventItem);
            this.pageEvent.Location = new System.Drawing.Point(4, 25);
            this.pageEvent.Name = "pageEvent";
            this.pageEvent.Padding = new System.Windows.Forms.Padding(3);
            this.pageEvent.Size = new System.Drawing.Size(785, 532);
            this.pageEvent.TabIndex = 2;
            this.pageEvent.Text = "Event";
            this.pageEvent.UseVisualStyleBackColor = true;
            // 
            // cbAnnotated
            // 
            this.cbAnnotated.AutoSize = true;
            this.cbAnnotated.Location = new System.Drawing.Point(22, 249);
            this.cbAnnotated.Name = "cbAnnotated";
            this.cbAnnotated.Size = new System.Drawing.Size(79, 20);
            this.cbAnnotated.TabIndex = 7;
            this.cbAnnotated.Text = "Annotate";
            this.cbAnnotated.UseVisualStyleBackColor = true;
            // 
            // btnSetEvent
            // 
            this.btnSetEvent.Location = new System.Drawing.Point(115, 249);
            this.btnSetEvent.Name = "btnSetEvent";
            this.btnSetEvent.Size = new System.Drawing.Size(83, 37);
            this.btnSetEvent.TabIndex = 6;
            this.btnSetEvent.Text = "Set";
            this.btnSetEvent.UseVisualStyleBackColor = true;
            this.btnSetEvent.Click += new System.EventHandler(this.btnSetEvent_Click);
            // 
            // listVIDItem
            // 
            this.listVIDItem.FormattingEnabled = true;
            this.listVIDItem.ItemHeight = 16;
            this.listVIDItem.Location = new System.Drawing.Point(461, 31);
            this.listVIDItem.Name = "listVIDItem";
            this.listVIDItem.Size = new System.Drawing.Size(179, 212);
            this.listVIDItem.TabIndex = 5;
            // 
            // lbVIDItem
            // 
            this.lbVIDItem.AutoSize = true;
            this.lbVIDItem.Location = new System.Drawing.Point(458, 12);
            this.lbVIDItem.Name = "lbVIDItem";
            this.lbVIDItem.Size = new System.Drawing.Size(28, 16);
            this.lbVIDItem.TabIndex = 4;
            this.lbVIDItem.Text = "VID";
            // 
            // listReportItem
            // 
            this.listReportItem.FormattingEnabled = true;
            this.listReportItem.ItemHeight = 16;
            this.listReportItem.Location = new System.Drawing.Point(242, 31);
            this.listReportItem.Name = "listReportItem";
            this.listReportItem.Size = new System.Drawing.Size(179, 212);
            this.listReportItem.TabIndex = 3;
            this.listReportItem.SelectedIndexChanged += new System.EventHandler(this.listReportItem_SelectedIndexChanged);
            // 
            // lbReportItem
            // 
            this.lbReportItem.AutoSize = true;
            this.lbReportItem.Location = new System.Drawing.Point(239, 12);
            this.lbReportItem.Name = "lbReportItem";
            this.lbReportItem.Size = new System.Drawing.Size(62, 16);
            this.lbReportItem.TabIndex = 2;
            this.lbReportItem.Text = "Report ID";
            // 
            // listEventItem
            // 
            this.listEventItem.FormattingEnabled = true;
            this.listEventItem.ItemHeight = 16;
            this.listEventItem.Location = new System.Drawing.Point(19, 31);
            this.listEventItem.Name = "listEventItem";
            this.listEventItem.Size = new System.Drawing.Size(179, 212);
            this.listEventItem.TabIndex = 1;
            this.listEventItem.SelectedIndexChanged += new System.EventHandler(this.listEventItem_SelectedIndexChanged);
            // 
            // lbEventItem
            // 
            this.lbEventItem.AutoSize = true;
            this.lbEventItem.Location = new System.Drawing.Point(16, 12);
            this.lbEventItem.Name = "lbEventItem";
            this.lbEventItem.Size = new System.Drawing.Size(54, 16);
            this.lbEventItem.TabIndex = 0;
            this.lbEventItem.Text = "Event ID";
            // 
            // pageCustomizeSetup
            // 
            this.pageCustomizeSetup.Controls.Add(this.rtbCustomize);
            this.pageCustomizeSetup.Location = new System.Drawing.Point(4, 25);
            this.pageCustomizeSetup.Name = "pageCustomizeSetup";
            this.pageCustomizeSetup.Padding = new System.Windows.Forms.Padding(3);
            this.pageCustomizeSetup.Size = new System.Drawing.Size(785, 532);
            this.pageCustomizeSetup.TabIndex = 3;
            this.pageCustomizeSetup.Text = "Customize";
            this.pageCustomizeSetup.UseVisualStyleBackColor = true;
            // 
            // rtbCustomize
            // 
            this.rtbCustomize.Location = new System.Drawing.Point(6, 6);
            this.rtbCustomize.Name = "rtbCustomize";
            this.rtbCustomize.Size = new System.Drawing.Size(452, 446);
            this.rtbCustomize.TabIndex = 0;
            this.rtbCustomize.Text = "";
            // 
            // pageCMS
            // 
            this.pageCMS.Controls.Add(this.btnE87NR3);
            this.pageCMS.Controls.Add(this.btnE87NR1);
            this.pageCMS.Controls.Add(this.btnLP2);
            this.pageCMS.Controls.Add(this.btnLP1);
            this.pageCMS.Controls.Add(this.ctrlCMS);
            this.pageCMS.Controls.Add(this.lbNowLoadPort);
            this.pageCMS.Controls.Add(this.shapeContainer4);
            this.pageCMS.Location = new System.Drawing.Point(4, 25);
            this.pageCMS.Name = "pageCMS";
            this.pageCMS.Padding = new System.Windows.Forms.Padding(3);
            this.pageCMS.Size = new System.Drawing.Size(785, 532);
            this.pageCMS.TabIndex = 4;
            this.pageCMS.Text = "CMS(E87)";
            this.pageCMS.UseVisualStyleBackColor = true;
            // 
            // btnE87NR1
            // 
            this.btnE87NR1.Location = new System.Drawing.Point(321, 10);
            this.btnE87NR1.Name = "btnE87NR1";
            this.btnE87NR1.Size = new System.Drawing.Size(100, 28);
            this.btnE87NR1.TabIndex = 2;
            this.btnE87NR1.Text = "RoundTrip1";
            this.btnE87NR1.UseVisualStyleBackColor = true;
            this.btnE87NR1.Click += new System.EventHandler(this.btnE87NR1_Click);
            // 
            // btnLP2
            // 
            this.btnLP2.Location = new System.Drawing.Point(218, 10);
            this.btnLP2.Name = "btnLP2";
            this.btnLP2.Size = new System.Drawing.Size(86, 49);
            this.btnLP2.TabIndex = 16;
            this.btnLP2.Text = "LP2";
            this.btnLP2.UseVisualStyleBackColor = true;
            this.btnLP2.Click += new System.EventHandler(this.btnLP2_Click);
            // 
            // btnLP1
            // 
            this.btnLP1.Enabled = false;
            this.btnLP1.Location = new System.Drawing.Point(102, 10);
            this.btnLP1.Name = "btnLP1";
            this.btnLP1.Size = new System.Drawing.Size(86, 49);
            this.btnLP1.TabIndex = 15;
            this.btnLP1.Text = "LP1";
            this.btnLP1.UseVisualStyleBackColor = true;
            this.btnLP1.Click += new System.EventHandler(this.btnLP1_Click);
            // 
            // ctrlCMS
            // 
            this.ctrlCMS.Controls.Add(this.pageLPT);
            this.ctrlCMS.Controls.Add(this.pageAccessMode);
            this.ctrlCMS.Controls.Add(this.pageAssociated);
            this.ctrlCMS.Controls.Add(this.pageCarrierRequest);
            this.ctrlCMS.Controls.Add(this.pageCarrierStatus);
            this.ctrlCMS.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ctrlCMS.Location = new System.Drawing.Point(3, 77);
            this.ctrlCMS.Name = "ctrlCMS";
            this.ctrlCMS.SelectedIndex = 0;
            this.ctrlCMS.Size = new System.Drawing.Size(779, 452);
            this.ctrlCMS.TabIndex = 0;
            // 
            // pageLPT
            // 
            this.pageLPT.Controls.Add(this.lbShowpreviousLPTS);
            this.pageLPT.Controls.Add(this.lbShowCurrentLPTS);
            this.pageLPT.Controls.Add(this.btnReadyToUnload);
            this.pageLPT.Controls.Add(this.btnReadyToLoad);
            this.pageLPT.Controls.Add(this.btnTranferBlocked);
            this.pageLPT.Controls.Add(this.btnTransferReady);
            this.pageLPT.Controls.Add(this.btnOutOfService);
            this.pageLPT.Controls.Add(this.btnInService);
            this.pageLPT.Controls.Add(this.lbCurrentLPTS);
            this.pageLPT.Controls.Add(this.lbPreviousLPTS);
            this.pageLPT.Controls.Add(this.picArrow);
            this.pageLPT.Controls.Add(this.shapeContainer1);
            this.pageLPT.Controls.Add(this.picLPTSM);
            this.pageLPT.Location = new System.Drawing.Point(4, 25);
            this.pageLPT.Name = "pageLPT";
            this.pageLPT.Padding = new System.Windows.Forms.Padding(3);
            this.pageLPT.Size = new System.Drawing.Size(771, 423);
            this.pageLPT.TabIndex = 0;
            this.pageLPT.Text = "Load Port Transfer";
            this.pageLPT.UseVisualStyleBackColor = true;
            // 
            // lbShowpreviousLPTS
            // 
            this.lbShowpreviousLPTS.AutoSize = true;
            this.lbShowpreviousLPTS.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbShowpreviousLPTS.Location = new System.Drawing.Point(437, 12);
            this.lbShowpreviousLPTS.Name = "lbShowpreviousLPTS";
            this.lbShowpreviousLPTS.Size = new System.Drawing.Size(60, 16);
            this.lbShowpreviousLPTS.TabIndex = 13;
            this.lbShowpreviousLPTS.Text = "Previous";
            // 
            // lbShowCurrentLPTS
            // 
            this.lbShowCurrentLPTS.AutoSize = true;
            this.lbShowCurrentLPTS.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbShowCurrentLPTS.Location = new System.Drawing.Point(437, 60);
            this.lbShowCurrentLPTS.Name = "lbShowCurrentLPTS";
            this.lbShowCurrentLPTS.Size = new System.Drawing.Size(54, 16);
            this.lbShowCurrentLPTS.TabIndex = 12;
            this.lbShowCurrentLPTS.Text = "Current";
            // 
            // btnReadyToUnload
            // 
            this.btnReadyToUnload.Enabled = false;
            this.btnReadyToUnload.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnReadyToUnload.Location = new System.Drawing.Point(623, 242);
            this.btnReadyToUnload.Name = "btnReadyToUnload";
            this.btnReadyToUnload.Size = new System.Drawing.Size(120, 40);
            this.btnReadyToUnload.TabIndex = 11;
            this.btnReadyToUnload.Text = "Ready To Unload";
            this.btnReadyToUnload.UseVisualStyleBackColor = true;
            this.btnReadyToUnload.Click += new System.EventHandler(this.btnReadyToUnload_Click);
            // 
            // btnReadyToLoad
            // 
            this.btnReadyToLoad.Enabled = false;
            this.btnReadyToLoad.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnReadyToLoad.Location = new System.Drawing.Point(497, 242);
            this.btnReadyToLoad.Name = "btnReadyToLoad";
            this.btnReadyToLoad.Size = new System.Drawing.Size(120, 40);
            this.btnReadyToLoad.TabIndex = 10;
            this.btnReadyToLoad.Text = "Ready To Load";
            this.btnReadyToLoad.UseVisualStyleBackColor = true;
            this.btnReadyToLoad.Click += new System.EventHandler(this.btnReadyToLoad_Click);
            // 
            // btnTranferBlocked
            // 
            this.btnTranferBlocked.Enabled = false;
            this.btnTranferBlocked.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnTranferBlocked.Location = new System.Drawing.Point(559, 288);
            this.btnTranferBlocked.Name = "btnTranferBlocked";
            this.btnTranferBlocked.Size = new System.Drawing.Size(120, 40);
            this.btnTranferBlocked.TabIndex = 9;
            this.btnTranferBlocked.Text = "Tranfer Blocked ";
            this.btnTranferBlocked.UseVisualStyleBackColor = true;
            this.btnTranferBlocked.Click += new System.EventHandler(this.btnTranferBlocked_Click);
            // 
            // btnTransferReady
            // 
            this.btnTransferReady.Enabled = false;
            this.btnTransferReady.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnTransferReady.Location = new System.Drawing.Point(559, 191);
            this.btnTransferReady.Name = "btnTransferReady";
            this.btnTransferReady.Size = new System.Drawing.Size(120, 40);
            this.btnTransferReady.TabIndex = 8;
            this.btnTransferReady.Text = "Transfer Ready";
            this.btnTransferReady.UseVisualStyleBackColor = true;
            this.btnTransferReady.Click += new System.EventHandler(this.btnTransferReady_Click);
            // 
            // btnOutOfService
            // 
            this.btnOutOfService.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOutOfService.Location = new System.Drawing.Point(623, 142);
            this.btnOutOfService.Name = "btnOutOfService";
            this.btnOutOfService.Size = new System.Drawing.Size(120, 40);
            this.btnOutOfService.TabIndex = 7;
            this.btnOutOfService.Text = "Out Of Service";
            this.btnOutOfService.UseVisualStyleBackColor = true;
            this.btnOutOfService.Click += new System.EventHandler(this.btnOutOfService_Click);
            // 
            // btnInService
            // 
            this.btnInService.BackColor = System.Drawing.Color.Transparent;
            this.btnInService.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnInService.Location = new System.Drawing.Point(497, 142);
            this.btnInService.Name = "btnInService";
            this.btnInService.Size = new System.Drawing.Size(120, 40);
            this.btnInService.TabIndex = 6;
            this.btnInService.Text = "In Service";
            this.btnInService.UseVisualStyleBackColor = false;
            this.btnInService.Click += new System.EventHandler(this.btnInService_Click);
            // 
            // lbCurrentLPTS
            // 
            this.lbCurrentLPTS.BackColor = System.Drawing.Color.LightGreen;
            this.lbCurrentLPTS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbCurrentLPTS.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbCurrentLPTS.Location = new System.Drawing.Point(497, 60);
            this.lbCurrentLPTS.Name = "lbCurrentLPTS";
            this.lbCurrentLPTS.Size = new System.Drawing.Size(246, 55);
            this.lbCurrentLPTS.TabIndex = 5;
            this.lbCurrentLPTS.Text = "NA";
            this.lbCurrentLPTS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPreviousLPTS
            // 
            this.lbPreviousLPTS.BackColor = System.Drawing.Color.LightGreen;
            this.lbPreviousLPTS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPreviousLPTS.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbPreviousLPTS.ForeColor = System.Drawing.Color.DarkGray;
            this.lbPreviousLPTS.Location = new System.Drawing.Point(497, 12);
            this.lbPreviousLPTS.Name = "lbPreviousLPTS";
            this.lbPreviousLPTS.Size = new System.Drawing.Size(197, 36);
            this.lbPreviousLPTS.TabIndex = 4;
            this.lbPreviousLPTS.Text = "NA";
            this.lbPreviousLPTS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picArrow
            // 
            this.picArrow.Image = ((System.Drawing.Image)(resources.GetObject("picArrow.Image")));
            this.picArrow.Location = new System.Drawing.Point(706, 12);
            this.picArrow.Name = "picArrow";
            this.picArrow.Size = new System.Drawing.Size(37, 36);
            this.picArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picArrow.TabIndex = 3;
            this.picArrow.TabStop = false;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 3);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectReadyToUpload,
            this.rectReadyToLoad,
            this.rectTransferReady,
            this.rectTranferBlacked,
            this.rectInService,
            this.rectOutOfService});
            this.shapeContainer1.Size = new System.Drawing.Size(765, 417);
            this.shapeContainer1.TabIndex = 14;
            this.shapeContainer1.TabStop = false;
            // 
            // rectReadyToUpload
            // 
            this.rectReadyToUpload.BorderColor = System.Drawing.Color.Red;
            this.rectReadyToUpload.BorderWidth = 3;
            this.rectReadyToUpload.Location = new System.Drawing.Point(280, 163);
            this.rectReadyToUpload.Name = "rectReadyToUpload";
            this.rectReadyToUpload.Size = new System.Drawing.Size(64, 64);
            this.rectReadyToUpload.Visible = false;
            // 
            // rectReadyToLoad
            // 
            this.rectReadyToLoad.BorderColor = System.Drawing.Color.Red;
            this.rectReadyToLoad.BorderWidth = 3;
            this.rectReadyToLoad.Location = new System.Drawing.Point(78, 163);
            this.rectReadyToLoad.Name = "rectReadyToLoad";
            this.rectReadyToLoad.Size = new System.Drawing.Size(64, 64);
            this.rectReadyToLoad.Visible = false;
            // 
            // rectTransferReady
            // 
            this.rectTransferReady.BorderColor = System.Drawing.Color.Red;
            this.rectTransferReady.BorderWidth = 3;
            this.rectTransferReady.Location = new System.Drawing.Point(49, 154);
            this.rectTransferReady.Name = "rectTransferReady";
            this.rectTransferReady.Size = new System.Drawing.Size(323, 87);
            this.rectTransferReady.Visible = false;
            // 
            // rectTranferBlacked
            // 
            this.rectTranferBlacked.BorderColor = System.Drawing.Color.Red;
            this.rectTranferBlacked.BorderWidth = 3;
            this.rectTranferBlacked.Location = new System.Drawing.Point(51, 266);
            this.rectTranferBlacked.Name = "rectTranferBlacked";
            this.rectTranferBlacked.Size = new System.Drawing.Size(319, 37);
            this.rectTranferBlacked.Visible = false;
            // 
            // rectInService
            // 
            this.rectInService.BorderColor = System.Drawing.Color.Red;
            this.rectInService.BorderWidth = 3;
            this.rectInService.Location = new System.Drawing.Point(29, 124);
            this.rectInService.Name = "rectInService";
            this.rectInService.Size = new System.Drawing.Size(361, 194);
            this.rectInService.Visible = false;
            // 
            // rectOutOfService
            // 
            this.rectOutOfService.BorderColor = System.Drawing.Color.Red;
            this.rectOutOfService.BorderWidth = 3;
            this.rectOutOfService.Location = new System.Drawing.Point(29, 55);
            this.rectOutOfService.Name = "rectOutOfService";
            this.rectOutOfService.Size = new System.Drawing.Size(362, 33);
            this.rectOutOfService.Visible = false;
            // 
            // picLPTSM
            // 
            this.picLPTSM.ErrorImage = ((System.Drawing.Image)(resources.GetObject("picLPTSM.ErrorImage")));
            this.picLPTSM.Image = ((System.Drawing.Image)(resources.GetObject("picLPTSM.Image")));
            this.picLPTSM.InitialImage = ((System.Drawing.Image)(resources.GetObject("picLPTSM.InitialImage")));
            this.picLPTSM.Location = new System.Drawing.Point(-4, 3);
            this.picLPTSM.Name = "picLPTSM";
            this.picLPTSM.Size = new System.Drawing.Size(435, 348);
            this.picLPTSM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLPTSM.TabIndex = 0;
            this.picLPTSM.TabStop = false;
            // 
            // pageAccessMode
            // 
            this.pageAccessMode.Controls.Add(this.btnReserved);
            this.pageAccessMode.Controls.Add(this.btnNotReserved);
            this.pageAccessMode.Controls.Add(this.lbAccessMode);
            this.pageAccessMode.Controls.Add(this.btnAuto);
            this.pageAccessMode.Controls.Add(this.btnManual);
            this.pageAccessMode.Controls.Add(this.shapeContainer2);
            this.pageAccessMode.Controls.Add(this.picAccessMode);
            this.pageAccessMode.Controls.Add(this.picReservation);
            this.pageAccessMode.Location = new System.Drawing.Point(4, 25);
            this.pageAccessMode.Name = "pageAccessMode";
            this.pageAccessMode.Padding = new System.Windows.Forms.Padding(3);
            this.pageAccessMode.Size = new System.Drawing.Size(771, 423);
            this.pageAccessMode.TabIndex = 1;
            this.pageAccessMode.Text = "Access Mode & Reservation";
            this.pageAccessMode.UseVisualStyleBackColor = true;
            // 
            // btnReserved
            // 
            this.btnReserved.Location = new System.Drawing.Point(328, 272);
            this.btnReserved.Name = "btnReserved";
            this.btnReserved.Size = new System.Drawing.Size(95, 42);
            this.btnReserved.TabIndex = 6;
            this.btnReserved.Text = "Reserved";
            this.btnReserved.UseVisualStyleBackColor = true;
            this.btnReserved.Click += new System.EventHandler(this.btnReserved_Click);
            // 
            // btnNotReserved
            // 
            this.btnNotReserved.Location = new System.Drawing.Point(328, 226);
            this.btnNotReserved.Name = "btnNotReserved";
            this.btnNotReserved.Size = new System.Drawing.Size(95, 42);
            this.btnNotReserved.TabIndex = 5;
            this.btnNotReserved.Text = "Not Reserved";
            this.btnNotReserved.UseVisualStyleBackColor = true;
            this.btnNotReserved.Click += new System.EventHandler(this.btnNotReserved_Click);
            // 
            // lbAccessMode
            // 
            this.lbAccessMode.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbAccessMode.ForeColor = System.Drawing.Color.Blue;
            this.lbAccessMode.Location = new System.Drawing.Point(328, 18);
            this.lbAccessMode.Name = "lbAccessMode";
            this.lbAccessMode.Size = new System.Drawing.Size(95, 29);
            this.lbAccessMode.TabIndex = 4;
            this.lbAccessMode.Text = "no state";
            this.lbAccessMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(328, 98);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(95, 43);
            this.btnAuto.TabIndex = 2;
            this.btnAuto.Text = "Auto";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // btnManual
            // 
            this.btnManual.Location = new System.Drawing.Point(328, 49);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(95, 43);
            this.btnManual.TabIndex = 1;
            this.btnManual.Text = "Manual";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(3, 3);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectReserved,
            this.rectNotReserved,
            this.lineDenyAccessMode2,
            this.lineDenyAccessMode1,
            this.rectAuto,
            this.rectManual});
            this.shapeContainer2.Size = new System.Drawing.Size(765, 417);
            this.shapeContainer2.TabIndex = 3;
            this.shapeContainer2.TabStop = false;
            // 
            // rectReserved
            // 
            this.rectReserved.BorderColor = System.Drawing.Color.Red;
            this.rectReserved.BorderWidth = 3;
            this.rectReserved.Location = new System.Drawing.Point(518, 219);
            this.rectReserved.Name = "rectReserved";
            this.rectReserved.Size = new System.Drawing.Size(106, 69);
            this.rectReserved.Visible = false;
            // 
            // rectNotReserved
            // 
            this.rectNotReserved.BorderColor = System.Drawing.Color.Red;
            this.rectNotReserved.BorderWidth = 3;
            this.rectNotReserved.Location = new System.Drawing.Point(519, 96);
            this.rectNotReserved.Name = "rectNotReserved";
            this.rectNotReserved.Size = new System.Drawing.Size(103, 70);
            this.rectNotReserved.Visible = false;
            // 
            // lineDenyAccessMode2
            // 
            this.lineDenyAccessMode2.BorderColor = System.Drawing.Color.Red;
            this.lineDenyAccessMode2.BorderWidth = 3;
            this.lineDenyAccessMode2.Name = "lineDenyAccessMode2";
            this.lineDenyAccessMode2.Visible = false;
            this.lineDenyAccessMode2.X1 = 305;
            this.lineDenyAccessMode2.X2 = 18;
            this.lineDenyAccessMode2.Y1 = 15;
            this.lineDenyAccessMode2.Y2 = 310;
            // 
            // lineDenyAccessMode1
            // 
            this.lineDenyAccessMode1.BorderColor = System.Drawing.Color.Red;
            this.lineDenyAccessMode1.BorderWidth = 3;
            this.lineDenyAccessMode1.Name = "lineDenyAccessMode1";
            this.lineDenyAccessMode1.Visible = false;
            this.lineDenyAccessMode1.X1 = 19;
            this.lineDenyAccessMode1.X2 = 305;
            this.lineDenyAccessMode1.Y1 = 15;
            this.lineDenyAccessMode1.Y2 = 311;
            // 
            // rectAuto
            // 
            this.rectAuto.BorderColor = System.Drawing.Color.Red;
            this.rectAuto.BorderWidth = 3;
            this.rectAuto.Location = new System.Drawing.Point(107, 209);
            this.rectAuto.Name = "rectAuto";
            this.rectAuto.Size = new System.Drawing.Size(111, 69);
            this.rectAuto.Visible = false;
            // 
            // rectManual
            // 
            this.rectManual.BorderColor = System.Drawing.Color.Red;
            this.rectManual.BorderWidth = 3;
            this.rectManual.Location = new System.Drawing.Point(107, 97);
            this.rectManual.Name = "rectManual";
            this.rectManual.Size = new System.Drawing.Size(111, 66);
            this.rectManual.Visible = false;
            // 
            // picAccessMode
            // 
            this.picAccessMode.Image = ((System.Drawing.Image)(resources.GetObject("picAccessMode.Image")));
            this.picAccessMode.Location = new System.Drawing.Point(20, 17);
            this.picAccessMode.Name = "picAccessMode";
            this.picAccessMode.Size = new System.Drawing.Size(291, 299);
            this.picAccessMode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAccessMode.TabIndex = 0;
            this.picAccessMode.TabStop = false;
            // 
            // picReservation
            // 
            this.picReservation.Image = ((System.Drawing.Image)(resources.GetObject("picReservation.Image")));
            this.picReservation.Location = new System.Drawing.Point(437, 17);
            this.picReservation.Name = "picReservation";
            this.picReservation.Size = new System.Drawing.Size(269, 297);
            this.picReservation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picReservation.TabIndex = 0;
            this.picReservation.TabStop = false;
            // 
            // pageAssociated
            // 
            this.pageAssociated.Controls.Add(this.lbAssociatedCarrierID);
            this.pageAssociated.Controls.Add(this.lbShowCarrierID);
            this.pageAssociated.Controls.Add(this.shapeContainer3);
            this.pageAssociated.Controls.Add(this.picAssociated);
            this.pageAssociated.Location = new System.Drawing.Point(4, 25);
            this.pageAssociated.Name = "pageAssociated";
            this.pageAssociated.Padding = new System.Windows.Forms.Padding(3);
            this.pageAssociated.Size = new System.Drawing.Size(771, 423);
            this.pageAssociated.TabIndex = 3;
            this.pageAssociated.Text = "Associated";
            this.pageAssociated.UseVisualStyleBackColor = true;
            // 
            // lbAssociatedCarrierID
            // 
            this.lbAssociatedCarrierID.Font = new System.Drawing.Font("微軟正黑體", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbAssociatedCarrierID.ForeColor = System.Drawing.Color.Blue;
            this.lbAssociatedCarrierID.Location = new System.Drawing.Point(6, 26);
            this.lbAssociatedCarrierID.Name = "lbAssociatedCarrierID";
            this.lbAssociatedCarrierID.Size = new System.Drawing.Size(294, 36);
            this.lbAssociatedCarrierID.TabIndex = 6;
            this.lbAssociatedCarrierID.Text = "Carrier ID";
            this.lbAssociatedCarrierID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbShowCarrierID
            // 
            this.lbShowCarrierID.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbShowCarrierID.ForeColor = System.Drawing.Color.Black;
            this.lbShowCarrierID.Location = new System.Drawing.Point(6, 3);
            this.lbShowCarrierID.Name = "lbShowCarrierID";
            this.lbShowCarrierID.Size = new System.Drawing.Size(87, 23);
            this.lbShowCarrierID.TabIndex = 5;
            this.lbShowCarrierID.Text = "Carrier ID";
            this.lbShowCarrierID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer3
            // 
            this.shapeContainer3.Location = new System.Drawing.Point(3, 3);
            this.shapeContainer3.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer3.Name = "shapeContainer3";
            this.shapeContainer3.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectAssociated,
            this.rectNotAssociated});
            this.shapeContainer3.Size = new System.Drawing.Size(765, 417);
            this.shapeContainer3.TabIndex = 1;
            this.shapeContainer3.TabStop = false;
            // 
            // rectAssociated
            // 
            this.rectAssociated.BorderColor = System.Drawing.Color.Red;
            this.rectAssociated.BorderWidth = 3;
            this.rectAssociated.Location = new System.Drawing.Point(101, 307);
            this.rectAssociated.Name = "rectAssociated";
            this.rectAssociated.Size = new System.Drawing.Size(111, 73);
            // 
            // rectNotAssociated
            // 
            this.rectNotAssociated.BorderColor = System.Drawing.Color.Red;
            this.rectNotAssociated.BorderWidth = 3;
            this.rectNotAssociated.Location = new System.Drawing.Point(100, 167);
            this.rectNotAssociated.Name = "rectNotAssociated";
            this.rectNotAssociated.Size = new System.Drawing.Size(112, 75);
            // 
            // picAssociated
            // 
            this.picAssociated.Image = ((System.Drawing.Image)(resources.GetObject("picAssociated.Image")));
            this.picAssociated.Location = new System.Drawing.Point(6, 73);
            this.picAssociated.Name = "picAssociated";
            this.picAssociated.Size = new System.Drawing.Size(294, 344);
            this.picAssociated.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAssociated.TabIndex = 0;
            this.picAssociated.TabStop = false;
            // 
            // pageCarrierRequest
            // 
            this.pageCarrierRequest.Controls.Add(this.btnAddCarrierID);
            this.pageCarrierRequest.Controls.Add(this.lbCID);
            this.pageCarrierRequest.Controls.Add(this.txtCarrierID);
            this.pageCarrierRequest.Controls.Add(this.btnClearRequest);
            this.pageCarrierRequest.Controls.Add(this.lbSecondRequest);
            this.pageCarrierRequest.Controls.Add(this.lbMainRequest);
            this.pageCarrierRequest.Controls.Add(this.rtxtSecondRequest);
            this.pageCarrierRequest.Controls.Add(this.rtxtMainRequest);
            this.pageCarrierRequest.Location = new System.Drawing.Point(4, 25);
            this.pageCarrierRequest.Name = "pageCarrierRequest";
            this.pageCarrierRequest.Padding = new System.Windows.Forms.Padding(3);
            this.pageCarrierRequest.Size = new System.Drawing.Size(771, 423);
            this.pageCarrierRequest.TabIndex = 4;
            this.pageCarrierRequest.Text = "Carrier Action Request";
            this.pageCarrierRequest.UseVisualStyleBackColor = true;
            // 
            // btnAddCarrierID
            // 
            this.btnAddCarrierID.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnAddCarrierID.Location = new System.Drawing.Point(665, 51);
            this.btnAddCarrierID.Name = "btnAddCarrierID";
            this.btnAddCarrierID.Size = new System.Drawing.Size(100, 52);
            this.btnAddCarrierID.TabIndex = 7;
            this.btnAddCarrierID.Text = "增加Carrier ID";
            this.btnAddCarrierID.UseVisualStyleBackColor = true;
            this.btnAddCarrierID.Click += new System.EventHandler(this.btnAddCarrierID_Click);
            // 
            // lbCID
            // 
            this.lbCID.AutoSize = true;
            this.lbCID.Location = new System.Drawing.Point(584, 22);
            this.lbCID.Name = "lbCID";
            this.lbCID.Size = new System.Drawing.Size(60, 16);
            this.lbCID.TabIndex = 6;
            this.lbCID.Text = "Carrier ID";
            // 
            // txtCarrierID
            // 
            this.txtCarrierID.Location = new System.Drawing.Point(665, 22);
            this.txtCarrierID.Name = "txtCarrierID";
            this.txtCarrierID.Size = new System.Drawing.Size(100, 23);
            this.txtCarrierID.TabIndex = 5;
            this.txtCarrierID.Text = "CSTID_01";
            // 
            // btnClearRequest
            // 
            this.btnClearRequest.Location = new System.Drawing.Point(690, 371);
            this.btnClearRequest.Name = "btnClearRequest";
            this.btnClearRequest.Size = new System.Drawing.Size(75, 46);
            this.btnClearRequest.TabIndex = 4;
            this.btnClearRequest.Text = "清除訊息";
            this.btnClearRequest.UseVisualStyleBackColor = true;
            this.btnClearRequest.Click += new System.EventHandler(this.btnClearRequest_Click);
            // 
            // lbSecondRequest
            // 
            this.lbSecondRequest.AutoSize = true;
            this.lbSecondRequest.Location = new System.Drawing.Point(293, 3);
            this.lbSecondRequest.Name = "lbSecondRequest";
            this.lbSecondRequest.Size = new System.Drawing.Size(56, 16);
            this.lbSecondRequest.TabIndex = 3;
            this.lbSecondRequest.Text = "次要訊息";
            // 
            // lbMainRequest
            // 
            this.lbMainRequest.AutoSize = true;
            this.lbMainRequest.Location = new System.Drawing.Point(9, 3);
            this.lbMainRequest.Name = "lbMainRequest";
            this.lbMainRequest.Size = new System.Drawing.Size(56, 16);
            this.lbMainRequest.TabIndex = 2;
            this.lbMainRequest.Text = "主要訊息";
            // 
            // rtxtSecondRequest
            // 
            this.rtxtSecondRequest.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rtxtSecondRequest.Location = new System.Drawing.Point(296, 22);
            this.rtxtSecondRequest.Name = "rtxtSecondRequest";
            this.rtxtSecondRequest.Size = new System.Drawing.Size(278, 395);
            this.rtxtSecondRequest.TabIndex = 1;
            this.rtxtSecondRequest.Text = "";
            this.rtxtSecondRequest.TextChanged += new System.EventHandler(this.rtxtSecondRequest_TextChanged);
            // 
            // rtxtMainRequest
            // 
            this.rtxtMainRequest.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rtxtMainRequest.Location = new System.Drawing.Point(12, 22);
            this.rtxtMainRequest.Name = "rtxtMainRequest";
            this.rtxtMainRequest.Size = new System.Drawing.Size(278, 395);
            this.rtxtMainRequest.TabIndex = 0;
            this.rtxtMainRequest.Text = "";
            this.rtxtMainRequest.TextChanged += new System.EventHandler(this.rtxtMainRequest_TextChanged);
            // 
            // pageCarrierStatus
            // 
            this.pageCarrierStatus.Controls.Add(this.btnIDVerifFail);
            this.pageCarrierStatus.Controls.Add(this.btnSlopMapVerifOK);
            this.pageCarrierStatus.Controls.Add(this.btnIDVerifOK);
            this.pageCarrierStatus.Controls.Add(this.shapeContainer5);
            this.pageCarrierStatus.Controls.Add(this.picCarrierStatus);
            this.pageCarrierStatus.Location = new System.Drawing.Point(4, 25);
            this.pageCarrierStatus.Name = "pageCarrierStatus";
            this.pageCarrierStatus.Padding = new System.Windows.Forms.Padding(3);
            this.pageCarrierStatus.Size = new System.Drawing.Size(771, 423);
            this.pageCarrierStatus.TabIndex = 2;
            this.pageCarrierStatus.Text = "Carrier Status";
            this.pageCarrierStatus.UseVisualStyleBackColor = true;
            // 
            // btnSlopMapVerifOK
            // 
            this.btnSlopMapVerifOK.Location = new System.Drawing.Point(615, 15);
            this.btnSlopMapVerifOK.Name = "btnSlopMapVerifOK";
            this.btnSlopMapVerifOK.Size = new System.Drawing.Size(137, 42);
            this.btnSlopMapVerifOK.TabIndex = 3;
            this.btnSlopMapVerifOK.Text = "Slot Map Verification OK";
            this.btnSlopMapVerifOK.UseVisualStyleBackColor = true;
            this.btnSlopMapVerifOK.Click += new System.EventHandler(this.btnSlopMapVerifOK_Click);
            // 
            // btnIDVerifOK
            // 
            this.btnIDVerifOK.Location = new System.Drawing.Point(20, 15);
            this.btnIDVerifOK.Name = "btnIDVerifOK";
            this.btnIDVerifOK.Size = new System.Drawing.Size(137, 42);
            this.btnIDVerifOK.TabIndex = 2;
            this.btnIDVerifOK.Text = "ID Verification OK";
            this.btnIDVerifOK.UseVisualStyleBackColor = true;
            this.btnIDVerifOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // shapeContainer5
            // 
            this.shapeContainer5.Location = new System.Drawing.Point(3, 3);
            this.shapeContainer5.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer5.Name = "shapeContainer5";
            this.shapeContainer5.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rtCarrierStopped,
            this.rtCarrierComplete,
            this.rtInAccessed,
            this.rtNotAccessed,
            this.rtSlopMapVerificationFail,
            this.rtSlopMapVerificationOK,
            this.rtWaitingForHostSlotMap,
            this.rtSlotMapNotRead,
            this.rtIDverificationFail,
            this.rtIDVerificationOK,
            this.rtWaitingForHostID,
            this.rtIDNotRead,
            this.rtCarrierAccessingStatus,
            this.rtCarrierSlotMapStatus,
            this.rtCarrierInStatus});
            this.shapeContainer5.Size = new System.Drawing.Size(765, 417);
            this.shapeContainer5.TabIndex = 1;
            this.shapeContainer5.TabStop = false;
            // 
            // rtCarrierStopped
            // 
            this.rtCarrierStopped.BorderColor = System.Drawing.Color.Red;
            this.rtCarrierStopped.BorderWidth = 3;
            this.rtCarrierStopped.Location = new System.Drawing.Point(612, 313);
            this.rtCarrierStopped.Name = "rtCarrierStopped";
            this.rtCarrierStopped.Size = new System.Drawing.Size(69, 38);
            this.rtCarrierStopped.Visible = false;
            // 
            // rtCarrierComplete
            // 
            this.rtCarrierComplete.BorderColor = System.Drawing.Color.Red;
            this.rtCarrierComplete.BorderWidth = 3;
            this.rtCarrierComplete.Location = new System.Drawing.Point(524, 313);
            this.rtCarrierComplete.Name = "rtCarrierComplete";
            this.rtCarrierComplete.Size = new System.Drawing.Size(68, 38);
            this.rtCarrierComplete.Visible = false;
            // 
            // rtInAccessed
            // 
            this.rtInAccessed.BorderColor = System.Drawing.Color.Red;
            this.rtInAccessed.BorderWidth = 3;
            this.rtInAccessed.Location = new System.Drawing.Point(570, 220);
            this.rtInAccessed.Name = "rtInAccessed";
            this.rtInAccessed.Size = new System.Drawing.Size(66, 38);
            this.rtInAccessed.Visible = false;
            // 
            // rtNotAccessed
            // 
            this.rtNotAccessed.BorderColor = System.Drawing.Color.Red;
            this.rtNotAccessed.BorderWidth = 3;
            this.rtNotAccessed.Location = new System.Drawing.Point(570, 143);
            this.rtNotAccessed.Name = "rtNotAccessed";
            this.rtNotAccessed.Size = new System.Drawing.Size(66, 38);
            this.rtNotAccessed.Visible = false;
            // 
            // rtSlopMapVerificationFail
            // 
            this.rtSlopMapVerificationFail.BorderColor = System.Drawing.Color.Red;
            this.rtSlopMapVerificationFail.BorderWidth = 3;
            this.rtSlopMapVerificationFail.Location = new System.Drawing.Point(417, 313);
            this.rtSlopMapVerificationFail.Name = "rtSlopMapVerificationFail";
            this.rtSlopMapVerificationFail.Size = new System.Drawing.Size(85, 39);
            this.rtSlopMapVerificationFail.Visible = false;
            // 
            // rtSlopMapVerificationOK
            // 
            this.rtSlopMapVerificationOK.BorderColor = System.Drawing.Color.Red;
            this.rtSlopMapVerificationOK.BorderWidth = 3;
            this.rtSlopMapVerificationOK.Location = new System.Drawing.Point(325, 313);
            this.rtSlopMapVerificationOK.Name = "rtSlopMapVerificationOK";
            this.rtSlopMapVerificationOK.Size = new System.Drawing.Size(84, 39);
            this.rtSlopMapVerificationOK.Visible = false;
            // 
            // rtWaitingForHostSlotMap
            // 
            this.rtWaitingForHostSlotMap.BorderColor = System.Drawing.Color.Red;
            this.rtWaitingForHostSlotMap.BorderWidth = 3;
            this.rtWaitingForHostSlotMap.Location = new System.Drawing.Point(381, 220);
            this.rtWaitingForHostSlotMap.Name = "rtWaitingForHostSlotMap";
            this.rtWaitingForHostSlotMap.Size = new System.Drawing.Size(62, 39);
            this.rtWaitingForHostSlotMap.Visible = false;
            // 
            // rtSlotMapNotRead
            // 
            this.rtSlotMapNotRead.BorderColor = System.Drawing.Color.Red;
            this.rtSlotMapNotRead.BorderWidth = 3;
            this.rtSlotMapNotRead.Location = new System.Drawing.Point(381, 134);
            this.rtSlotMapNotRead.Name = "rtSlotMapNotRead";
            this.rtSlotMapNotRead.Size = new System.Drawing.Size(62, 39);
            this.rtSlotMapNotRead.Visible = false;
            // 
            // rtIDverificationFail
            // 
            this.rtIDverificationFail.BorderColor = System.Drawing.Color.Red;
            this.rtIDverificationFail.BorderWidth = 3;
            this.rtIDverificationFail.Location = new System.Drawing.Point(217, 313);
            this.rtIDverificationFail.Name = "rtIDverificationFail";
            this.rtIDverificationFail.Size = new System.Drawing.Size(79, 39);
            this.rtIDverificationFail.Visible = false;
            // 
            // rtIDVerificationOK
            // 
            this.rtIDVerificationOK.BorderColor = System.Drawing.Color.Red;
            this.rtIDVerificationOK.BorderWidth = 3;
            this.rtIDVerificationOK.Location = new System.Drawing.Point(97, 313);
            this.rtIDVerificationOK.Name = "rtIDVerificationOK";
            this.rtIDVerificationOK.Size = new System.Drawing.Size(77, 39);
            this.rtIDVerificationOK.Visible = false;
            // 
            // rtWaitingForHostID
            // 
            this.rtWaitingForHostID.BorderColor = System.Drawing.Color.Red;
            this.rtWaitingForHostID.BorderWidth = 3;
            this.rtWaitingForHostID.Location = new System.Drawing.Point(164, 220);
            this.rtWaitingForHostID.Name = "rtWaitingForHostID";
            this.rtWaitingForHostID.Size = new System.Drawing.Size(61, 39);
            this.rtWaitingForHostID.Visible = false;
            // 
            // rtIDNotRead
            // 
            this.rtIDNotRead.BorderColor = System.Drawing.Color.Red;
            this.rtIDNotRead.BorderWidth = 3;
            this.rtIDNotRead.Location = new System.Drawing.Point(169, 141);
            this.rtIDNotRead.Name = "rtIDNotRead";
            this.rtIDNotRead.Size = new System.Drawing.Size(53, 27);
            this.rtIDNotRead.Visible = false;
            // 
            // rtCarrierAccessingStatus
            // 
            this.rtCarrierAccessingStatus.BorderColor = System.Drawing.Color.Red;
            this.rtCarrierAccessingStatus.BorderWidth = 3;
            this.rtCarrierAccessingStatus.Location = new System.Drawing.Point(512, 75);
            this.rtCarrierAccessingStatus.Name = "rtCarrierAccessingStatus";
            this.rtCarrierAccessingStatus.Size = new System.Drawing.Size(183, 306);
            this.rtCarrierAccessingStatus.Visible = false;
            // 
            // rtCarrierSlotMapStatus
            // 
            this.rtCarrierSlotMapStatus.BorderColor = System.Drawing.Color.Red;
            this.rtCarrierSlotMapStatus.BorderWidth = 3;
            this.rtCarrierSlotMapStatus.Location = new System.Drawing.Point(311, 75);
            this.rtCarrierSlotMapStatus.Name = "rtCarrierSlotMapStatus";
            this.rtCarrierSlotMapStatus.Size = new System.Drawing.Size(201, 306);
            this.rtCarrierSlotMapStatus.Visible = false;
            // 
            // rtCarrierInStatus
            // 
            this.rtCarrierInStatus.BorderColor = System.Drawing.Color.Red;
            this.rtCarrierInStatus.BorderWidth = 3;
            this.rtCarrierInStatus.Location = new System.Drawing.Point(73, 75);
            this.rtCarrierInStatus.Name = "rtCarrierInStatus";
            this.rtCarrierInStatus.Size = new System.Drawing.Size(238, 306);
            this.rtCarrierInStatus.Visible = false;
            // 
            // picCarrierStatus
            // 
            this.picCarrierStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCarrierStatus.Image = ((System.Drawing.Image)(resources.GetObject("picCarrierStatus.Image")));
            this.picCarrierStatus.Location = new System.Drawing.Point(3, 3);
            this.picCarrierStatus.Name = "picCarrierStatus";
            this.picCarrierStatus.Size = new System.Drawing.Size(765, 417);
            this.picCarrierStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCarrierStatus.TabIndex = 0;
            this.picCarrierStatus.TabStop = false;
            // 
            // lbNowLoadPort
            // 
            this.lbNowLoadPort.Font = new System.Drawing.Font("微軟正黑體", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbNowLoadPort.Location = new System.Drawing.Point(6, 3);
            this.lbNowLoadPort.Name = "lbNowLoadPort";
            this.lbNowLoadPort.Size = new System.Drawing.Size(87, 60);
            this.lbNowLoadPort.TabIndex = 17;
            this.lbNowLoadPort.Text = "LP1";
            this.lbNowLoadPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer4
            // 
            this.shapeContainer4.Location = new System.Drawing.Point(3, 3);
            this.shapeContainer4.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer4.Name = "shapeContainer4";
            this.shapeContainer4.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectLP2,
            this.rectLP1});
            this.shapeContainer4.Size = new System.Drawing.Size(779, 526);
            this.shapeContainer4.TabIndex = 18;
            this.shapeContainer4.TabStop = false;
            // 
            // rectLP2
            // 
            this.rectLP2.FillColor = System.Drawing.Color.Blue;
            this.rectLP2.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.rectLP2.Location = new System.Drawing.Point(209, 3);
            this.rectLP2.Name = "rectLP2";
            this.rectLP2.Size = new System.Drawing.Size(97, 59);
            // 
            // rectLP1
            // 
            this.rectLP1.FillColor = System.Drawing.Color.White;
            this.rectLP1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.rectLP1.Location = new System.Drawing.Point(90, 2);
            this.rectLP1.Name = "rectLP1";
            this.rectLP1.Size = new System.Drawing.Size(103, 59);
            // 
            // timeUIUpdate
            // 
            this.timeUIUpdate.Tick += new System.EventHandler(this.timeUIUpdate_Tick);
            // 
            // btnIDVerifFail
            // 
            this.btnIDVerifFail.Location = new System.Drawing.Point(167, 15);
            this.btnIDVerifFail.Name = "btnIDVerifFail";
            this.btnIDVerifFail.Size = new System.Drawing.Size(137, 42);
            this.btnIDVerifFail.TabIndex = 4;
            this.btnIDVerifFail.Text = "ID Verification Fail";
            this.btnIDVerifFail.UseVisualStyleBackColor = true;
            this.btnIDVerifFail.Click += new System.EventHandler(this.btnIDVerifFail_Click);
            // 
            // btnE87NR3
            // 
            this.btnE87NR3.Location = new System.Drawing.Point(427, 10);
            this.btnE87NR3.Name = "btnE87NR3";
            this.btnE87NR3.Size = new System.Drawing.Size(100, 28);
            this.btnE87NR3.TabIndex = 19;
            this.btnE87NR3.Text = "RoundTrip3";
            this.btnE87NR3.UseVisualStyleBackColor = true;
            this.btnE87NR3.Click += new System.EventHandler(this.btnE87NR3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 677);
            this.Controls.Add(this.tbShowSML);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.comSVID);
            this.Controls.Add(this.lbSVID);
            this.Controls.Add(this.btnSent);
            this.Controls.Add(this.pnLeftFrame);
            this.Controls.Add(this.lbConnectStateChange);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "SecsDll Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Leave += new System.EventHandler(this.Form1_Leave);
            this.pnLeftFrame.ResumeLayout(false);
            this.pnLeftFrame.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.pageGeneral.ResumeLayout(false);
            this.pageGeneral.PerformLayout();
            this.gbCommand.ResumeLayout(false);
            this.pageAlarm.ResumeLayout(false);
            this.pageAlarm.PerformLayout();
            this.pageEvent.ResumeLayout(false);
            this.pageEvent.PerformLayout();
            this.pageCustomizeSetup.ResumeLayout(false);
            this.pageCMS.ResumeLayout(false);
            this.ctrlCMS.ResumeLayout(false);
            this.pageLPT.ResumeLayout(false);
            this.pageLPT.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLPTSM)).EndInit();
            this.pageAccessMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAccessMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReservation)).EndInit();
            this.pageAssociated.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAssociated)).EndInit();
            this.pageCarrierRequest.ResumeLayout(false);
            this.pageCarrierRequest.PerformLayout();
            this.pageCarrierStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCarrierStatus)).EndInit();
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
        private System.Windows.Forms.Button btnEqpOnLineLocal;
        private System.Windows.Forms.Button btnEqpOnLineRemote;
        private System.Windows.Forms.Button btnAlarmSetTest;
        private System.Windows.Forms.Button btnAlarmresetTest;
        private System.Windows.Forms.Button btnTerminalMessage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage pageGeneral;
        private System.Windows.Forms.TabPage pageAlarm;
        private System.Windows.Forms.Label lbAlarm;
        private System.Windows.Forms.ListBox listAlarmID;
        private System.Windows.Forms.Label lbReceiveTerminalMSG;
        private System.Windows.Forms.RichTextBox rtbRecieveTerminalMSG;
        private System.Windows.Forms.Label lbSendTerminalMSG;
        private System.Windows.Forms.RichTextBox rtbSendTerminalMSG;
        private System.Windows.Forms.GroupBox gbCommand;
        private System.Windows.Forms.Button btnS2F17;
        private System.Windows.Forms.TabPage pageEvent;
        private System.Windows.Forms.Button btnSetEvent;
        private System.Windows.Forms.ListBox listVIDItem;
        private System.Windows.Forms.Label lbVIDItem;
        private System.Windows.Forms.ListBox listReportItem;
        private System.Windows.Forms.Label lbReportItem;
        private System.Windows.Forms.ListBox listEventItem;
        private System.Windows.Forms.Label lbEventItem;
        private System.Windows.Forms.TabPage pageCustomizeSetup;
        private System.Windows.Forms.RichTextBox rtbCustomize;
        private System.Windows.Forms.CheckBox cbAnnotated;
        private System.Windows.Forms.TabPage pageCMS;
        private System.Windows.Forms.TabControl ctrlCMS;
        private System.Windows.Forms.TabPage pageLPT;
        private System.Windows.Forms.PictureBox picLPTSM;
        private System.Windows.Forms.Label lbCurrentLPTS;
        private System.Windows.Forms.Label lbPreviousLPTS;
        private System.Windows.Forms.PictureBox picArrow;
        private System.Windows.Forms.Button btnReadyToUnload;
        private System.Windows.Forms.Button btnReadyToLoad;
        private System.Windows.Forms.Button btnTranferBlocked;
        private System.Windows.Forms.Button btnTransferReady;
        private System.Windows.Forms.Button btnOutOfService;
        private System.Windows.Forms.Button btnInService;
        private System.Windows.Forms.Label lbShowCurrentLPTS;
        private System.Windows.Forms.Label lbShowpreviousLPTS;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectReadyToUpload;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectReadyToLoad;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectTransferReady;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectTranferBlacked;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectInService;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectOutOfService;
        private System.Windows.Forms.Button btnLP2;
        private System.Windows.Forms.Button btnLP1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectLP2;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectLP1;
        private System.Windows.Forms.Label lbNowLoadPort;
        private System.Windows.Forms.TabPage pageAccessMode;
        private System.Windows.Forms.Label lbAccessMode;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Button btnManual;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectAuto;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectManual;
        private System.Windows.Forms.PictureBox picAccessMode;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer4;
        private System.Windows.Forms.TabPage pageCarrierStatus;
        private System.Windows.Forms.PictureBox picReservation;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineDenyAccessMode2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineDenyAccessMode1;
        private System.Windows.Forms.Button btnReserved;
        private System.Windows.Forms.Button btnNotReserved;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectReserved;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectNotReserved;
        private System.Windows.Forms.TabPage pageAssociated;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer3;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectAssociated;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectNotAssociated;
        private System.Windows.Forms.PictureBox picAssociated;
        private System.Windows.Forms.Label lbAssociatedCarrierID;
        private System.Windows.Forms.Label lbShowCarrierID;
        private System.Windows.Forms.TabPage pageCarrierRequest;
        private System.Windows.Forms.Button btnClearRequest;
        private System.Windows.Forms.Label lbSecondRequest;
        private System.Windows.Forms.Label lbMainRequest;
        private System.Windows.Forms.RichTextBox rtxtSecondRequest;
        private System.Windows.Forms.RichTextBox rtxtMainRequest;
        private System.Windows.Forms.Button btnAddCarrierID;
        private System.Windows.Forms.Label lbCID;
        private System.Windows.Forms.TextBox txtCarrierID;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer5;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtCarrierStopped;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtCarrierComplete;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtInAccessed;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtNotAccessed;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtSlopMapVerificationFail;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtSlopMapVerificationOK;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtWaitingForHostSlotMap;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtSlotMapNotRead;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtIDverificationFail;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtIDVerificationOK;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtWaitingForHostID;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtIDNotRead;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtCarrierAccessingStatus;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtCarrierSlotMapStatus;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rtCarrierInStatus;
        private System.Windows.Forms.PictureBox picCarrierStatus;
        private System.Windows.Forms.Timer timeUIUpdate;
        private System.Windows.Forms.Button btnE87NR1;
        private System.Windows.Forms.Button btnIDVerifOK;
        private System.Windows.Forms.Button btnSlopMapVerifOK;
        private System.Windows.Forms.Button btnIDVerifFail;
        private System.Windows.Forms.Button btnE87NR3;
    }
}

