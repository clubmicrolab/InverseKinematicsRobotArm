using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Text;
using System.Threading.Tasks;
using SerialComm;

namespace ControlAuto
{
    public class ControlAuto
    {
        int bodyCalibration = 187;
        int shoulderCalibration = 311;
        int elbowCalibration = 330;
        int wristCalibration = 15;
        int palmCalibration = 60;

        double bodyAngleValue;
        double shoulderAngleValue;
        double elbowAngleValue;
        double wristAngleValue;
        double palmAngleValue;

        double lastBodyAngleValue;
        double lastShoulderAngleValue;
        double lastElbowAngleValue;
        double lastWristAngleValue;
        double lastPalmAngleValue;

        bool isT0Active = true;
        bool isT1Active = false;

        public void Execute(Transform[] objectChildren, SerialPort port, Sender sender)
      {
                Transform body = objectChildren[1];
                Transform shoulder = objectChildren[2];
                Transform elbow = objectChildren[3];
                Transform wrist = objectChildren[4];
                Transform palm = objectChildren[5];
                Transform gripperRight = objectChildren[6];
                Transform gripperLeft = objectChildren[8];

            bodyAngleValue = Math.Floor(body.localRotation.y * bodyCalibration);
            shoulderAngleValue = Math.Floor(shoulder.localRotation.x * shoulderCalibration);
            elbowAngleValue = Math.Floor(elbow.localRotation.x * elbowCalibration);
            wristAngleValue = Math.Floor(wrist.localRotation.y * wristCalibration);
            palmAngleValue = Math.Floor(palm.localRotation.x * palmCalibration);

            if (bodyAngleValue != lastBodyAngleValue)
            {
                sender.SendGCode(port, $"G0 X" + bodyAngleValue);
            }
            if(shoulderAngleValue != lastShoulderAngleValue) 
            {
                sender.SendGCode(port, $"G0 Y" + shoulderAngleValue);
            }

            if (elbowAngleValue != lastElbowAngleValue)
            {
                sender.SendGCode(port, $"G0 Z" + elbowAngleValue);
            }

            if (wristAngleValue !=  lastWristAngleValue)
            {
                if (!isT0Active)
                {
                    sender.SendGCode(port, $"T0");
                    isT0Active = true;
                    isT1Active = false;
                }
                sender.SendGCode(port, $"G0 E" +  wristAngleValue);
            }

            if (palmAngleValue != lastPalmAngleValue)
            {
                if (!isT1Active)
                {
                    sender.SendGCode(port, $"T1");
                    isT1Active = true;
                    isT0Active = false;
                }
                sender.SendGCode(port, $"G0 E" +  palmAngleValue);
            }

            lastBodyAngleValue = bodyAngleValue;
            lastShoulderAngleValue = shoulderAngleValue;
            lastElbowAngleValue = elbowAngleValue;
            lastWristAngleValue = wristAngleValue;
            lastPalmAngleValue = palmAngleValue;
        }
    }
}
