using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Text;
using System.Threading.Tasks;
using SerialComm;
using ControlManual;
using ControlAuto;
using UnityEditor.Experimental.GraphView;

public class Controller : MonoBehaviour
{
    ControlManual.ControlManual controlManual = new ControlManual.ControlManual();
    ControlAuto.ControlAuto controlAuto = new ControlAuto.ControlAuto();
    SerialPort port = new SerialPort("COM21", 9600);
    Sender sender = new Sender();
    Transform[] objectChildren;

    private long lastUpdateTime;
    void Start()
    {
        port.Open();
        System.Threading.Thread.Sleep(200); //Delay of 1 second
        objectChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in objectChildren)
        {
            child.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        lastUpdateTime = GetCurrentTimeMillis();
        sender.SendGCode(port, $"T0");
    }

    void Update()
    {
        long currentTime = GetCurrentTimeMillis();
        long elapsedTime = currentTime - lastUpdateTime;
        if (elapsedTime >= 20) // 1 second delay
        {
            controlManual.Execute(objectChildren, port,sender);
            //controlAuto.Execute(objectChildren, port,sender);
            lastUpdateTime = currentTime;
        }
    }

    private long GetCurrentTimeMillis()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}