using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SanwaSecsDll;
//using Secs4Net;
//using Secs4Net.Sml;



namespace TestForm
{
    public partial class Form1 : Form
    {
        SanwaSecs _secsGemTool;
        SecsLogger _secsLogger;
        SecsMessageList _secsMessagesList;

        readonly BindingList<PrimaryMessageWrapper> recvBuffer = new BindingList<PrimaryMessageWrapper>();

        public Dictionary<string, LoadPort> _loadPortList;

        bool NormalRoundtrip1Start = false;
        //EventWaitHandle ewhNormalRoundtrip1 = new EventWaitHandle(false, EventResetMode.ManualReset);

        bool NormalRoundtrip2Start = false;
        EventWaitHandle ewhNormalRoundtrip2 = new EventWaitHandle(false, EventResetMode.ManualReset);

        EventWaitHandle ewhSlotMapVerifOK = new EventWaitHandle(false, EventResetMode.ManualReset);

        enum eVerification
        {
            OK = 0,
            Fail
        };

        EventWaitHandle[] ewhEQPVerif = new EventWaitHandle[]
        {
            new EventWaitHandle(false, EventResetMode.AutoReset, "OK"),
            new EventWaitHandle(false, EventResetMode.AutoReset, "Fail")
        };

        EventWaitHandle[] ewhHostVerif = new EventWaitHandle[]
        {
            new EventWaitHandle(false, EventResetMode.AutoReset, "OK"),
            new EventWaitHandle(false, EventResetMode.AutoReset, "Fail")
        };

        EventWaitHandle ewhCarrierOut = new EventWaitHandle(false, EventResetMode.AutoReset, "CarrierOut");


        //E87
        public string E87_CE_LP1_LPTSM_SCT2_IN_SERVICE = "E87_CE_LP1_LPTSM_SCT2_IN_SERVICE";
        public string E87_CE_LP1_LPTSM_SCT3_OUT_OF_SERVICE = "E87_CE_LP1_LPTSM_SCT3_OUT_OF_SERVICE";
        public string E87_CE_LP1_LPTSM_SCT4_TRANSFER_READY = "E87_CE_LP1_LPTSM_SCT4_TRANSFER_READY";
        public string E87_CE_LP1_LPTSM_SCT4_TRANSFER_BLOCKED = "E87_CE_LP1_LPTSM_SCT4_TRANSFER_BLOCKED";
        public string E87_CE_LP1_LPTSM_SCT5_READY_LOAD = "E87_CE_LP1_LPTSM_SCT5_READY_LOAD";
        public string E87_CE_LP1_LPTSM_SCT5_READY_TO_UNLOAD = "E87_CE_LP1_LPTSM_SCT5_READY_TO_UNLOAD";
        public string E87_CE_LP1_LPTSM_SCT6_TRANSFER_BLOCKED = "E87_CE_LP1_LPTSM_SCT6_TRANSFER_BLOCKED";
        public string E87_CE_LP1_LPTSM_SCT7_TRANSFER_BLOCKED = "E87_CE_LP1_LPTSM_SCT7_TRANSFER_BLOCKED";
        public string E87_CE_LP1_LPTSM_SCT8_READY_TO_LOAD = "E87_CE_LP1_LPTSM_SCT8_READY_TO_LOAD";
        public string E87_CE_LP1_LPTSM_SCT9_READY_TO_UNLOAD = "E87_CE_LP1_LPTSM_SCT9_READY_TO_UNLOAD";
        public string E87_CE_LP1_LPTSM_SCT10_TRANSFER_READY = "E87_CE_LP1_LPTSM_SCT10_TRANSFER_READY";

        public Form1()
        {
            InitializeComponent();

            _secsLogger = new SecsLogger(this);
            try
            {
#if DEBUG
                _secsMessagesList = new SecsMessageList("..\\Debug\\Sml\\common.sml");

#else
                _secsMessagesList = new SecsMessageList("..\\Release\\Sml\\common.sml");
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            listboxSmlFile.BeginUpdate();
            foreach (var msg in _secsMessagesList)
                listboxSmlFile.Items.Add($"S{msg.S,-3}F{msg.F,3} : {msg.Name}");
            listboxSmlFile.EndUpdate();


            _secsGemTool = new SanwaSecs
            {
                _logger = _secsLogger,
                _secsMessages = _secsMessagesList
            };
#if DEBUG
            //設定路徑
            _secsGemTool.eqpSVFileName = "..\\Debug\\Csv\\EqpSV.csv";
            _secsGemTool.eqpEventFileName = "..\\Debug\\Csv\\EqpEvent.csv";
            _secsGemTool.eqpECFileName = "..\\Debug\\Csv\\EpqEC.csv";
            _secsGemTool.eqpAlarmFileName = "..\\Debug\\Csv\\EqpAlarm.csv";
            _secsGemTool.eqpDVFileName = "..\\Debug\\Csv\\EqpDV.csv";
#else
            _secsGemTool.eqpSVFileName = "..\\Release\\Csv\\EqpSV.csv";
            _secsGemTool.eqpEventFileName = "..\\Release\\Csv\\EqpEvent.csv";
            _secsGemTool.eqpECFileName = "..\\Release\\Csv\\EpqEC.csv";
            _secsGemTool.eqpAlarmFileName = "..\\Debug\\Csv\\EpqAlarm.csv";

#endif
            _secsGemTool.ConnectionStateChangedEvent += new SanwaSecs.ConnectionStateChanged(UpdateConnectionState);

            _secsGemTool.ChangeControlStateEvent += UpdateControlState;

            _secsGemTool.PrimaryMessageReceivedEvent += PrimaryMessageReceived;

            _secsGemTool.S3F21AutoModeEvent += UpdateAccessMode;
            _secsGemTool.S3F21ManualModeEvent += UpdateAccessMode;

            _secsGemTool.S3F17BindEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17CancelBindEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17CarrierNotificationEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17CancelCarrierNotificationEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17ProceedWithCarrierEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17CancelCarrierEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17CancelCarrierAtPortEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17CarrierReCreateEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17CarrierReleaseEvent += UpdateCarrierRequest;

            _secsGemTool.S3F17CarrierOutEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17CancelCarrierOutEvent += UpdateCarrierRequest;
            _secsGemTool.S3F17CarrierInEvent += UpdateCarrierRequest;

            _secsGemTool.S3F25InServiceEvent += UpdateLoadPortState;
            _secsGemTool.S3F25OutOfServiceEvent += UpdateLoadPortState;
            _secsGemTool.S3F25AutoModeEvent += UpdateAccessMode;
            _secsGemTool.S3F25ManualModeEvent += UpdateAccessMode;
            _secsGemTool.S3F25ReserveAtPortEvent += UpdateReservation;
            _secsGemTool.S3F25CancelReserveAtPortEvent += UpdateReservation;
           
            _secsGemTool.S3F27AutoModeEvent += UpdateAccessMode;
            _secsGemTool.S3F27ManualModeEvent += UpdateAccessMode;

            _secsGemTool.S10F3TerminalMessageEvent += UpdateReceiveTerminalMSG;
            _secsGemTool.S10F5TerminalMessageEvent += UpdateReceiveTerminalMSG;

            _secsGemTool.Initialize();


            _loadPortList = _secsGemTool._loadPortList;
            LoadPort lpObj = new LoadPort
            {
                Name = "LP1",
                Index = 0,
                Number = 1
            };
            _loadPortList.Add(lpObj.Name, lpObj);

            lpObj = new LoadPort
            {
                Name = "LP2",
                Index = 1,
                Number = 2
            };
            _loadPortList.Add(lpObj.Name, lpObj);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _secsGemTool.Connect();
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _secsGemTool.Disconnect();

            lbConnectStateChange.BackColor = Color.Red;
            lbConnectStateChange.Text = "Disconnect";
        }
        public void UpdateReceiveTerminalMSG(object sender, SecsMessageCommand msgObj)
        {
            rtbRecieveTerminalMSG.BeginInvoke((MethodInvoker)delegate
            {
                rtbRecieveTerminalMSG.Clear();


                rtbRecieveTerminalMSG.AppendText("<<Name>>:"+msgObj.Name +"\r\n");
                rtbRecieveTerminalMSG.AppendText("<<Secs>>:" + msgObj.MessageName + "\r\n");
                foreach (string msg in _secsGemTool._terminalMSGList)
                {
                    rtbRecieveTerminalMSG.AppendText(msg +"\r\n");
                }
            });
        }
        public void PrimaryMessageReceived(object sender,PrimaryMessageWrapper e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                try
                {
                    rtbCustomize.AppendText("=========================================\r\n");
                    rtbCustomize.AppendText("收到 Stream: " + e.Message.S.ToString() + ", Function:" + e.Message.F.ToString() + "\r\n");

                    if (e.Message.S.ToString() == "2" && e.Message.F.ToString() == "41")
                    {
                        ReplyS2F42(e);
                    }
                }

                catch(Exception ex)
                {
                    _secsGemTool.ReplyIllegalData(e);
                    _secsLogger.Error("PrimaryMessageReceived_" + ex.Message);
                }
            });
        }
        void ReplyS2F42(PrimaryMessageWrapper e)
        {
            //Receive
            //Structure: L,2
            //  1. < RCMD >
            //  2.L,n # of parameters
            //      1.L,2
            //        1. < CPNAME1 > parameter 1 name
            //        2. < CPVAL1 > parameter 1 value
            //          .
            //          .
            //      n.L,2
            //        1. < CPNAMEn > parameter n name
            //        2. < CPVALn > parameter n value

            if (e.Message.SecsItem.Count != 2)
            {
                rtbCustomize.AppendText("格式異常\r\n");
                _secsGemTool.ReplyIllegalData(e);
                return;
            }

            Item RCMDItem = e.Message.SecsItem.Items[0];
            Item ParaListItem = e.Message.SecsItem.Items[1];
            if (!_secsGemTool.CheckFomart20(RCMDItem))
            {
                rtbCustomize.AppendText("<RCMD> 格式異常\r\n");
                _secsGemTool.ReplyIllegalData(e);
                return;
            }
            rtbCustomize.AppendText("==>" + RCMDItem.GetString() + "\r\n");

            //Do Something
            //...
            //...
            //...
            //Replay    
            //Structure: L,2
            //  1. < HCACK >
            //  2.L,n # of parameters
            //      1.L,2
            //          1. < CPNAME1 > parameter 1 name
            //          2. < CPACK1 > parameter 1 reason
            //          .
            //          .
            //      n.L,2
            //          1. < CPNAMEn > parameter n name
            //          2. < CPACKn > parameter n reason

            string replySML = "";
            replySML = _secsGemTool.GetSMLName(e.Message.S, e.Message.F + 1);
            replySML += "<L[2]\r\n";
            byte[] HCACK = { SanwaACK.HCACK_ACK };
            replySML += "< B[0] " + HCACK.ToHexString() + ">\r\n";
            replySML += "< L[0]\r\n";
            replySML += ">\r\n";
            replySML += ">\r\n";

            e.ReplyAsync(replySML.ToSecsMessage());
        }
        public void UpdateConnectionState()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                lbConnectStateChange.Text = _secsGemTool.State.ToString();

                if (_secsGemTool.State == SanwaSecsDll.ConnectionState.Connected ||
                    _secsGemTool.State == SanwaSecsDll.ConnectionState.Selected)
                {
                    lbConnectStateChange.BackColor = Color.Lime;

                    if (_secsGemTool.State == SanwaSecsDll.ConnectionState.Selected)
                    {
                        timeUIUpdate.Start();
                    }
                }
                else
                {
                    lbConnectStateChange.BackColor = Color.Red;
                }
            });
        }
        public void UpdateReservation(object sender, E87_HostCommand hcObj)
        {
            switch (hcObj.Name.ToUpper())
            {
                case "RESERVEATPORT":

                    ThreadPool.QueueUserWorkItem(new WaitCallback(DoNormalRoundtrip7), hcObj);

                    break;
            }
        }
        public void UpdateAssociation(object sender, LoadPort lpObj)
        {
            //this.BeginInvoke((MethodInvoker)delegate
            //{
            //    if (lpObj == null) return;

            //    UpdateE87UI(sender, lpObj);

            //});
        }
        public void UpdateCarrierRequest(object sender, E87_HostCommand hcObj)
        {
            switch(hcObj.Name.ToUpper())
            {
                case "BIND":
                    if (hcObj.lpObj.CurrentState != E87_LPTS.READY_TO_LOAD)
                    {
                        MessageBox.Show("需切換到 【Ready to Load】", "三和技研好棒棒");
                        return;
                    }

                    ThreadPool.QueueUserWorkItem(new WaitCallback(DoNormalRoundtrip2), hcObj);

                    break;

                case "CARRIERNOTIFICATION":
                    //if (hcObj.lpObj.CurrentState != E87_LPTS.READY_TO_LOAD)
                    //{
                    //    MessageBox.Show("需切換到 【Ready to Load】", "三和技研好棒棒");
                    //    return;
                    //}

                    ThreadPool.QueueUserWorkItem(new WaitCallback(DoNormalRoundtrip5), hcObj);

                    break;
                case "CANCELBIND":
                    lock (hcObj.lpObj)
                    {
                        lock (hcObj.carrierObj)
                        {
                            _secsGemTool.ChangeReserviceState(hcObj.lpObj, E87_RS.NOT_RESERVED);
                            _secsGemTool.LoadPortCarrierAssociated(hcObj.lpObj, hcObj.carrierObj, E87_ASSOCIATED.NOT_ASSOCIATION);
                        }
                    }

                    break;

                case "CARRIERRECREATE":
                    lock (hcObj.lpObj)
                    {
                        lock (hcObj.carrierObj)
                        {
                            _secsGemTool.ChangeReserviceState(hcObj.lpObj, E87_RS.NOT_RESERVED);
                            _secsGemTool.LoadPortCarrierAssociated(hcObj.lpObj, hcObj.carrierObj, E87_ASSOCIATED.NOT_ASSOCIATION);

                            if (hcObj.carrierObj != null)
                            {
                                if (hcObj.carrierObj.ContentMap.Count != 0 ||
                                    hcObj.carrierObj.SlotMap.Count != 0)
                                {
                                    _secsGemTool.ChangeReserviceState(hcObj.lpObj, E87_RS.RESERVED);
                                    _secsGemTool.LoadPortCarrierAssociated(hcObj.lpObj, hcObj.carrierObj, E87_ASSOCIATED.ASSOCIATION);
                                }
                            }
                        }
                    }
                    break;

                case "PROCEEDWITHCARRIER":

                    //ewhNormalRoundtrip1.Set();
                    ewhHostVerif[(int)eVerification.OK].Set();
                    break;

                case "CANCELCARRIER":
                    ewhHostVerif[(int)eVerification.Fail].Set();
                    break;

                case "CARRIEROUT":
                    ewhCarrierOut.Set();
                    break;

            }

            if (hcObj == null) return;

            this.BeginInvoke((MethodInvoker)delegate
            {
                rtxtMainRequest.AppendText("===============\r\n");
                rtxtSecondRequest.AppendText("===============\r\n");
                rtxtMainRequest.AppendText("<<Name>>:" + hcObj.Name + "\r\n");
                rtxtMainRequest.AppendText("<<SECS>>:" + hcObj.MessageName + "\r\n");

                if (hcObj.lpObj != null)
                    rtxtMainRequest.AppendText("<<Load Port>>:" + hcObj.lpObj.Name + "\r\n");

                if (hcObj.carrierObj != null)
                    rtxtMainRequest.AppendText("<<Carrier>>:" + hcObj.carrierObj.ObjID + "\r\n");

                rtxtSecondRequest.AppendText("<<Name>>:CarrierNotification\r\n");


                if (hcObj.carrierObj != null)
                {
                    rtxtSecondRequest.AppendText("<<Carrier Capacity>>:" + hcObj.carrierObj.Capacity.ToString() + "\r\n");

                    int count = 0;
                    foreach (var contentMap in hcObj.carrierObj.ContentMap.Values)
                    {
                        count++;
                        rtxtSecondRequest.AppendText("<<Carrier[" + count.ToString() + "]LotID>>:" + contentMap.LotID + "\r\n");
                        rtxtSecondRequest.AppendText("<<Carrier[" + count.ToString() + "]SubstrateID>>:" + contentMap.SubstrateID + "\r\n");
                    }

                    count = 0;
                    foreach (var slotMap in hcObj.carrierObj.SlotMap)
                    {
                        count++;
                        rtxtSecondRequest.AppendText("<<Carrier[" + count.ToString() + "]SlotMap>>:" + slotMap.ToString() + "\r\n");
                    }

                    rtxtSecondRequest.AppendText("<<Carrier SubstrateCount>>:" + hcObj.carrierObj.SubstrateCount.ToString() + "\r\n");

                    rtxtSecondRequest.AppendText("<<Carrier LocationID>>:" + hcObj.carrierObj.LocationID + "\r\n");

                    rtxtSecondRequest.AppendText("<<Carrier Usage>>:" + hcObj.carrierObj.Usage + "\r\n");
                }
            });
        }
        public void UpdateAccessMode(object sender, E87_HostCommand hcObj)
        {
            //this.BeginInvoke((MethodInvoker)delegate
            //{
            //    if (hcObj == null) return;
            //    if (hcObj.lpObj == null) return;

            //    UpdateE87UI(sender, hcObj.lpObj);

            //});
        }
        public void UpdateLoadPortState(object sender, E87_HostCommand hcObj)
        {
            //this.BeginInvoke((MethodInvoker)delegate
            //{
            //    if (hcObj == null) return;
            //    if (hcObj.lpObj == null) return;

            //    UpdateE87UI(sender, hcObj.lpObj);
            //});
        }
        public void UpdateControlState(object sender, CONTROL_STATE state)
        {
            //this.BeginInvoke((MethodInvoker)delegate
            //{
            //    switch (state)
            //    {
            //        case CONTROL_STATE.EQUIPMENT_OFF_LINE:
            //            lblControlState.Text = "EQUIPMENT_OFF_LINE";
            //            lblControlState.BackColor = Color.Red;
            //            break;

            //        case CONTROL_STATE.ATTEMPT_ON_LINE:
            //            lblControlState.Text = "ATTEMPT_ON_LINE";
            //            lblControlState.BackColor = Color.Red;
            //            break;

            //        case CONTROL_STATE.HOST_OFF_LINE:
            //            lblControlState.Text = "HOST_OFF_LINE";
            //            lblControlState.BackColor = Color.Red;
            //            break;

            //        case CONTROL_STATE.ON_LINE_LOCAL:
            //            lblControlState.Text = "ON_LINE_LOCATE";
            //            lblControlState.BackColor = Color.Lime;
            //            break;

            //        case CONTROL_STATE.ON_LINE_REMOTE:
            //            lblControlState.Text = "ON_LINE_REMOTE";
            //            lblControlState.BackColor = Color.Lime;
            //            break;
            //    }
            //});
        }
        class SecsLogger : ISecsGemLogger
        {
            readonly Form1 _form;
            internal SecsLogger(Form1 form)
            {
                _form = form;
            }
            public void MessageIn(SecsMessage msg, int systembyte)
            {
                _form.BeginInvoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"<-- [0x{systembyte:X8}] {msg.ToSml()}\n");
                });
            }

            public void MessageOut(SecsMessage msg, int systembyte)
            {
                _form.BeginInvoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"--> [0x{systembyte:X8}] {msg.ToSml()}\n");
                });
            }

            public void Info(string msg)
            {
                _form.BeginInvoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Blue;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }

            public void Warning(string msg)
            {
                _form.BeginInvoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Red;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }

            public void Error(string msg, Exception ex = null)
            {
                _form.BeginInvoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Red;
                    _form.richTextBox1.AppendText($"{msg}\n");
                    _form.richTextBox1.SelectionColor = Color.Gray;
                    _form.richTextBox1.AppendText($"{ex}\n");
                });
            }

            public void Debug(string msg)
            {
                _form.BeginInvoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.DarkOrange;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }
        }
        private async void btnS1F1_Click(object sender, EventArgs e)
        {
            await _secsGemTool.S1F1Async();
        }
        private void listboxSmlFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            SecsMessage secsMsg = _secsMessagesList[listboxSmlFile.SelectedIndex];
            tbShowSML.Text = secsMsg.ToSml();
            btnSent.Enabled = (secsMsg.F % 2) != 0 ? true :false;
        }
        private void listboxSmlFile_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
        private async void btnSent_Click(object sender, EventArgs e)
        {
            if (_secsGemTool.State != SanwaSecsDll.ConnectionState.Selected)
                return;

            try
            {
                SecsMessage secsMsg = _secsMessagesList[listboxSmlFile.SelectedIndex];
                await _secsGemTool.SetStreamFunction(secsMsg);
            }
            catch (SecsException ex)
            {
                _secsLogger.Error("Form1.btnSent_Click", ex);
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }
        private void edMDLN_TextChanged(object sender, EventArgs e)
        {
            _secsGemTool.strMDLN = edMDLN.Text;
        }
        private void edSOFTREV_TextChanged(object sender, EventArgs e)
        {
            _secsGemTool.strSOFTREV = edSOFTREV.Text;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            edMDLN.Text = _secsGemTool.strMDLN;
            edSOFTREV.Text = _secsGemTool.strSOFTREV;

            comSVID.DataSource = new BindingSource(_secsGemTool._svList, null);
            comSVID.DisplayMember = "Key";
            comSVID.ValueMember = "Value";

            //模擬給值 start ++
            _secsGemTool.SetSV("GEM_LINK_STATE", 2);
            _secsGemTool.SetSV("GEM_COMM_MODE", 0);
            //模擬給值 start ++

            foreach (string key in _secsGemTool._alarmList.Keys)
                listAlarmID.Items.Add(key);

            foreach (string key in _secsGemTool._eventList.Keys)
                listEventItem.Items.Add(key);
        }
        private async void btnS1F3_Click(object sender, EventArgs e)
        {
            await _secsGemTool.S1F13Async();
        }
        private void btnEqpOffineLine_Click(object sender, EventArgs e)
        {
            _secsGemTool.ChangeToOffLineState();
        }
        private void btnEqpRequestOnline_Click(object sender, EventArgs e)
        {
            _secsGemTool.RequestToOnLine();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
        private async void btnEqpOnLineLocate_Click(object sender, EventArgs e)
        {
            await _secsGemTool.ChangeToOnLineLocalState();
        }
        private async void btnEqpOnLineRemote_Click(object sender, EventArgs e)
        {
            await _secsGemTool.ChangeToOnLineRemoteState();
        }
        private void btnAlarmSetTest_Click(object sender, EventArgs e)
        {
            int index = listAlarmID.SelectedIndex;
            if(index >= 0)
            { 
                var KeyPair = _secsGemTool._alarmList.ElementAt(index);

                _secsGemTool.S5F1SetAlarmReport(KeyPair.Value._name, true);

                listAlarmID.Refresh();
            }
        }
        private void btnAlarmresetTest_Click(object sender, EventArgs e)
        {
            int index = listAlarmID.SelectedIndex;
            if (index >= 0)
            {
                var KeyPair = _secsGemTool._alarmList.ElementAt(index);

                _secsGemTool.S5F1SetAlarmReport(KeyPair.Value._name, false);

                listAlarmID.Refresh();
            }
        }
        private void btnTerminalMessage_Click(object sender, EventArgs e)
        {
            _secsGemTool.S10F1SetTerminalMSG(rtbSendTerminalMSG.Text);
        }
        private void listAlarmID_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Brush AlarmResetBrush = Brushes.Black; //初始化字体颜色=黑色
            Brush AlarmSetBrush = Brushes.Red;

            SanwaAlarm alarmObj = _secsGemTool._alarmList.ElementAt(e.Index).Value;
            if (alarmObj._set)
            {
                e.Graphics.DrawString(listAlarmID.Items[e.Index].ToString(), e.Font, AlarmSetBrush, e.Bounds, null);
            }
            else
            {
                e.Graphics.DrawString(listAlarmID.Items[e.Index].ToString(), e.Font, AlarmResetBrush, e.Bounds, null);
            }

            e.DrawFocusRectangle();
        }
        private void btnS2F17_Click(object sender, EventArgs e)
        {
            _secsGemTool.S2F17Async();
        }
        private void listEventItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBoxObj = (ListBox)sender;
            if (listBoxObj.SelectedIndex < 0) return;

            listReportItem.Items.Clear();

            SanwaEvent eventObj = _secsGemTool._eventList.ElementAt(listBoxObj.SelectedIndex).Value;
            if(eventObj._rptidList.Count > 0)
            {
                foreach (string rptid in eventObj._rptidList)
                    listReportItem.Items.Add(rptid);

                listReportItem.SetSelected(0, true);
            }
        }
        private void listReportItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBoxObj = (ListBox)sender;
            if (listBoxObj.SelectedIndex < 0) return;

            listVIDItem.Items.Clear();

            string rptid = listBoxObj.GetItemText(listBoxObj.SelectedItem);
            _secsGemTool._reportList.TryGetValue(rptid, out SanwaRPTID rptObj);
            if (rptObj != null)
            {
                foreach (string vid in rptObj._vidList.Keys)
                    listVIDItem.Items.Add(vid);
            }
        }
        private void btnSetEvent_Click(object sender, EventArgs e)
        {
            int index = listEventItem.SelectedIndex;
            if (index >= 0)
            {
                var KeyPair = _secsGemTool._eventList.ElementAt(index);

                if(!cbAnnotated.Checked)
                {
                    _secsGemTool.S6F11Async(KeyPair.Value._name);
                }
                else
                {
                    _secsGemTool.S6F13Async(KeyPair.Value._name);
                }

            
                listEventItem.Refresh();
            }
        }
        private void btnInService_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;

            bool setEvent = true;

            lock (lpObj)
            {
                setEvent = _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.IN_SERVICE);
            }

            if(setEvent)
            {
                _secsGemTool._eventList.TryGetValue(E87_CE_LP1_LPTSM_SCT2_IN_SERVICE, out SanwaEvent eventObj);
                if (eventObj != null)
                    _secsGemTool.S6F11Async(eventObj._name);
            }
         
         }
        private void btnOutOfService_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;

            bool setEvent = true;
            lock (lpObj)
            {
                setEvent = _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.OUT_OF_SERVICE);
            }

            if(setEvent)
            {
                _secsGemTool._eventList.TryGetValue(E87_CE_LP1_LPTSM_SCT3_OUT_OF_SERVICE, out SanwaEvent eventObj);
                if (eventObj != null)
                    _secsGemTool.S6F11Async(eventObj._name);
            }

        }
        private void btnTransferReady_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;



            bool setEvent = true;
            lock (lpObj)
            {
                setEvent = _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_READY);
            }

            if(setEvent)
            {
                string eventName = E87_CE_LP1_LPTSM_SCT4_TRANSFER_READY;
                switch (lpObj.PreviousState)
                {
                    //E87 LP1 LPTSM SCT4 IN SERVICE -> TRANSFER READY
                    case E87_LPTS.IN_SERVICE:
                        eventName = E87_CE_LP1_LPTSM_SCT4_TRANSFER_READY;
                        break;
                    //E87 LP1 LPTSM SCT10 TRANSFER BLOCKED -> TRANSFER READY
                    case E87_LPTS.TRANSFER_BLOCKED:
                        eventName = E87_CE_LP1_LPTSM_SCT10_TRANSFER_READY;
                        break;
                }

                _secsGemTool._eventList.TryGetValue(eventName, out SanwaEvent eventObj);
                if (eventObj != null)
                    _secsGemTool.S6F11Async(eventObj._name);
            }
        }
        private void btnTranferBlocked_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;

            bool setEvent = true;

            lock (lpObj)
            {
                setEvent = _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);
            }

            if(setEvent)
            {
                string eventName = E87_CE_LP1_LPTSM_SCT6_TRANSFER_BLOCKED;
                switch (lpObj.PreviousState)
                {
                    case E87_LPTS.READY_TO_LOAD:
                        eventName = E87_CE_LP1_LPTSM_SCT6_TRANSFER_BLOCKED;
                        break;
                    //E87 LP1 LPTSM SCT7 READY TO UNLOAD -> TRANSFER BLOCKED
                    case E87_LPTS.READY_TO_UNLOAD:
                        eventName = E87_CE_LP1_LPTSM_SCT7_TRANSFER_BLOCKED;
                        break;
                }

                _secsGemTool._eventList.TryGetValue(eventName, out SanwaEvent eventObj);
                if (eventObj != null)
                    _secsGemTool.S6F11Async(eventObj._name);
            }
        }
        private void btnReadyToLoad_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            
            if (lpObj == null) return;

            bool setEvent = true;

            lock(lpObj)
            {
                setEvent = _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_LOAD);
            }
            

            if(setEvent)
            {
                string eventName = E87_CE_LP1_LPTSM_SCT5_READY_LOAD;
                switch (lpObj.PreviousState)
                {
                    //E87_CE_LP1_LPTSM_SCT5_READY_LOAD
                    case E87_LPTS.TRANSFER_READY:
                        eventName = E87_CE_LP1_LPTSM_SCT5_READY_LOAD;
                        break;
                    //E87_CE_LP1_LPTSM_SCT8_READY_TO_LOAD
                    case E87_LPTS.TRANSFER_BLOCKED:
                        eventName = E87_CE_LP1_LPTSM_SCT8_READY_TO_LOAD;
                        break;
                }

                _secsGemTool._eventList.TryGetValue(eventName, out SanwaEvent eventObj);
                if (eventObj != null)
                    _secsGemTool.S6F11Async(eventObj._name);
            }
        }
        private void btnReadyToUnload_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;
            bool setEvent = true;

            lock(lpObj)
            {
                setEvent = _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_UNLOAD);
            }

            if(setEvent)
            {
                string eventName = E87_CE_LP1_LPTSM_SCT5_READY_TO_UNLOAD;
                switch (lpObj.PreviousState)
                {
                    //E87 LP1 LPTSM SCT5 TRANSFER READY -> READY TO UNLOAD
                    case E87_LPTS.TRANSFER_READY:
                        eventName = E87_CE_LP1_LPTSM_SCT5_READY_TO_UNLOAD;
                        break;
                    //E87 LP1 LPTSM SCT9 TRANSFER BLOCKED -> READY TO UNLOAD
                    case E87_LPTS.TRANSFER_BLOCKED:
                        eventName = E87_CE_LP1_LPTSM_SCT9_READY_TO_UNLOAD;
                        break;
                }

                _secsGemTool._eventList.TryGetValue(eventName, out SanwaEvent eventObj);
                if (eventObj != null)
                    _secsGemTool.S6F11Async(eventObj._name);
            }
        }
        private void UpdateE87UI(object sender, LoadPort lpObj)
        {
            if (lpObj == null) return;
            if(tabControl1.SelectedTab != pageCMS)  return;


            if (lpObj.Name == "LP1")
            {
                lbNowLoadPort.Text = "LP1";
                rectLP1.FillColor = Color.White;
                rectLP2.FillColor = Color.Blue;
            }
            else
            {
                lbNowLoadPort.Text = "LP2";
                rectLP2.FillColor = Color.White;
                rectLP1.FillColor = Color.Blue;
            }

            if (ctrlCMS.SelectedTab == pageAccessMode)
            {
                if (lpObj.IsReserved == E87_RS.NOT_RESERVED)
                {
                    rectNotReserved.Visible = true;
                    rectReserved.Visible = false;

                    lbAccessMode.ForeColor = Color.Blue;
                    if (lpObj.AccessMode == E87_AM.AUTO)
                    {
                        rectManual.Visible = false;
                        rectAuto.Visible = true;
                        lbAccessMode.Text = "Auto";
                    }
                    else if (lpObj.AccessMode == E87_AM.MANUAL)
                    {
                        rectManual.Visible = true;
                        rectAuto.Visible = false;
                        lbAccessMode.Text = "Manual";
                    }
                    else
                    {
                        rectManual.Visible = false;
                        rectAuto.Visible = false;
                        lbAccessMode.Text = "no state";
                    }

                    lineDenyAccessMode1.Visible = false;
                    lineDenyAccessMode2.Visible = false;
                }
                else if (lpObj.IsReserved == E87_RS.RESERVED)
                {
                    rectNotReserved.Visible = false;
                    rectReserved.Visible = true;

                    lbAccessMode.ForeColor = Color.Red;
                    lbAccessMode.Text = "Reserved";

                    lineDenyAccessMode1.Visible = true;
                    lineDenyAccessMode2.Visible = true;
                }
                else
                {
                    rectNotReserved.Visible = false;
                    rectReserved.Visible = false;
                }
            }

            if (ctrlCMS.SelectedTab == pageAssociated)
            {
                if (lpObj.Carrier != null)
                {
                    rectNotAssociated.Visible = false;
                    rectAssociated.Visible = true;

                    lbAssociatedCarrierID.Text = lpObj.Carrier.ObjID;
                }
                else
                {
                    rectNotAssociated.Visible = true;
                    rectAssociated.Visible = false;

                    lbAssociatedCarrierID.Text = "";
                }
            }

            if (ctrlCMS.SelectedTab == pageLPT)
            {
                ShowLPTSBtnEnabled(lpObj.CurrentState);

                switch (lpObj.CurrentState)
                {
                    case E87_LPTS.NO_STATE:
                        lbCurrentLPTS.Text = "no state";
                        break;
                    case E87_LPTS.IN_SERVICE:
                        lbCurrentLPTS.Text = "IN SERVICE";
                        rectOutOfService.Visible = false;
                        rectReadyToLoad.Visible = false;
                        rectReadyToUpload.Visible = false;
                        rectTranferBlacked.Visible = false;
                        rectTransferReady.Visible = false;
                        rectInService.Visible = true;
                        break;
                    case E87_LPTS.OUT_OF_SERVICE:
                        lbCurrentLPTS.Text = "OUT OF SERVICE";
                        rectInService.Visible = false;
                        rectReadyToLoad.Visible = false;
                        rectReadyToUpload.Visible = false;
                        rectTranferBlacked.Visible = false;
                        rectTransferReady.Visible = false;
                        rectOutOfService.Visible = true;
                        break;
                    case E87_LPTS.READY_TO_LOAD:
                        lbCurrentLPTS.Text = "READY TO LOAD";
                        rectOutOfService.Visible = false;
                        rectInService.Visible = false;
                        rectReadyToUpload.Visible = false;
                        rectTranferBlacked.Visible = false;
                        rectTransferReady.Visible = false;
                        rectReadyToLoad.Visible = true;

                        break;
                    case E87_LPTS.READY_TO_UNLOAD:
                        lbCurrentLPTS.Text = "READY TO UNLOAD";
                        rectOutOfService.Visible = false;
                        rectInService.Visible = false;
                        rectReadyToLoad.Visible = false;
                        rectTranferBlacked.Visible = false;
                        rectTransferReady.Visible = false;
                        rectReadyToUpload.Visible = true;

                        break;
                    case E87_LPTS.TRANSFER_BLOCKED:
                        lbCurrentLPTS.Text = "TRANSFER BLOCKED";
                        rectOutOfService.Visible = false;
                        rectInService.Visible = false;
                        rectReadyToLoad.Visible = false;
                        rectReadyToUpload.Visible = false;
                        rectTransferReady.Visible = false;
                        rectTranferBlacked.Visible = true;
                        break;
                    case E87_LPTS.TRANSFER_READY:
                        lbCurrentLPTS.Text = "TRANSFER READY";
                        rectOutOfService.Visible = false;
                        rectInService.Visible = false;
                        rectReadyToLoad.Visible = false;
                        rectReadyToUpload.Visible = false;
                        rectTranferBlacked.Visible = false;
                        rectTransferReady.Visible = true;
                        break;
                }
                switch (lpObj.PreviousState)
                {
                    case E87_LPTS.NO_STATE:
                        lbPreviousLPTS.Text = "no state";
                        break;
                    case E87_LPTS.IN_SERVICE:
                        lbPreviousLPTS.Text = "IN SERVICE";
                        break;
                    case E87_LPTS.OUT_OF_SERVICE:
                        lbPreviousLPTS.Text = "OUT OF SERVICE";
                        break;
                    case E87_LPTS.READY_TO_LOAD:
                        lbPreviousLPTS.Text = "READY TO LOAD";
                        break;
                    case E87_LPTS.READY_TO_UNLOAD:
                        lbPreviousLPTS.Text = "READY TO UNLOAD";
                        break;
                    case E87_LPTS.TRANSFER_BLOCKED:
                        lbPreviousLPTS.Text = "TRANSFER BLOCKED";
                        break;
                    case E87_LPTS.TRANSFER_READY:
                        lbPreviousLPTS.Text = "TRANSFER READY";
                        break;
                }
            }

            if (ctrlCMS.SelectedTab == pageCarrierStatus)
            {
                if (lpObj.Carrier != null)
                {
                    SanwaCarrier carrier = lpObj.Carrier;
                    switch(carrier.CarrierIDStatus)
                    {
                        case E87_CIDS.CARRIER:
                            rtCarrierInStatus.Visible = true;
                            rtIDNotRead.Visible = false;
                            rtIDVerificationOK.Visible = false;
                            rtIDverificationFail.Visible = false;
                            rtWaitingForHostID.Visible = false;
                            break;

                        case E87_CIDS.ID_NOT_READ:
                            rtCarrierInStatus.Visible = false;
                            rtIDNotRead.Visible = true;
                            rtIDVerificationOK.Visible = false;
                            rtIDverificationFail.Visible = false;
                            rtWaitingForHostID.Visible = false;
                            break;

                        case E87_CIDS.ID_VERIFICATION_OK:
                            rtCarrierInStatus.Visible = false;
                            rtIDNotRead.Visible = false;
                            rtIDVerificationOK.Visible = true;
                            rtIDverificationFail.Visible = false;
                            rtWaitingForHostID.Visible = false;
                            break;

                        case E87_CIDS.ID_VERIFICATION_FAILED:
                            rtCarrierInStatus.Visible = false;
                            rtIDNotRead.Visible = false;
                            rtIDVerificationOK.Visible = false;
                            rtIDverificationFail.Visible = true;
                            rtWaitingForHostID.Visible = false;
                            break;

                        case E87_CIDS.WAITING_FOR_HOST:
                            rtCarrierInStatus.Visible = false;
                            rtIDNotRead.Visible = false;
                            rtIDVerificationOK.Visible = false;
                            rtIDverificationFail.Visible = false;
                            rtWaitingForHostID.Visible = true;
                            break;
                        default:
                            rtCarrierInStatus.Visible = false;
                            rtIDNotRead.Visible = false;
                            rtIDVerificationOK.Visible = false;
                            rtIDverificationFail.Visible = false;
                            rtWaitingForHostID.Visible = false;
                            break;
                    }
                    switch(carrier.SlotMapStatus)
                    {
                        case E87_CSMS.CARRIER:
                            rtCarrierSlotMapStatus.Visible = true;
                            rtSlotMapNotRead.Visible = false;
                            rtWaitingForHostSlotMap.Visible = false;
                            rtSlopMapVerificationOK.Visible = false;
                            rtSlopMapVerificationFail.Visible = false;
                            break;
                        case E87_CSMS.SLOT_MAP_NOT_READ:
                            rtCarrierSlotMapStatus.Visible = false;
                            rtSlotMapNotRead.Visible = true;
                            rtWaitingForHostSlotMap.Visible = false;
                            rtSlopMapVerificationOK.Visible = false;
                            rtSlopMapVerificationFail.Visible = false;
                            break;
                        case E87_CSMS.SLOT_MAP_VERIFICATION_OK:
                            rtCarrierSlotMapStatus.Visible = false;
                            rtSlotMapNotRead.Visible = false;
                            rtWaitingForHostSlotMap.Visible = false;
                            rtSlopMapVerificationOK.Visible = true;
                            rtSlopMapVerificationFail.Visible = false;
                            break;
                        case E87_CSMS.SLOT_MAP_VERIFICATION_FAIL:
                            rtCarrierSlotMapStatus.Visible = false;
                            rtSlotMapNotRead.Visible = false;
                            rtWaitingForHostSlotMap.Visible = false;
                            rtSlopMapVerificationOK.Visible = false;
                            rtSlopMapVerificationFail.Visible = true;
                            break;
                        case E87_CSMS.WAITING_FOR_HOST:
                            rtCarrierSlotMapStatus.Visible = false;
                            rtSlotMapNotRead.Visible = false;
                            rtWaitingForHostSlotMap.Visible = true;
                            rtSlopMapVerificationOK.Visible = false;
                            rtSlopMapVerificationFail.Visible = false;
                            break;
                        default:
                            rtCarrierSlotMapStatus.Visible = false;
                            rtSlotMapNotRead.Visible = false;
                            rtWaitingForHostSlotMap.Visible = false;
                            rtSlopMapVerificationOK.Visible = false;
                            rtSlopMapVerificationFail.Visible = false;
                            break;

                    }
                    switch(carrier.CarrierAccessingStatus)
                    {
                        case E87_CAS.CARRIER:
                            rtCarrierAccessingStatus.Visible = true;
                            rtNotAccessed.Visible = false;
                            rtInAccessed.Visible = false;
                            rtCarrierComplete.Visible = false;
                            rtCarrierStopped.Visible = false;
                            break;

                        case E87_CAS.CARRIER_COMPLETE:
                            rtCarrierAccessingStatus.Visible = false;
                            rtNotAccessed.Visible = false;
                            rtInAccessed.Visible = false;
                            rtCarrierComplete.Visible = true;
                            rtCarrierStopped.Visible = false;
                            break;
                        case E87_CAS.CARRIER_STOPPED:
                            rtCarrierAccessingStatus.Visible = false;
                            rtNotAccessed.Visible = false;
                            rtInAccessed.Visible = false;
                            rtCarrierComplete.Visible = false;
                            rtCarrierStopped.Visible = true;
                            break;
                        case E87_CAS.IN_ACCESSED:
                            rtCarrierAccessingStatus.Visible = false;
                            rtNotAccessed.Visible = false;
                            rtInAccessed.Visible = true;
                            rtCarrierComplete.Visible = false;
                            rtCarrierStopped.Visible = false;
                            break;
                        case E87_CAS.NOT_ACCESSED:
                            rtCarrierAccessingStatus.Visible = false;
                            rtNotAccessed.Visible = true;
                            rtInAccessed.Visible = false;
                            rtCarrierComplete.Visible = false;
                            rtCarrierStopped.Visible = false;
                            break;

                        default:
                            rtCarrierAccessingStatus.Visible = false;
                            rtNotAccessed.Visible = false;
                            rtInAccessed.Visible = false;
                            rtCarrierComplete.Visible = false;
                            rtCarrierStopped.Visible = false;
                            break;

                    }
                }
                else
                {
                    rtCarrierInStatus.Visible = false;
                    rtIDNotRead.Visible = false;
                    rtIDVerificationOK.Visible = false;
                    rtIDverificationFail.Visible = false;
                    rtWaitingForHostID.Visible = false;

                    rtCarrierSlotMapStatus.Visible = false;
                    rtSlotMapNotRead.Visible = false;
                    rtWaitingForHostSlotMap.Visible = false;
                    rtSlopMapVerificationOK.Visible = false;
                    rtSlopMapVerificationFail.Visible = false;

                    rtCarrierAccessingStatus.Visible = false;
                    rtNotAccessed.Visible = false;
                    rtInAccessed.Visible = false;
                    rtCarrierComplete.Visible = false;
                    rtCarrierStopped.Visible = false;
                }
            }
        }
        private void btnLP1_Click(object sender, EventArgs e)
        {
            btnLP1.Enabled = false;
            btnLP2.Enabled = true;
        }
        private void btnLP2_Click(object sender, EventArgs e)
        {
            btnLP1.Enabled = true;
            btnLP2.Enabled = false;
        }
        private void ShowLPTSBtnEnabled(E87_LPTS nowState)
        {
            switch(nowState)
            {
                case E87_LPTS.NO_STATE:
                    btnInService.Enabled = true;
                    btnOutOfService.Enabled = true;
                    btnTransferReady.Enabled = false;
                    btnTranferBlocked.Enabled = false;
                    btnReadyToLoad.Enabled = false;
                    btnReadyToUnload.Enabled = false;
                    break;
                case E87_LPTS.IN_SERVICE:
                    btnOutOfService.Enabled = true;
                    btnInService.Enabled = false;
                    btnTransferReady.Enabled = true;
                    btnTranferBlocked.Enabled = true;
                    btnReadyToLoad.Enabled = false;
                    btnReadyToUnload.Enabled = false;
                    break;
                case E87_LPTS.OUT_OF_SERVICE:
                    btnOutOfService.Enabled = false;
                    btnInService.Enabled = true;
                    btnTransferReady.Enabled = false;
                    btnTranferBlocked.Enabled = false;
                    btnReadyToLoad.Enabled = false;
                    btnReadyToUnload.Enabled = false;
                    break;
                case E87_LPTS.TRANSFER_READY:
                    btnOutOfService.Enabled = true;
                    btnInService.Enabled = false;
                    btnTransferReady.Enabled = false;
                    btnTranferBlocked.Enabled = false;
                    btnReadyToLoad.Enabled = true;
                    btnReadyToUnload.Enabled = true;
                    break;
                case E87_LPTS.TRANSFER_BLOCKED:
                    btnOutOfService.Enabled = true;
                    btnInService.Enabled = false;
                    btnTransferReady.Enabled = true;
                    btnTranferBlocked.Enabled = false;
                    btnReadyToLoad.Enabled = true;
                    btnReadyToUnload.Enabled = true;
                    break;
                case E87_LPTS.READY_TO_LOAD:
                    btnOutOfService.Enabled = true;
                    btnInService.Enabled = false;
                    btnTransferReady.Enabled = false;
                    btnTranferBlocked.Enabled = true;
                    btnReadyToLoad.Enabled = false;
                    btnReadyToUnload.Enabled = false;
                    break;
                case E87_LPTS.READY_TO_UNLOAD:
                    btnOutOfService.Enabled = true;
                    btnInService.Enabled = false;
                    btnTransferReady.Enabled = false;
                    btnTranferBlocked.Enabled = true;
                    btnReadyToLoad.Enabled = false;
                    btnReadyToUnload.Enabled = false;
                    break;
            }
        }
        private void btnManual_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;


            lock (lpObj)
            {
                _secsGemTool.ChangeAccessMode(lpObj, E87_AM.MANUAL);
            }
            //UpdateE87UI(sender, lpObj);
        }
        private void btnAuto_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;

            lock(lpObj)
            {
                _secsGemTool.ChangeAccessMode(lpObj, E87_AM.AUTO);
            }



        }
        private void btnNotReserved_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;

            lock (lpObj)
            {
                _secsGemTool.ChangeReserviceState(lpObj, E87_RS.NOT_RESERVED);
            }

        }
        private void btnReserved_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;

            lock (lpObj)
            {
                _secsGemTool.ChangeReserviceState(lpObj, E87_RS.RESERVED);
            }
            //UpdateE87UI(sender, lpObj);
        }
        private void btnClearRequest_Click(object sender, EventArgs e)
        {
            rtxtMainRequest.Clear();
            rtxtSecondRequest.Clear();
        }
        private void rtxtMainRequest_TextChanged(object sender, EventArgs e)
        {
            rtxtMainRequest.SelectionStart = rtxtMainRequest.TextLength;
            rtxtMainRequest.ScrollToCaret();
        }
        private void rtxtSecondRequest_TextChanged(object sender, EventArgs e)
        {
            rtxtSecondRequest.SelectionStart = rtxtSecondRequest.TextLength;
            rtxtSecondRequest.ScrollToCaret();
        }
        private void btnAddCarrierID_Click(object sender, EventArgs e)
        {
            _secsGemTool._carrierList.TryGetValue(txtCarrierID.Text, out SanwaCarrier carrierObj);

            if(carrierObj == null)
            {
                carrierObj = new SanwaCarrier
                {
                    ObjID = txtCarrierID.Text
                };

                _secsGemTool._carrierList.Add(carrierObj.ObjID, carrierObj);
            }
        }
        private void timeUIUpdate_Tick(object sender, EventArgs e)
        {
            string name = "LP1";

            if (btnLP2.Enabled)
            {
                name = "LP1";
            }
            else
            {
                name = "LP2";
            }

            _loadPortList.TryGetValue(name, out LoadPort lpObj);

            if(lpObj != null)
                UpdateE87UI(null, lpObj);


            switch (_secsGemTool._currentState)
            {
                case CONTROL_STATE.EQUIPMENT_OFF_LINE:
                    lblControlState.Text = "EQUIPMENT_OFF_LINE";
                    lblControlState.BackColor = Color.Red;
                    break;

                case CONTROL_STATE.ATTEMPT_ON_LINE:
                    lblControlState.Text = "ATTEMPT_ON_LINE";
                    lblControlState.BackColor = Color.Red;
                    break;

                case CONTROL_STATE.HOST_OFF_LINE:
                    lblControlState.Text = "HOST_OFF_LINE";
                    lblControlState.BackColor = Color.Red;
                    break;

                case CONTROL_STATE.ON_LINE_LOCAL:
                    lblControlState.Text = "ON_LINE_LOCATE";
                    lblControlState.BackColor = Color.Lime;
                    break;

                case CONTROL_STATE.ON_LINE_REMOTE:
                    lblControlState.Text = "ON_LINE_REMOTE";
                    lblControlState.BackColor = Color.Lime;
                    break;
            }

        }
        private void Form1_Leave(object sender, EventArgs e)
        {
            timeUIUpdate.Stop();
        }

        private void btnE87NR1_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;

            if (NormalRoundtrip1Start) return;

            if (lpObj.CurrentState != E87_LPTS.READY_TO_LOAD)
            {
                MessageBox.Show("需切換到 【Ready to Load】", "三和技研好棒棒");
                return;
            }

            if (lpObj.Associated != E87_ASSOCIATED.NOT_ASSOCIATION)
            {
                MessageBox.Show("需切換到 【Not Association】", "三和技研好棒棒");
                return;
            }


            ThreadPool.QueueUserWorkItem(new WaitCallback(DoNormalRoundtrip1), lpObj);
        }

        private void DoNormalRoundtrip1 (object Obj)
        {
            LoadPort lpObj = (LoadPort)Obj;

            NormalRoundtrip1Start = true;
            bool ProcAbnormal = false;

            SanwaCarrier carrierObj = null;

            lock (lpObj)
            {
                //Loading transfer starts
                if (_secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED))
                {
                    //Loading transfer completes
                    _secsGemTool._eventList.TryGetValue(E87_CE_LP1_LPTSM_SCT6_TRANSFER_BLOCKED, out SanwaEvent eventObj);

                    if (eventObj != null)
                        _secsGemTool.S6F11Async(eventObj._name);

                    //CarrierID is read
                    string CarrierID = "CSTID_01";

                    _secsGemTool._carrierList.TryGetValue(CarrierID, out carrierObj);

                    if (carrierObj == null)
                    {
                        carrierObj = new SanwaCarrier
                        {
                            ObjID = CarrierID,
                            LocationID = lpObj.Name
                        };

                        _secsGemTool._carrierList.Add(CarrierID, carrierObj);

                        //if (lpObj.IsReserved != E87_RS.NOT_RESERVED)
                        //{
                        //    MessageBox.Show("需切換到 【Not Reserved】", "三和技研好棒棒");
                        //    return;
                        //}

                        lock(carrierObj)
                        {
                            _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.IN_ACCESSED);

                            //LCAS -> A
                            _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.ASSOCIATION);
                            Thread.Sleep(100);
                            //CIDS -> WFH
                            _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.WAITING_FOR_HOST);
                            Thread.Sleep(100);
                            //CSMS -> SNR
                            _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_NOT_READ);
                            Thread.Sleep(100);

                            int index = WaitHandle.WaitAny(ewhHostVerif);
                            
                            //ewhNormalRoundtrip1.WaitOne();
                            //ewhNormalRoundtrip1.Reset();

                            if(index == (int)eVerification.Fail)
                            {
                                _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_FAILED);
                                //Thread.Sleep(1000);

                                _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_STOPPED);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                //CarrierID is verified by host, and result is OK

                                //CIDS -> IVO
                                _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_OK);
                                Thread.Sleep(100);

                                //CSMS -> WFH
                                _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.WAITING_FOR_HOST);
                                Thread.Sleep(100);
                                //Host commands to proceed.

                                index = WaitHandle.WaitAny(ewhHostVerif);

                                if (index == (int)eVerification.Fail)
                                {
                                    _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_FAIL);

                                    _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_STOPPED);
                                    Thread.Sleep(1000);
                                }
                                else
                                {
                                    _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_OK);

                                    //CarrierID is verified by host, and result is OK

                                    //Process starts

                                    //Process completes
                                    _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_COMPLETE);
                                    Thread.Sleep(1000);
                                }
                            }


                            //Carrier is undocked 
                            _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_UNLOAD);


                            _secsGemTool._carrierList.Remove(CarrierID);

                            //Unloading transfer starts.
                            _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);
                            Thread.Sleep(100);
                            //Unloading transfer completes.
                            _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_LOAD);
                            Thread.Sleep(1000);
                            _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.NOT_ASSOCIATION);

                            _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.CARRIER);

                            _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.CARRIER);
                        }

                    }
                    else
                    {
                        ProcAbnormal = true;
                        MessageBox.Show("目前已經有一組相同的CSTID", "移出當下的CSTID");
                    }
                }
                else
                {
                    ProcAbnormal = true;
                }
            }

            NormalRoundtrip1Start = false;
        }
        private void DoNormalRoundtrip2(object hcObj)
        {
            E87_HostCommand Obj = (E87_HostCommand)hcObj;
            NormalRoundtrip2Start = true;
            LoadPort lpObj = Obj.lpObj;
            SanwaCarrier carrierObj = Obj.carrierObj;

            string CarrierID = carrierObj.ObjID;
            lock (lpObj)
            {
                lock(carrierObj)
                {
                    _secsGemTool.ChangeReserviceState(lpObj, E87_RS.RESERVED);
                    _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.ASSOCIATION);
                    _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_NOT_READ);
                    _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_NOT_READ);


                    _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.IN_ACCESSED);
                    Thread.Sleep(1000);

                    _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);

                    Thread.Sleep(500);

                    _secsGemTool.ChangeReserviceState(lpObj, E87_RS.NOT_RESERVED);

                    int index = WaitHandle.WaitAny(ewhEQPVerif);

                    if (index == (int)eVerification.Fail)
                    {
                        _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.WAITING_FOR_HOST);

                        index = WaitHandle.WaitAny(ewhHostVerif);

                        if(index == (int)eVerification.Fail)
                        {
                            _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_FAILED);

                            _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_STOPPED);
                        }
                        else
                        {
                            _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_OK);

                            ewhSlotMapVerifOK.WaitOne();
                            ewhSlotMapVerifOK.Reset();

                            _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_OK);

                            //Process starts
                            Thread.Sleep(500);
                            _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_COMPLETE);
                        }
                    }
                    else
                    {
                        _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_OK);

                        ewhSlotMapVerifOK.WaitOne();
                        ewhSlotMapVerifOK.Reset();

                        _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_OK);

                        //Process starts
                        Thread.Sleep(500);
                        _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_COMPLETE);

                        //Process Completes
                    }

                    _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_UNLOAD);

                    _secsGemTool._carrierList.Remove(CarrierID);

                    _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);

                    Thread.Sleep(1000);

                    _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_LOAD);
                    _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.NOT_ASSOCIATION);
                    _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.CARRIER);
                    _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.CARRIER);
                }
            }


            NormalRoundtrip2Start = false;
        }
        private void DoNormalRoundtrip3(object Obj)
        {
            LoadPort lpObj = (LoadPort)Obj;
            SanwaCarrier carrierObj = null;

            lock (lpObj)
            {
                //Loading transfer starts
                if (_secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED))
                {
                    //Loading transfer completes
                    _secsGemTool._eventList.TryGetValue(E87_CE_LP1_LPTSM_SCT6_TRANSFER_BLOCKED, out SanwaEvent eventObj);

                    if (eventObj != null)
                        _secsGemTool.S6F11Async(eventObj._name);

                    //CarrierID is read
                    string CarrierID = "CSTID_01";

                    _secsGemTool._carrierList.TryGetValue(CarrierID, out carrierObj);

                    if (carrierObj == null)
                    {
                        carrierObj = new SanwaCarrier
                        {
                            ObjID = CarrierID,
                            LocationID = lpObj.Name
                        };

                        _secsGemTool._carrierList.Add(CarrierID, carrierObj);

                        lock (carrierObj)
                        {
                            _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.IN_ACCESSED);

                            //LCAS -> A
                            _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.ASSOCIATION);
                            Thread.Sleep(100);
                            //CIDS -> WFH
                            _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.WAITING_FOR_HOST);
                            Thread.Sleep(100);
                            //CSMS -> SNR
                            _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_NOT_READ);
                            Thread.Sleep(100);

                            int index = WaitHandle.WaitAny(ewhHostVerif);

                            if (index == (int)eVerification.Fail)
                            {
                                _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_FAILED);
                                //Thread.Sleep(1000);

                                _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_STOPPED);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                //CarrierID is verified by host, and result is OK
                                //CIDS -> IVO
                                _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_OK);
                                Thread.Sleep(100);

                                //Carrier-in starts

                                //CarrierID-in compeletes

                                _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_LOAD);

                                _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.NOT_ASSOCIATION);

                                //CSMS -> WFH
                                _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.WAITING_FOR_HOST);
                                Thread.Sleep(100);
                                //Host commands to proceed.

                                index = WaitHandle.WaitAny(ewhHostVerif);

                                if (index == (int)eVerification.Fail)
                                {
                                    _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_FAIL);

                                    _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_STOPPED);
                                    Thread.Sleep(1000);
                                }
                                else
                                {
                                    _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_OK);

                                    //CarrierID is verified by host, and result is OK

                                    //Process starts

                                    //Process completes
                                    _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_COMPLETE);

                                    ewhCarrierOut.WaitOne();

                                    _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.ASSOCIATION);
                                    Thread.Sleep(2000);


                                    _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);
                                }
                            }


                            //Carrier is undocked 
                            _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_UNLOAD);


                            _secsGemTool._carrierList.Remove(CarrierID);

                            //Unloading transfer starts.
                            _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);
                            Thread.Sleep(100);
                            //Unloading transfer completes.
                            _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_LOAD);
                            Thread.Sleep(1000);
                            _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.NOT_ASSOCIATION);

                            _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.CARRIER);

                            _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.CARRIER);
                        }

                    }
                    else
                    {
                        MessageBox.Show("目前已經有一組相同的CSTID", "移出當下的CSTID");
                    }
                }
            }
        }
        private void DoNormalRoundtrip5(object hcObj)
        {
            E87_HostCommand Obj = (E87_HostCommand)hcObj;

            //LoadPort lpObj = Obj.lpObj;
            SanwaCarrier carrierObj = Obj.carrierObj;

            string CarrierID = carrierObj.ObjID;
            lock (carrierObj)
            {
                //INR
                _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_NOT_READ);

                //SNR
                _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_NOT_READ);

                _loadPortList.TryGetValue("LP1", out LoadPort lpObj);

                _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.IN_ACCESSED);

                //TB
                _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);

                //A
                _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.ASSOCIATION);

                int index = WaitHandle.WaitAny(ewhEQPVerif);

                if (index == (int)eVerification.Fail)
                {
                    _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.WAITING_FOR_HOST);

                    index = WaitHandle.WaitAny(ewhHostVerif);

                    if (index == (int)eVerification.Fail)
                    {
                        _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_FAILED);

                        _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_STOPPED);
                    }
                    else
                    {
                        _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_OK);

                        ewhSlotMapVerifOK.WaitOne();
                        ewhSlotMapVerifOK.Reset();

                        _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_OK);

                        //Process starts
                        Thread.Sleep(500);
                        _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_COMPLETE);
                    }
                }
                else
                {
                    _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_OK);

                    ewhSlotMapVerifOK.WaitOne();
                    ewhSlotMapVerifOK.Reset();

                    _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_OK);

                    //Process starts
                    Thread.Sleep(500);
                    _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_COMPLETE);

                    //Process Completes
                }



                _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_UNLOAD);

                _secsGemTool._carrierList.Remove(CarrierID);

                _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);

                Thread.Sleep(1000);

                _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_LOAD);
                _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.NOT_ASSOCIATION);
                _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.CARRIER);
                _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.CARRIER);

            }

        }
        private void DoNormalRoundtrip7(object hcObj)
        {
            E87_HostCommand Obj = (E87_HostCommand)hcObj;

            LoadPort lpObj = Obj.lpObj;

            lock(lpObj)
            {
                //R
                _secsGemTool.ChangeReserviceState(lpObj, E87_RS.RESERVED);

                //Loading transfer start

                //TB
                _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);

                //Loading transfer compeltes

                SanwaCarrier carrierObj = new SanwaCarrier
                {
                    ObjID = "CSTID_01",
                    LocationID = lpObj.Name
                };

                _secsGemTool._carrierList.Add(carrierObj.ObjID, carrierObj);

                lock(carrierObj)
                {
                    _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.IN_ACCESSED);

                    //NR
                    _secsGemTool.ChangeReserviceState(lpObj, E87_RS.NOT_RESERVED);

                    _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.ASSOCIATION);

                    _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.WAITING_FOR_HOST);

                    _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_NOT_READ);

                    //CarrierID is verified by host, and result is OK. 
                    int index = WaitHandle.WaitAny(ewhHostVerif);

                    if(index == (int)eVerification.Fail)
                    {
                        _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_FAILED);

                        _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_STOPPED);
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.ID_VERIFICATION_OK);

                        _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.WAITING_FOR_HOST);


                        index = WaitHandle.WaitAny(ewhHostVerif);

                        if(index == (int)eVerification.Fail)
                        {
                            _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_FAIL);

                            _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_STOPPED);
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.SLOT_MAP_VERIFICATION_OK);

                            //Process starts
                            Thread.Sleep(500);
                            _secsGemTool.CarrierAccessingStatus(carrierObj, E87_CAS.CARRIER_COMPLETE);
                            Thread.Sleep(1000);
                            //Process Completes
                        }

                    }

                    _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_UNLOAD);

                    _secsGemTool._carrierList.Remove(carrierObj.ObjID);

                    _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.TRANSFER_BLOCKED);

                    Thread.Sleep(1000);

                    _secsGemTool.ChangeLoadPortState(lpObj, E87_LPTS.READY_TO_LOAD);
                    _secsGemTool.LoadPortCarrierAssociated(lpObj, carrierObj, E87_ASSOCIATED.NOT_ASSOCIATION);
                    _secsGemTool.CarrierIDStatusChange(carrierObj, E87_CIDS.CARRIER);
                    _secsGemTool.CarrierSlotMapStatusChange(carrierObj, E87_CSMS.CARRIER);
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            ewhEQPVerif[(int)eVerification.OK].Set();
        }
        private void btnSlopMapVerifOK_Click(object sender, EventArgs e)
        {
            ewhSlotMapVerifOK.Set();
        }

        private void btnIDVerifFail_Click(object sender, EventArgs e)
        {
            ewhEQPVerif[(int)eVerification.Fail].Set();
        }

        private void btnE87NR3_Click(object sender, EventArgs e)
        {
            LoadPort lpObj = null;

            if (!btnLP1.Enabled)
            {
                _loadPortList.TryGetValue("LP1", out lpObj);
            }
            else
            {
                _loadPortList.TryGetValue("LP2", out lpObj);
            }

            if (lpObj == null) return;

            if (NormalRoundtrip1Start) return;

            if (lpObj.CurrentState != E87_LPTS.READY_TO_LOAD)
            {
                MessageBox.Show("需切換到 【Ready to Load】", "三和技研好棒棒");
                return;
            }

            if (lpObj.Associated != E87_ASSOCIATED.NOT_ASSOCIATION)
            {
                MessageBox.Show("需切換到 【Not Association】", "三和技研好棒棒");
                return;
            }


            ThreadPool.QueueUserWorkItem(new WaitCallback(DoNormalRoundtrip3), lpObj);
        }
    }
}
