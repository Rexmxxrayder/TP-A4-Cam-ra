using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
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
    private CameraConfiguration configuration;

    public Rail rail;
    public float distanceOnRail;
    public float speed;

    [SerializeField]
    Transform target;

    private void Awake() {
        configuration = new CameraConfiguration();
    }

    private void Update() {
        distanceOnRail += Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        if (rail.isLoop) {
            if (distanceOnRail < 0 || distanceOnRail > rail.GetLength()) {
                distanceOnRail %= rail.GetLength();
                distanceOnRail += rail.GetLength();
                distanceOnRail %= rail.GetLength();
            }
        } else {
            distanceOnRail = Mathf.Clamp(distanceOnRail, 0f, rail.GetLength());
        }
        pivot = rail.GetPosition(distanceOnRail);
        transform.position = pivot;
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
