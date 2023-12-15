using System;
using System.IO;
using System.IO.Ports;
using Mono;

class Program
{
    static void Main()
    {
        // 시리얼 포트 경로
        string portPath = "/dev/ttyUSB0"; // 리눅스 시스템에서 사용 중인 포트에 따라 변경 필요

        // 시리얼 포트 설정
        using (FileStream serialStream = new FileStream(portPath, FileMode.Open, FileAccess.ReadWrite))
        {
            // Mono.Posix.Termios.TermiosSettings를 사용하여 시리얼 포트 설정
            var settings = new Mono.Unix.Termios.TermiosSettings();
            settings.BaudRate = BaudRate.B9600;
            settings.DataBits = 8;
            settings.Parity = Mono.Unix.Termios.Parity.None;
            settings.StopBits = Mono.Unix.Termios.StopBits.One;
            settings.CFlag = Mono.Unix.Termios.CFlag.CREAD | Mono.Unix.Termios.CFlag.CLOCAL;

            Mono.Unix.Native.Syscall.tcsetattr(serialStream.Handle.ToInt32(), Mono.Unix.Termios.TermiosActions.TCSANOW, ref settings);

            // 데이터 송신
            string sendData = "Hello, Linux Serial Port!";
            byte[] sendDataBytes = System.Text.Encoding.ASCII.GetBytes(sendData);
            serialStream.Write(sendDataBytes, 0, sendDataBytes.Length);

            // 데이터 수신
            byte[] receiveDataBytes = new byte[1024];
            int bytesRead = serialStream.Read(receiveDataBytes, 0, receiveDataBytes.Length);
            string receivedData = System.Text.Encoding.ASCII.GetString(receiveDataBytes, 0, bytesRead);
            Console.WriteLine("Received data: " + receivedData);
        }
    }
}