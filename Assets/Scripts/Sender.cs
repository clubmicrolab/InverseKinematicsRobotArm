using System;
using System.IO.Ports;
using UnityEngine;

namespace SerialComm

{ 

        public class Sender
        {
        public void SendGCode(SerialPort port, string command)
        {
                
                port.WriteLine(command);
                Debug.Log($"Trimitem pe {port} : {command}");

        }

    }
}

