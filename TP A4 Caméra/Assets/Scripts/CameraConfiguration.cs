using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class CameraConfiguration : MonoBehaviour
{
    
    [Range(0f, 360f)]
    public float yaw;

    [Range(-90f, 90f)]
    public float pitch;

    [Range(-180f, 180f)]
    public float roll;

    [SerializeField]
    public Vector3 pivot;

    public float distance;

    [Range(0f, 179f)]
    public float fov;


    public void Update()
    {
        if (distance < 0)
            distance = 0;
    }

    public Quaternion GetRotation()
    {
        Quaternion orientation = Quaternion.Euler(pitch, yaw, roll);

        return orientation;
    }

    public Vector3 GetPosition()
    {
        Quaternion orientation = GetRotation();
        Vector3 offset = orientation * (Vector3.back * distance);

        return pivot + offset;
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pivot, 0.25f);
        Vector3 position = GetPosition();
        Gizmos.DrawLine(pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
