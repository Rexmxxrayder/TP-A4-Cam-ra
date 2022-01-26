using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
    
    private float yaw;
    private float pitch;

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

    public bool isAuto;

    private void Awake() {
        configuration = new CameraConfiguration();
    }

    private void Update() {
        
        
        if (isAuto)
        {
            float distance = Mathf.Infinity;
            Vector3 position = Vector3.zero;

            for (int i = 0; i < rail.railPoint.Length - 1; i++)
            {
                Vector3 positionNearestPoint = MathsUtils.GetNearestPointOnSegment(rail.railPoint[i].position, rail.railPoint[i + 1].position, target.position);
                float newDistance = Vector3.Distance(positionNearestPoint, target.position);

                if (newDistance < distance)
                {
                    distance = newDistance;
                    position = positionNearestPoint;
                }
            }

            if (rail.isLoop)
            {
                Vector3 positionNearestPoint2 = MathsUtils.GetNearestPointOnSegment(rail.railPoint[rail.railPoint.Length - 1].position, rail.railPoint[0].position, target.position);
                float newDistance2 = Vector3.Distance(positionNearestPoint2, target.position);

                if (newDistance2 < distance)
                {
                    distance = newDistance2;
                    position = positionNearestPoint2;
                }
            }

            gameObject.transform.position = position;
        }
        else
        {
            distanceOnRail += Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
            if (rail.isLoop)
            {
                if (distanceOnRail < 0 || distanceOnRail > rail.GetLength())
                {
                    distanceOnRail %= rail.GetLength();
                    distanceOnRail += rail.GetLength();
                    distanceOnRail %= rail.GetLength();
                }
            }
            else
            {
                distanceOnRail = Mathf.Clamp(distanceOnRail, 0f, rail.GetLength());
            }
            pivot = rail.GetPosition(distanceOnRail);
            transform.position = pivot;
        }

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
