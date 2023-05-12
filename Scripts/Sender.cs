using System;
using System.IO.Ports;
using UnityEngine;
using System.Threading.Tasks;


namespace SerialComm
{ 
        public class Sender
        {
        public async void SendGCode(SerialPort port, string command)
        {
            await Task.Delay(100);
            port.WriteLine(command);
            Debug.Log($"Trimitem pe {port} : {command}");
        }
    }
}

