using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VirtualKeyboard
{
    public partial class MainForm : Form
    {
        private SerialPort serialPort = null;

        private Dictionary<Keys, byte[]> keyCodes = new Dictionary<Keys, byte[]>();

        private readonly byte SHIFT_LEFT = 0x12;
        private readonly byte CTRL_LEFT = 0x14;
        private readonly byte ALT_LEFT = 0x11;
        private readonly byte RELEASE_KEY = 0xf0;

        private bool LShiftDown = false;
        private bool RShiftDown = false;
        private bool LCtrlDown = false;
        private bool RCtrlDown = false;
        private bool LAltDown = false;
        private bool RAltDown = false;

        private Label LShiftButton;
        private Label RShiftButton;
        private Label LCtrlButton;
        private Label RCtrlButton;
        private Label LAltButton;
        private Label RAltButton;

        private void InitializeLabels()
        {
            foreach (Control control in this.Controls)
            {
                Label label = control as Label;
                if (label != null)
                {
                    if (label.Tag != null)
                    {
                        label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyOnMouseDown);
                        label.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyOnMouseUp);
                        Keys keyCode = (Keys)Enum.Parse(typeof(Keys), (string)label.Tag);
                        if (keyCode == Keys.LShiftKey)
                        {
                            LShiftButton = label;
                        }
                        else if (keyCode == Keys.RShiftKey)
                        {
                            RShiftButton = label;
                        }
                        else if (keyCode == Keys.LControlKey)
                        {
                            LCtrlButton = label;
                        }
                        else if (keyCode == Keys.RControlKey)
                        {
                            RCtrlButton = label;
                        }
                        else if (keyCode == Keys.LMenu)
                        {
                            LAltButton = label;
                        }
                        else if (keyCode == Keys.RMenu)
                        {
                            RAltButton = label;
                        }
                    }
                }
            }
        }

        private void InitializeKeyCode()
        {
            AddKeyCode(Keys.Space, 0x29);
            AddKeyCode(Keys.Escape, 0x76);
            AddKeyCode(Keys.Tab, 0x0d);

            AddKeyCode(Keys.F1, 0x05);
            AddKeyCode(Keys.F2, 0x06);
            AddKeyCode(Keys.F3, 0x04);
            AddKeyCode(Keys.F4, 0x0c);
            AddKeyCode(Keys.F5, 0x03);
            AddKeyCode(Keys.F6, 0x0b);
            AddKeyCode(Keys.F7, 0x83);
            AddKeyCode(Keys.F8, 0x0a);
            AddKeyCode(Keys.F9, 0x01);
            AddKeyCode(Keys.F10, 0x09);
            AddKeyCode(Keys.F11, 0x78);
            AddKeyCode(Keys.F12, 0x07);

            AddKeyCode(Keys.A, 0x1c);
            AddKeyCode(Keys.B, 0x32);
            AddKeyCode(Keys.C, 0x21);
            AddKeyCode(Keys.D, 0x23);
            AddKeyCode(Keys.E, 0x24);
            AddKeyCode(Keys.F, 0x2b);
            AddKeyCode(Keys.G, 0x34);
            AddKeyCode(Keys.H, 0x33);
            AddKeyCode(Keys.I, 0x43);
            AddKeyCode(Keys.J, 0x3b);
            AddKeyCode(Keys.K, 0x42);
            AddKeyCode(Keys.L, 0x4b);
            AddKeyCode(Keys.M, 0x3a);
            AddKeyCode(Keys.N, 0x31);
            AddKeyCode(Keys.O, 0x44);
            AddKeyCode(Keys.P, 0x4d);
            AddKeyCode(Keys.Q, 0x15);
            AddKeyCode(Keys.R, 0x2d);
            AddKeyCode(Keys.S, 0x1b);
            AddKeyCode(Keys.T, 0x2c);
            AddKeyCode(Keys.U, 0x3c);
            AddKeyCode(Keys.V, 0x2a);
            AddKeyCode(Keys.W, 0x1d);
            AddKeyCode(Keys.X, 0x22);
            AddKeyCode(Keys.Y, 0x35);
            AddKeyCode(Keys.Z, 0x1a);

            AddKeyCode(Keys.Oemtilde, 0x0e); // `~

            AddKeyCode(Keys.D1, 0x16);
            AddKeyCode(Keys.D2, 0x1e);
            AddKeyCode(Keys.D3, 0x26);
            AddKeyCode(Keys.D4, 0x25);
            AddKeyCode(Keys.D5, 0x2e);
            AddKeyCode(Keys.D6, 0x36);
            AddKeyCode(Keys.D7, 0x3d);
            AddKeyCode(Keys.D8, 0x3e);
            AddKeyCode(Keys.D9, 0x46);
            AddKeyCode(Keys.D0, 0x45);

            AddKeyCode(Keys.OemMinus, 0x4e); // _-
            AddKeyCode(Keys.Oemplus, 0x55); // +=
            AddKeyCode(Keys.Back, 0x66); // backspace

            AddKeyCode(Keys.OemOpenBrackets, 0x54); // [{
            AddKeyCode(Keys.OemCloseBrackets, 0x5b); // ]}
            AddKeyCode(Keys.OemPipe, 0x5d); // |\
            AddKeyCode(Keys.OemSemicolon, 0x4c); // ;:
            AddKeyCode(Keys.OemQuotes, 0x52); // '"
            AddKeyCode(Keys.Enter, 0x5a); // Enter
            AddKeyCode(Keys.Oemcomma, 0x41); // ,<
            AddKeyCode(Keys.OemPeriod, 0x49); // .>
            AddKeyCode(Keys.OemQuestion, 0x4a); // /?

            AddKeyCode(Keys.Delete, 0xe0, 0x71); // Delete
            AddKeyCode(Keys.Home, 0xe0, 0x6c); // Home
            AddKeyCode(Keys.End, 0xe0, 0x69); // End
            AddKeyCode(Keys.PageUp, 0xe0, 0x7d); // PageUp
            AddKeyCode(Keys.PageDown, 0xe0, 0x7a); // PageDown
            AddKeyCode(Keys.Up, 0xe0, 0x75); // Up
            AddKeyCode(Keys.Down, 0xe0, 0x72); // Down
            AddKeyCode(Keys.Left, 0xe0, 0x6b); // Left
            AddKeyCode(Keys.Right, 0xe0, 0x74); // Right
        }

        private void AddKeyCode(Keys k, byte b1)
        {
            keyCodes.Add(k, new byte[] { b1 });
        }

        private void AddKeyCode(Keys k, byte b1, byte b2)
        {
            keyCodes.Add(k, new byte[] { b1, b2 });
        }

        public MainForm()
        {
            InitializeComponent();
            InitializeLabels();
            InitializeKeyCode();
        }

        private readonly Color NormalColor = SystemColors.Control;
        private readonly Color PressedColor = SystemColors.ControlDark;

        private void KeyOnMouseDown(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                label.BackColor = PressedColor;
            }
        }

        private void KeyOnMouseUp(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                Keys keyCode = (Keys)Enum.Parse(typeof(Keys), (string)label.Tag);
                if (keyCode == Keys.CapsLock)
                {
                    label.BackColor = NormalColor;
                    SetStatus("Caps lock is ignored.");
                    return;
                }
                // check shift pressed:
                if (label == LShiftButton)
                {
                    if (LShiftDown)
                    {
                        LShiftDown = false;
                        LShiftButton.BackColor = NormalColor;
                    }
                    else
                    {
                        LShiftDown = true;
                        RShiftDown = false;
                        LShiftButton.BackColor = PressedColor;
                        RShiftButton.BackColor = NormalColor;
                    }
                    return;
                }
                if (label == RShiftButton)
                {
                    if (RShiftDown)
                    {
                        RShiftDown = false;
                        RShiftButton.BackColor = NormalColor;
                    }
                    else
                    {
                        RShiftDown = true;
                        LShiftDown = false;
                        RShiftButton.BackColor = PressedColor;
                        LShiftButton.BackColor = NormalColor;
                    }
                    return;
                }
                // check ctrl pressed:
                if (label == LCtrlButton)
                {
                    if (LCtrlDown)
                    {
                        LCtrlDown = false;
                        LCtrlButton.BackColor = NormalColor;
                    }
                    else
                    {
                        LCtrlDown = true;
                        RCtrlDown = false;
                        LCtrlButton.BackColor = PressedColor;
                        RCtrlButton.BackColor = NormalColor;
                    }
                    return;
                }
                if (label == RCtrlButton)
                {
                    if (RCtrlDown)
                    {
                        RCtrlDown = false;
                        RCtrlButton.BackColor = NormalColor;
                    }
                    else
                    {
                        RCtrlDown = true;
                        LCtrlDown = false;
                        RCtrlButton.BackColor = PressedColor;
                        LCtrlButton.BackColor = NormalColor;
                    }
                    return;
                }
                // check alt pressed:
                if (label == LAltButton)
                {
                    if (LAltDown)
                    {
                        LAltDown = false;
                        LAltButton.BackColor = NormalColor;
                    }
                    else
                    {
                        LAltDown = true;
                        RAltDown = false;
                        LAltButton.BackColor = PressedColor;
                        RAltButton.BackColor = NormalColor;
                    }
                    return;
                }
                if (label == RAltButton)
                {
                    if (RAltDown)
                    {
                        RAltDown = false;
                        RAltButton.BackColor = NormalColor;
                    }
                    else
                    {
                        RAltDown = true;
                        LAltDown = false;
                        RAltButton.BackColor = PressedColor;
                        LAltButton.BackColor = NormalColor;
                    }
                    return;
                }

                label.BackColor = NormalColor;
                SendKey(keyCode, LShiftDown || RShiftDown, LCtrlDown || RCtrlDown, LAltDown || RAltDown);
                // clear shift / ctrl / alt status:
                LShiftDown = RShiftDown = LCtrlDown = RCtrlDown = LAltDown = RAltDown = false;
                LShiftButton.BackColor = NormalColor;
                RShiftButton.BackColor = NormalColor;
                LCtrlButton.BackColor = NormalColor;
                RCtrlButton.BackColor = NormalColor;
                LAltButton.BackColor = NormalColor;
                RAltButton.BackColor = NormalColor;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            SendKey(e.KeyCode, e.Shift, e.Control, e.Alt);
        }

        private void SendKey(Keys keyCode, bool shift, bool ctrl, bool alt)
        {
            if (!this.keyCodes.ContainsKey(keyCode))
            {
                Debug.WriteLine("Skip send key " + keyCode);
                return;
            }
            List<byte> buffer = new List<byte>();
            if (shift)
            {
                buffer.Add(SHIFT_LEFT);
            }
            if (ctrl)
            {
                buffer.Add(CTRL_LEFT);
            }
            if (alt)
            {
                buffer.Add(ALT_LEFT);
            }

            byte[] codes = keyCodes[keyCode];
            foreach (byte code in codes)
            {
                buffer.Add(code);
            }

            if (codes[0] == 0xe0)
            {
                buffer.Add(codes[0]);
                buffer.Add(RELEASE_KEY);
                buffer.Add(codes[1]);
            }
            else
            {
                buffer.Add(RELEASE_KEY);
                buffer.Add(codes[0]);
            }

            if (alt)
            {
                buffer.Add(RELEASE_KEY);
                buffer.Add(ALT_LEFT);
            }
            if (ctrl)
            {
                buffer.Add(RELEASE_KEY);
                buffer.Add(CTRL_LEFT);
            }
            if (shift)
            {
                buffer.Add(RELEASE_KEY);
                buffer.Add(SHIFT_LEFT);
            }
            StringBuilder sb = new StringBuilder(200);
            sb.Append("Key = ");
            if (shift)
            {
                sb.Append("SHIFT + ");
            }
            if (ctrl)
            {
                sb.Append("CTRL + ");
            }
            if (alt)
            {
                sb.Append("ALT + ");
            }
            sb.Append(keyCode);
            sb.Append(". Code =");
            foreach (byte code in buffer)
            {
                sb.Append(" ").Append(code.ToString("X2"));
            }
            string status = sb.ToString();
            SetStatus(status);
            WriteBytesAsync(buffer.ToArray());
        }

        private object bufferLock = new object();
        private Queue<byte> bufferQueue = new Queue<byte>();

        private void WriteBytesAsync(byte[] buffer)
        {
            if (this.serialPort != null)
            {
                lock (bufferLock)
                {
                    foreach (byte b in buffer)
                    {
                        this.bufferQueue.Enqueue(b);
                        Debug.WriteLine("Will send byte " + b.ToString("X2") + "...");
                    }
                }
            }
        }

        private void SendSerialDataWork()
        {
            Debug.WriteLine("Serial port sending thread started.");
            while (this.serialPort != null)
            {
                Thread.Sleep(10);
                lock (bufferLock)
                {
                    if (bufferQueue.Count > 0)
                    {
                        byte b = bufferQueue.Dequeue();
                        try
                        {
                            this.serialPort.Write(new byte[] { b }, 0, 1);
                            Debug.WriteLine("Byte " + b.ToString("X2") + " sent.");
                        }
                        catch (Exception ex)
                        {
                            ClosePort();
                            Debug.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            Debug.WriteLine("Serial port sending thread ended.");
        }

        private void ToggleSerialPort(object sender, EventArgs e)
        {
            if (this.serialPort == null)
            {
                ConfigForm config = new ConfigForm();
                config.ShowDialog();
                if (config.serialPort != null)
                {
                    OpenPort(config.serialPort);
                }
            }
            else
            {
                ClosePort();
            }
        }

        private void OpenPort(SerialPort port)
        {
            this.serialPort = port;
            this.lblPower.BackColor = Color.Green;
            SetStatus("Serial port is open.");
            this.bufferQueue.Clear();
            Thread sending = new Thread(SendSerialDataWork);
            sending.Start();
        }

        private void ClosePort()
        {
            this.serialPort.Close();
            this.serialPort = null;
            this.lblPower.BackColor = Color.DarkRed;
            SetStatus("Serial port is closed.");
        }

        private void SetStatus(string text)
        {
            this.stText.Text = text;
            Debug.WriteLine("Set status: " + text);
        }

        private void OpenWebsite(object sender, EventArgs e)
        {
            ToolStripStatusLabel label = sender as ToolStripStatusLabel;
            if (label != null)
            {
                Debug.WriteLine("Open url: " + label.Text);
                ProcessStartInfo psi = new ProcessStartInfo("cmd", "/c start " + label.Text);
                try
                {
                    System.Diagnostics.Process.Start(psi);
                }
                catch (Win32Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
