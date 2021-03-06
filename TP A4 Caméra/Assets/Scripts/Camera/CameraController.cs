using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Camera cameraa;
    private CameraConfiguration currentConfig = new CameraConfiguration();
    private CameraConfiguration targetConfig = new CameraConfiguration();

    public float speed;

    [SerializeField]
    private List<AView> activeViews = new List<AView>();


    private static CameraController instance;
    public static CameraController Instance { get { return instance; } }

    [SerializeField]
    Transform centralTarget;
    public bool isCentralTarget;
    public Vector2 targetYawPitch;
    public Vector2 YawMaxOffset;
    public Vector2 PitchMaxOffset;

    bool isCutRequested = true;


    private void Awake() {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

    }

    private void Start() {
        if (centralTarget != null) {
            Vector3 direction = Vector3.zero;
            direction.x = centralTarget.position.x - transform.position.x;
            direction.y = centralTarget.position.y - transform.position.y;
            direction.z = centralTarget.position.z - transform.position.z;
            direction = direction.normalized;
            targetYawPitch.x = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            targetYawPitch.y = -Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        }
    }

    private void Update() {
        Debug.Log("aaaaaaa");
        for (int i = 0; i < activeViews.Count; i++) {
            Debug.Log(activeViews[i].name);
        }
        targetConfig.Reset();

        float weightTotal = 0;

        foreach (AView view in activeViews) {
            CameraConfiguration config = view.GetConfiguration();

            targetConfig.pitch += config.pitch * view.weight;
            targetConfig.roll += config.roll * view.weight;
            targetConfig.fov += config.fov * view.weight;
            targetConfig.pivot += config.pivot * view.weight;
            targetConfig.distance += config.distance * view.weight;

            weightTotal += view.weight;

        }

        targetConfig.pitch /= weightTotal;
        targetConfig.roll /= weightTotal;
        targetConfig.fov /= weightTotal;
        targetConfig.pivot /= weightTotal;
        targetConfig.distance /= weightTotal;
        targetConfig.yaw = ComputeAverageYaw();


        if (speed * Time.deltaTime < 1 && !isCutRequested) {
            currentConfig.pivot = currentConfig.pivot + (targetConfig.pivot - currentConfig.pivot) * speed * Time.deltaTime;
            currentConfig.pitch = targetConfig.pitch + (targetConfig.pitch - currentConfig.pitch) * speed * Time.deltaTime;
            currentConfig.roll = targetConfig.roll + (targetConfig.roll - currentConfig.roll) * speed * Time.deltaTime;
            currentConfig.fov = targetConfig.fov + (targetConfig.fov - currentConfig.fov) * speed * Time.deltaTime;
            currentConfig.distance = targetConfig.distance + (targetConfig.distance - currentConfig.distance) * speed * Time.deltaTime;


            Vector2 vYawTarget = new Vector2(Mathf.Cos(targetConfig.yaw * Mathf.Deg2Rad),
                Mathf.Sin(targetConfig.yaw * Mathf.Deg2Rad));

            Vector2 vYawCurrent = new Vector2(Mathf.Cos(currentConfig.yaw * Mathf.Deg2Rad),
                Mathf.Sin(currentConfig.yaw * Mathf.Deg2Rad));

            Vector2 vYawLerp = Vector2.Lerp(vYawCurrent, vYawTarget, speed * Time.deltaTime);

            currentConfig.yaw = Vector2.SignedAngle(Vector2.right, vYawLerp);
        } else {
            currentConfig.pivot = targetConfig.pivot;
            currentConfig.pitch = targetConfig.pitch;
            currentConfig.roll = targetConfig.roll;
            currentConfig.fov = targetConfig.fov;
            currentConfig.distance = targetConfig.distance;
            currentConfig.yaw = targetConfig.yaw;
            isCutRequested = false;
        }


        #region FixedFollowView
        if (isCentralTarget) {
            if (currentConfig.yaw > targetYawPitch.x + YawMaxOffset.x) {
                currentConfig.yaw = YawMaxOffset.x;
            }

            if (currentConfig.yaw < targetYawPitch.x + YawMaxOffset.y) {
                currentConfig.yaw = YawMaxOffset.y;
            }

            if (currentConfig.pitch > targetYawPitch.y + PitchMaxOffset.x) {
                currentConfig.pitch = PitchMaxOffset.x;
            }

            if (currentConfig.pitch < targetYawPitch.y + PitchMaxOffset.y) {
                currentConfig.pitch = PitchMaxOffset.y;
            }
        }

        #endregion
        ApplyConfiguration(cameraa, currentConfig);

    }

    public void ApplyConfiguration(Camera cam, CameraConfiguration config) {
        cam.transform.rotation = config.GetRotation();
        cam.transform.position = config.GetPosition();
        cam.fieldOfView = config.fov;
    }

    public void AddView(AView view) {
        activeViews.Add(view);
    }

    public void Cut() {
        isCutRequested = true;
    }
    public void RemoveView(AView view) {
        activeViews.Remove(view);
    }

    public float ComputeAverageYaw() {
        Vector2 sum = Vector2.zero;
        foreach (AView view in activeViews) {
            CameraConfiguration config = view.GetConfiguration();

            sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad),
            Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
        //float sum = 0;
        //float total = 0;
        //foreach (AView view in activeViews) {
        //    CameraConfiguration config = view.GetConfiguration();
        //    float yaw = config.yaw;
        //    yaw %= 360;
        //    yaw += 360;
        //    yaw %= 360;
        //    yaw = yaw - 180;
        //    sum += yaw * view.weight;
        //    total++;
        //}
        //return sum / total + 180;

    }

    private void OnDrawGizmos() {
        if (currentConfig != null) {
            currentConfig.DrawGizmos(Color.red);
        }
    }
}
