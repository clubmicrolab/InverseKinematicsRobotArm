using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float damping = 2.0f;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - (target.position + Vector3.up * height);
    }

    void LateUpdate()
    {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, damping * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.position + rotation * offset;

        transform.LookAt(target.position + Vector3.up * height);
    }
}
