using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollowView : AView
{
    [Range(0f, 360f)]
    public float yaw;

    [Range(-90f, 90f)]
    public float pitch;

    [Range(-180f, 180f)]
    public float roll;

    [Range(0f, 179f)]
    public float fov;

    [SerializeField]
    Transform target;

    private CameraConfiguration configuration = new CameraConfiguration();

    private void Update() {
        LookAt(target);
    }
    void LookAt(Transform target) {
        Vector3 direction = Vector3.zero;
        direction.x = target.position.x - transform.position.x;
        direction.y = target.position.y - transform.position.y;
        direction.z = target.position.z - transform.position.z;
        direction = direction.normalized;
        yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(pitch, yaw, roll);
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
}
