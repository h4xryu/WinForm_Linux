using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

public class Draw : Form
{
    public bool _continue;
    private SerialPort serialPort;
    private ComboBox comboBoxPorts;
    public Draw()
    {
        serialPort = new SerialPort();
        this.Text = "test";
        this.Size = new System.Drawing.Size(480, 320);
        AddDynamicTextBox();
        AddSendBtn();
        AddComboBoxPorts();
        InitializeSerialPort();
    }
    //====================================================================
    private void InitializeSerialPort()
    {

        Thread readThread = new Thread(new ThreadStart(ReadThreadMethod));
        string[] serialPorts = new string[1000];
        try
        {
            serialPorts = Directory.GetFiles("/dev/")
                .Where(p => p.StartsWith("/dev/ttyA") || p.StartsWith("/dev/ttyUSB") || p.StartsWith("/dev/ttyS"))
                .ToArray();

            if (serialPorts.Length > 0)
            {
                Console.WriteLine("Available serial ports:");
                foreach (string port in serialPorts)
                {
                    Console.WriteLine(port);
                }

                // Allow the user to set the appropriate properties.
                serialPort.PortName = serialPorts[0];
                serialPort.BaudRate = 9600;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;

                // Set the read/write timeouts
                serialPort.ReadTimeout = 500;
                serialPort.WriteTimeout = 500;

                _continue = true;
                //readThread.Start();
                // 사용 가능한 포트 이름을 가져와 콤보 박스에 추가합니다.

                comboBoxPorts.Items.AddRange(serialPorts);


                readThread.Join();
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

    private void AddSendBtn()
    {
        Button btn = new Button();
        btn.Text = "send";
        btn.Size = new System.Drawing.Size(40, 20);
        btn.Location = new System.Drawing.Point(210, 50);
        btn.Click += new EventHandler(Btn_Clicked);
        this.Controls.Add(btn);
    }
    private void AddComboBoxPorts()
    {
        comboBoxPorts = new ComboBox();
        this.Controls.Add(comboBoxPorts);
    }

    private void AddDynamicTextBox()
    {
        // 동적으로 TextBox를 생성합니다.
        TextBox dynamicTextBox = new TextBox();

        // TextBox의 속성을 설정합니다.
        dynamicTextBox.Name = "dynamicTextBox";
        dynamicTextBox.Location = new System.Drawing.Point(50, 50);
        dynamicTextBox.Size = new System.Drawing.Size(150, 20);

        // 폼에 TextBox를 추가합니다.
        this.Controls.Add(dynamicTextBox);
    }
    //====================================================================
    private void Btn_Clicked(object sender, EventArgs e)
    {
        MessageBox.Show("success");

    }
    private void buttonOpenPort_Click(object sender, EventArgs e)
    {
        // 선택된 포트를 열고 데이터 수신을 시작합니다.
        if (!serialPort.IsOpen)
        {
            try
            {
                serialPort.Open();
                MessageBox.Show("포트가 열렸습니다.");
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
        Application.Run(new Draw());
    }

}
