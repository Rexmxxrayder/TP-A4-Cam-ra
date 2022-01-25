using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{
    enum Config {
        BOTTOM = 0,
        MID = 1,
        TOP = 2
    }

    Config config;

    [Range(0f, 360f)]
    public float yaw;

    [Range(-90f, 90f)]
    public float[] pitch = new float[3];

    [Range(-180f, 180f)]
    public float[] roll = new float[3];

    [SerializeField]
    public Vector3 pivot;

    public float distance;
    Curve curve;
    Vector3 curvePosition;
    Vector3 curveSpeed;

    [Range(0f, 179f)]
    public float[] fov = new float[3];
    private CameraConfiguration configuration;

    [SerializeField]
    Transform target;

    private void Awake() {
        configuration = new CameraConfiguration();
    }

    private void Update() {
    
    }

    public override CameraConfiguration GetConfiguration() {
        configuration.Reset();

        configuration.yaw = yaw;
        configuration.pitch = pitch[(int)config];
        configuration.roll = roll[(int)config];
        configuration.fov = fov[(int)config];
        configuration.pivot = gameObject.transform.position;

        return configuration;
    }
}
