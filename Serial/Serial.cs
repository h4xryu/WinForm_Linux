using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

public class Serial : Form
{
    //====================================================================
    public bool _continue;
    private SerialPort serialPort;
    private ComboBox comboBoxPorts;
    private TextBox dynamicTextBox;
    private RichTextBox TerminalTextBox;
    private Label LabelCommand;
    private Button btnConnect;
    //====================================================================
    public Serial()
    {
        serialPort = new SerialPort();
        this.Text = "Serial";
        this.Size = new System.Drawing.Size(818, 497);
        this.BackColor = System.Drawing.Color.Gainsboro;
        this.AllowDrop = false;
        this.AutoValidate = AutoValidate.EnablePreventFocusChange;
        this.DoubleBuffered = false;
        this.AutoScaleMode = AutoScaleMode.Font;
        this.AutoScroll = false;
        // this.Font = new Font(Font.Bold,10);

        AddTerminalTextBox();
        AddDynamicTextBox();
        AddSendBtn();
        AddConnectBtn();
        AddComboBoxPorts();
        AddLabelCommand();
        InitializeSerialPort();

    }
    //====================================================================
    private void InitializeSerialPort()
    {

        //Thread readThread = new Thread(new ThreadStart(ReadThreadMethod));
        string[] serialPorts = new string[1000];
        try
        {
            serialPorts = Directory.GetFiles("/dev/")
                .Where(p => p.StartsWith("/dev/ttyA") || p.StartsWith("/dev/ttyUSB") || p.StartsWith("/dev/ttyS"))
                .ToArray();

            if (serialPorts.Length > 0)
            {
                // Console.WriteLine("Available serial ports:");
                // foreach (string port in serialPorts)
                // {
                //     Console.WriteLine(port);
                // }

                // Allow the user to set the appropriate properties.
                serialPort.PortName = serialPorts[0];
                serialPort.BaudRate = 115200;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;

                // Set the read/write timeouts
                serialPort.ReadTimeout = 500;
                serialPort.WriteTimeout = 500;

                _continue = true;
                //readThread.Start();
                // 사용 가능한 포트 이름을 가져와 콤보 박스에 추가합니다.

                comboBoxPorts.Items.AddRange(serialPorts);


                //readThread.Join();
                //readThread_stop();
                serialPort.Close();
            }
            else
            {
                Console.WriteLine("No serial ports found in /dev directory.");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while getting serial ports: {ex.Message}");
        }
    }

    private void ReadThreadMethod()
    {
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        string message;
        string name;
        name = Console.ReadLine();
    // 스레드에서 실행될 코드를 작성합니다.
    // 예: 시리얼 데이터를 읽고 처리하는 등의 작업을 수행합니다.
        while (_continue)
        {
            message = Console.ReadLine();

            if (stringComparer.Equals("quit", message))
            {
                _continue = false;
            }
            else
            {
                serialPort.WriteLine(
                    String.Format("<{0}>: {1}", name, message));
            }
        }
        name = Console.ReadLine();

    }
    private void readThread_stop(){
        _continue = false;
    }

    private void comboBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 콤보 박스에서 선택된 포트를 시리얼 포트에 할당합니다.
        if (comboBoxPorts.SelectedItem != null)
        {
            serialPort.PortName = comboBoxPorts.SelectedItem.ToString();
        }
    }
    private void MainForm_Load(object sender, EventArgs e)
    {
        AddDynamicTextBox();
    }
    //====================================================================
    private void AddSendBtn()
    {
        Button btn = new Button();
        btn.Text = "send";
        btn.TabIndex = 4;
        btn.Font = new Font("Aerial",10F);
        btn.Size = new System.Drawing.Size(82, 22);
        btn.Location = new System.Drawing.Point(694, 395);
        btn.Click += new EventHandler(Btn_Clicked);
        this.Controls.Add(btn);
    }
    private void AddConnectBtn()
    {
        btnConnect = new Button();
        btnConnect.Text = "Connect";
        btnConnect.Size = new Size(60,22);
        btnConnect.Location = new Point(130,395);
        btnConnect.Click += new EventHandler(btnConnect_Clicked);
        this.Controls.Add(btnConnect);
    }
    private void AddComboBoxPorts()
    {
        comboBoxPorts = new ComboBox();
        comboBoxPorts.Text = "Select Port";
        comboBoxPorts.Location = new System.Drawing.Point(26,395);
        comboBoxPorts.Size = new System.Drawing.Size(100,30);
        this.Controls.Add(comboBoxPorts);
    }

    private void AddDynamicTextBox()
    {
        // 동적으로 TextBox를 생성합니다.
        dynamicTextBox = new TextBox();

        // TextBox의 속성을 설정합니다.
        dynamicTextBox.Name = "dynamicTextBox";
        dynamicTextBox.Location = new System.Drawing.Point(282, 395);
        dynamicTextBox.Size = new System.Drawing.Size(405, 30);

        // 폼에 TextBox를 추가합니다.
        this.Controls.Add(dynamicTextBox);
    }
    private void AddTerminalTextBox()
    {
        TerminalTextBox = new RichTextBox();
        TerminalTextBox.Size = new Size(750,311);
        TerminalTextBox.Location = new Point(26,38);
        TerminalTextBox.TabIndex = 16;
        
        // TerminalTextBox.BackColor = Color.Black;
        TerminalTextBox.ForeColor = Color.Lime;
        TerminalTextBox.Text = "";
        TerminalTextBox.TextChanged += new EventHandler(TerminalTextBox_changed);
        TerminalTextBox.ReadOnly = true;
        this.Controls.Add(TerminalTextBox);
    }
    private void AddLabelCommand(){
        LabelCommand = new Label();
        LabelCommand.Location = new Point(200,395);
        LabelCommand.Size = new Size(95,23);
        LabelCommand.Font = new Font("Aerial", 10F);
        LabelCommand.Text = "Command";
        this.Controls.Add(LabelCommand);
    }
    //====================================================================

    private void Btn_Clicked(object sender, EventArgs e)
    {
        //MessageBox.Show("success");
        // TerminalTextBox.ReadOnly = false;
        string input;
        try
        {
            //serialPort.Open(); //포트 열기

            TerminalTextBox.Text = "Enter data to send (type 'exit' to quit):";

            

            input = dynamicTextBox.Text;

            if (input.ToLower() != "exit")
            {
                // 데이터 쓰기
                TerminalTextBox.Text = input;
            }
            MessageBox.Show(input);

        }
        finally {}
        // TerminalTextBox.ReadOnly = true;

    }

    private void btnConnect_Clicked(object sender, EventArgs e){
        // 선택된 포트를 열고 데이터 수신을 시작합니다.
        if (!serialPort.IsOpen)
        {
            try
            {
                serialPort.PortName = comboBoxPorts.Text;
                serialPort.BaudRate = 115200;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                serialPort.Open();
                TerminalTextBox.Text = "Serial Opened.";
                comboBoxPorts.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("포트를 열 수 없습니다: " + ex.Message);
            }
        }
        else
        {
            MessageBox.Show("포트가 이미 열려 있습니다.");
        }
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e){
        this.Invoke(new EventHandler(Serial_Received));
    }
    private void Serial_Received(object sender, EventArgs e){
        int ReciveData = int.Parse(serialPort.ReadLine());
        TerminalTextBox.Text += string.Format("{0:X2}",ReciveData);
    }
    private void TerminalTextBox_changed(object sender, EventArgs e){
        
    }


    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        // 폼이 닫힐 때 시리얼 포트를 닫습니다.
        if (serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
    //====================================================================
    static public void Main()
    {
        Application.Run(new Serial());
    }

}
