using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SawanSecsDll;
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
#else
            _secsGemTool.eqpSVFileName = "..\\Release\\Csv\\EqpSV.csv";
            _secsGemTool.eqpEventFileName = "..\\Release\\Csv\\EqpEvent.csv";
            _secsGemTool.eqpECFileName = "..\\Release\\Csv\\EpqEC.csv";
            _secsGemTool.eqpAlarmFileName = "..\\Debug\\Csv\\EpqAlarm.csv";

#endif
            _secsGemTool.ConnectionStateChangedEvent += new SanwaSecs.ConnectionStateChanged(UpdateConnectionState);
            //_secsGemTool.PrimaryMessageReceivedEvent += new SanwaSecs.
            _secsGemTool.ChangeControlStateEvent += delegate{ UpdateControlState(_secsGemTool._currentState);}; 
            _secsGemTool.Initialize();

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

        public void UpdateConnectionState()
        {
            this.Invoke((MethodInvoker)delegate
            {
                lbConnectStateChange.Text = _secsGemTool.State.ToString();

                if (_secsGemTool.State == SawanSecsDll.ConnectionState.Connected ||
                    _secsGemTool.State == SawanSecsDll.ConnectionState.Selected)
                {
                    lbConnectStateChange.BackColor = Color.Lime;
                }
                else
                {
                    lbConnectStateChange.BackColor = Color.Red;
                }
            });
        }

        public void UpdateControlState(CONTROL_STATE state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                switch (state)
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

                    case CONTROL_STATE.ON_LINE_LOCATE:
                        lblControlState.Text = "ON_LINE_LOCATE";
                        lblControlState.BackColor = Color.Lime;
                        break;

                    case CONTROL_STATE.ON_LINE_REMOTE:
                        lblControlState.Text = "ON_LINE_REMOTE";
                        lblControlState.BackColor = Color.Lime;
                        break;
                }
            });
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
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"<-- [0x{systembyte:X8}] {msg.ToSml()}\n");
                });
            }

            public void MessageOut(SecsMessage msg, int systembyte)
            {
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Black;
                    _form.richTextBox1.AppendText($"--> [0x{systembyte:X8}] {msg.ToSml()}\n");
                });
            }

            public void Info(string msg)
            {
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Blue;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }

            public void Warning(string msg)
            {
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Red;
                    _form.richTextBox1.AppendText($"{msg}\n");
                });
            }

            public void Error(string msg, Exception ex = null)
            {
                _form.Invoke((MethodInvoker)delegate {
                    _form.richTextBox1.SelectionColor = Color.Red;
                    _form.richTextBox1.AppendText($"{msg}\n");
                    _form.richTextBox1.SelectionColor = Color.Gray;
                    _form.richTextBox1.AppendText($"{ex}\n");
                });
            }

            public void Debug(string msg)
            {
                _form.Invoke((MethodInvoker)delegate {
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
            if (_secsGemTool.State != SawanSecsDll.ConnectionState.Selected)
                return;
            //if (string.IsNullOrWhiteSpace(tbShowSML.Text))
            //    return;

            try
            {
                SecsMessage secsMsg = _secsMessagesList[listboxSmlFile.SelectedIndex];
                await _secsGemTool.SetStreamFunction(secsMsg);
                //_secsGemTool.SetStreamFunction(tbShowSML.Text.ToSecsMessage());
            }
            catch (SecsException ex)
            {
                //txtRecvSecondary.Text = ex.Message;
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
            _secsGemTool.SetSV("5", 1);
            _secsGemTool.SetSV("6", 3);
            _secsGemTool.SetSV("10", 1);
            _secsGemTool.SetSV("12",2);
            _secsGemTool.SetSV("13", 3);
            _secsGemTool.SetSV("24", "CCCC");
            _secsGemTool.SetSV("55", "DDDD");
            _secsGemTool.SetSV("5001", 0);

            byte[] Test = { 0x00, 0xFF };
            _secsGemTool.SetSV("10004", Test);
            

            Dictionary<string, SanwaSV> _svtempList = new Dictionary<string, SanwaSV>();

            SanwaSV temp = new SanwaSV();
            temp._format = SecsFormat.U4;
            temp._id = "20";
            temp._value = (uint)100;

            _svtempList.Add(temp._id, temp);

            temp = new SanwaSV();
            temp._format = SecsFormat.U4;
            temp._id = "21";
            temp._value = (uint)200;
            _svtempList.Add(temp._id, temp);

            _secsGemTool.SetSV("41", _svtempList);
            //模擬給值 start ++

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
            await _secsGemTool.ChangeToOnLineLocateState();
        }

        private async void btnEqpOnLineRemote_Click(object sender, EventArgs e)
        {
            await _secsGemTool.ChangeToOnLineRemoteState();
        }

        private void btnAlarmSetTest_Click(object sender, EventArgs e)
        {
            _secsGemTool.S5F1SetAlarmReport(AlarmID.ALARM_PUMP_PRESS, true);
        }

        private void btnAlarmresetTest_Click(object sender, EventArgs e)
        {
            _secsGemTool.S5F1SetAlarmReport(AlarmID.ALARM_PUMP_PRESS, false);
        }

        private void btnTerminalMessage_Click(object sender, EventArgs e)
        {
            _secsGemTool.S10F1SetTerminalMSG("Try to Test");
        }
    }
}
