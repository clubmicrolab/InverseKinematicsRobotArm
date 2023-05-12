using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Text;
using System.Threading.Tasks;
using SerialComm;
using UnityEditor.Experimental.GraphView;

namespace ControlManual
{
    public class ControlManual
    {
        double varbody = 0;
        double varshould = 0;
        double varpalm = 0;
        double varelbow = 0;
        double varwrist = 0;
        double vargripp = 120;
        double speed = 100;

        bool isT0Presed = true;

        public void Execute(Transform[] objectChildren, SerialPort port, Sender sender)
        {
            Transform body = objectChildren[1];
            Transform shoulder = objectChildren[2];
            Transform elbow = objectChildren[3];
            Transform wrist = objectChildren[4];
            Transform palm = objectChildren[5];
            Transform gripperRight = objectChildren[6];
            Transform gripperLeft = objectChildren[8];

            //shoulder
            if (Input.GetKey(KeyCode.Keypad8))
            {
                shoulder.transform.Rotate(0.175f, 0f, 0f);
                varshould += 0.35;
                sender.SendGCode(port, $"G0 Y" + varshould);
            }   
            if (Input.GetKey(KeyCode.Keypad2))
            {
                shoulder.transform.Rotate(-0.175f, 0f, 0f);
                varshould -= 0.35;
                sender.SendGCode(port, $"G0 Y" + varshould);
            }

            //wrist
            if (Input.GetKey(KeyCode.A))
            {
                if (!isT0Presed)
                {
                    sender.SendGCode(port, $"T0");
                    isT0Presed = true;
                }
                wrist.transform.Rotate(0f, 0.3f, 0f);
                varwrist += 0.3;
                sender.SendGCode(port, $"G0 E" + varwrist);
            }        
            if (Input.GetKey(KeyCode.D))
            {
                if (!isT0Presed)
                {
                    sender.SendGCode(port, $"T0");
                    isT0Presed = true;
                }
                wrist.transform.Rotate(0f, -0.3f, 0f);
                varwrist -= 0.3;
                sender.SendGCode(port, $"G0 E" + varwrist);
            }

            //body 
            if (Input.GetKey(KeyCode.Keypad4))
            {
                body.transform.Rotate(0f, 0.35f, 0f);
                varbody -= 0.5;
                sender.SendGCode(port, $"G0 X" + varbody);
            }   
            if (Input.GetKey(KeyCode.Keypad6))
            {
                body.transform.Rotate(0f, -0.35f, 0f);
                varbody += 0.5;
                sender.SendGCode(port, $"G0 X" + varbody);
            }

            //elbow
            if (Input.GetKey(KeyCode.W))
            {
                elbow.transform.Rotate(0.2f, 0f, 0f);
                varelbow += 0.5;
                sender.SendGCode(port, $"G0 Z" + varelbow);
            }         
            if (Input.GetKey(KeyCode.S))
            {
                elbow.transform.Rotate(-0.2f, 0f, 0f);
                varelbow -= 0.5;
                sender.SendGCode(port, $"G0 Z" + varelbow);
            }

            //palm
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (isT0Presed)
                {
                    sender.SendGCode(port, $"T1");
                    isT0Presed = false;
                }
                palm.transform.Rotate(1.5f, 0f, 0f);
                varpalm += 0.65;
                sender.SendGCode(port, $"G0 E" + varpalm);
            }    
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (isT0Presed)
                {
                    sender.SendGCode(port, $"T1");
                    isT0Presed = false;
                }
                palm.transform.Rotate(-1.5f, 0f, 0f);
                varpalm -= 0.65;
                sender.SendGCode(port, $"G0 E" + varpalm);
            }

            //servo
            if (Input.GetKey(KeyCode.RightArrow))
            {
                vargripp -= 3;
                if (vargripp < 0)
                {
                    vargripp = 0;
                }
                else
                {
                    gripperRight.transform.Rotate(0f, 0f, -0.6f);
                    gripperLeft.transform.Rotate(0f, 0f, 0.6f);
                    sender.SendGCode(port, $"M280 P0 S" + vargripp);
                }
            }  
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                vargripp += 3;
                if (vargripp > 120)
                {
                    vargripp = 120;
                }
                else
                {
                    gripperRight.transform.Rotate(0f, 0f, 0.6f);
                    gripperLeft.transform.Rotate(0f, 0f, -0.6f);
                    sender.SendGCode(port, $"M280 P0 S" + vargripp);
                }

            }

            //Speed control
            if (Input.GetKey(KeyCode.Q))
            {
                speed -= 10;
                sender.SendGCode(port, $"M220 S" + speed);
            } 
            if (Input.GetKey(KeyCode.E))
            {
                speed += 10;
                sender.SendGCode(port, $"M220 S" + speed);

            }
        }
    }
}
