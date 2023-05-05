using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Text;
using System.Threading.Tasks;
using SerialComm;


public class IKSolver : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;

    public GameObject Parent;
    public GameObject Target;

    List<Vector3> effectorPositions = new List<Vector3>();

    SerialPort port = new SerialPort("COM5", 9600);
    Sender sender = new Sender();

    bool isT0Presed = true;

    //Coeficienti pentru calibrare la brate:
    int bodyCalibration = 0;
    int shoulderCalibration = 311;
    int elbowCalibration = 330;
    int wristCalibration = 0;
    int palmCalibration = 0;


    int varbody = 0;
    int varshould = 0;
    int varpalm = 0;
    int varelbow = 0;
    int varwrist = 0;
    int vargripp = 120;


    private long lastUpdateTime;

    void Start()
    {
        Transform[] objectChildren = GetComponentsInChildren<Transform>();

        port.Open();
        System.Threading.Thread.Sleep(1000); //Delay of 1 second
        lastUpdateTime = GetCurrentTimeMillis();
        foreach (Transform child in objectChildren)
        {
            child.eulerAngles = new Vector3(0f, 0f, 0f);
        }

    }

    void Update()
    {




        //Camera.main.transform.LookAt(Parent.transform);

        Transform[] objectChildren = GetComponentsInChildren<Transform>();
        Transform body = objectChildren[1];
        Transform shoulder = objectChildren[2];
        Transform elbow = objectChildren[3];
        Transform wrist = objectChildren[4];
        Transform palm = objectChildren[5];
        Transform gripperRight = objectChildren[6];
        Transform gripperLeft = objectChildren[8];

        //gripperRight.transform.Rotate(0f, 0f, 90f);



        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            shoulder.transform.Rotate(3f, 0f, 0f);
            varshould+= 8;
            sender.SendGCode(port, $"G0 Y" + varshould);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            shoulder.transform.Rotate(-3f, 0f, 0f);
            varshould-=8;
            sender.SendGCode(port, $"G0 Y" + varshould);
        }



        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!isT0Presed)
            {
                sender.SendGCode(port, $"T0");
                isT0Presed = true;
            }
                wrist.transform.Rotate(0f, 3f, 0f);
                varwrist++;
                sender.SendGCode(port, $"G0 E" + varwrist);

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isT0Presed)
            {
                sender.SendGCode(port, $"T0");
                isT0Presed = true;
            }
            wrist.transform.Rotate(0f, -3f, 0f);
            varwrist--;
            sender.SendGCode(port, $"G0 E" + varwrist);
        }




        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            body.transform.Rotate(0f, 3f, 0f);
            varbody-=3;
            sender.SendGCode(port, $"G0 X" + varbody);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            body.transform.Rotate(0f, -3f, 0f);
            varbody+= 3;
            sender.SendGCode(port, $"G0 X"+ varbody);
        }




        if (Input.GetKeyDown(KeyCode.W))
        {
            elbow.transform.Rotate(3f, 0f, 0f);
            varelbow+=5;
            sender.SendGCode(port, $"G0 Z" + varelbow);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            elbow.transform.Rotate(-3f, 0f, 0f);
            varelbow-=5;
            sender.SendGCode(port, $"G0 Z" + varelbow);
        }

        
 

            if (Input.GetKeyDown(KeyCode.UpArrow))
          {
                if (isT0Presed)
                {
                    sender.SendGCode(port, $"T1");
                    isT0Presed = false;
                }
                palm.transform.Rotate(3f, 0f, 0f);
            varpalm++;
            sender.SendGCode(port, $"G0 E" + varpalm);
          }
          if (Input.GetKeyDown(KeyCode.DownArrow))
          {
                if (isT0Presed)
                {
                    sender.SendGCode(port, $"T1");
                    isT0Presed = false;
                }
                palm.transform.Rotate(-3f, 0f, 0f);
            varpalm--;
            sender.SendGCode(port, $"G0 E" + varpalm);
          }
        



        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gripperRight.transform.Rotate(0f, 0f, -2f);
            gripperLeft.transform.Rotate(0f, 0f, 2f);
            vargripp -= 10;
            sender.SendGCode(port, $"M280 P0 S" + vargripp);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gripperRight.transform.Rotate(0f, 0f, 2f);
            gripperLeft.transform.Rotate(0f, 0f, -2f);
            vargripp += 10;
            sender.SendGCode(port, $"M280 P0 S" + vargripp);

        }




        long currentTime = GetCurrentTimeMillis();
        long elapsedTime = currentTime - lastUpdateTime;


        Debug.Log(shoulder.name);
        if (elapsedTime >= 200) // 1 second delay
        {
            //sender.SendGCode(port, $"{shoulder.name}{Math.Floor(shoulder.rotation.x*shoulderCalibration)}");
            //sender.SendGCode(port, $"{elbow.name}{Math.Floor(elbow.rotation.x*elbowCalibration)}");

            //sender.SendGCode(port, $"M280 P0 S0");
            //sender.SendGCode(port, $"G0 X0");
            //sender.SendGCode(port, $"{wrist.name}+{wrist.rotation.y}");
            //sender.SendGCode(port, $"{palm.name}+{palm.rotation.x / calibrationIndex}");

            lastUpdateTime = currentTime;
        }
    }

    private long GetCurrentTimeMillis()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}