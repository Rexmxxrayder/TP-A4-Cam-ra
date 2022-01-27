using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{


    [Range(-180f, 180f)]
    public float yaw;
    public float yawSpeed;

    [Range(-90f, 90f)]
    public float[] pitchs = new float[3];
    public float pitch;

    [Range(-180f, 180f)]
    public float[] rolls = new float[3];
    public float roll;

    [SerializeField]
    public Vector3 pivot;

    public float distance;
    public Curve curve;
    public float curveT;
    public float curveSpeed;

    [Range(0f, 179f)]
    public float[] fovs = new float[3];
    public float fov;
    private CameraConfiguration configuration = new CameraConfiguration();

    [SerializeField]
    Transform target;

    private void Awake() {
    }

    private void Update() {
        curveT += Input.GetAxisRaw("Vertical") * curveSpeed * Time.deltaTime;
        curveT = Mathf.Clamp(curveT, 0f, 1f);
        yaw += Input.GetAxisRaw("Horizontal") * yawSpeed * Time.deltaTime;
        yaw = Mathf.Clamp(yaw, -180f,180f);

        float adaptCurveT = curveT;
        if (adaptCurveT > 0.5f) {
            adaptCurveT -= 0.5f;
            adaptCurveT *= 2;
            pitch = Mathf.Lerp(pitchs[1], pitchs[2], adaptCurveT);
            fov = Mathf.Lerp(fovs[1], fovs[2], adaptCurveT);
            roll = Mathf.Lerp(rolls[1], rolls[2], adaptCurveT);
        } else {
            adaptCurveT *= 2;
            pitch = Mathf.Lerp(pitchs[0], pitchs[1], adaptCurveT);
            fov = Mathf.Lerp(fovs[0], fovs[1], adaptCurveT);
            roll = Mathf.Lerp(rolls[0], rolls[1], adaptCurveT);
        }
        pivot = curve.GetPosition(curveT, ComputeCurveToWorldMatrix());
        transform.position = pivot;
    }

    public override CameraConfiguration GetConfiguration() {
        configuration.Reset();

        configuration.yaw = yaw;
        configuration.pitch = pitch;
        configuration.roll = roll;
        configuration.fov = fov;
        configuration.pivot = gameObject.transform.position;

        return configuration;
    }

    public Matrix4x4 ComputeCurveToWorldMatrix() {
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);
        return Matrix4x4.TRS(curve.transform.position, rotation, Vector3.one);
    }
}
