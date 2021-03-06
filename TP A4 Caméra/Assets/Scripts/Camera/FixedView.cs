using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedView : AView
{
    [Range(0f, 360f)]
    public float yaw;

    [Range(-90f, 90f)]
    public float pitch;

    [Range(-180f, 180f)]
    public float roll;

    [Range(0f, 179f)]
    public float fov;

    private CameraConfiguration configuration = new CameraConfiguration();

    private void Awake()
    {
    }

    public override CameraConfiguration GetConfiguration()
    {
        configuration.Reset();

        configuration.yaw = yaw;
        configuration.pitch = pitch;
        configuration.roll = roll;
        configuration.fov = fov;
        configuration.pivot = gameObject.transform.position;

        return configuration;
    }

}
