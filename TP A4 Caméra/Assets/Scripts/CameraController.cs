using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    private CameraConfiguration currentConfig = new CameraConfiguration();
    private CameraConfiguration targetConfig = new CameraConfiguration();

    public float speed;
    private bool isFirstUpdate = true;

    private List<AView> activeViews = new List<AView>();

    
    private static CameraController instance;
    public static CameraController Instance { get { return instance; } }


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

    }

    private void Start()
    {
        //currentConfig;
    }

    private void Update()
    {

        targetConfig.Reset();

        float weightTotal = 0;

        foreach(AView view in activeViews)
        {
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


        if (speed * Time.deltaTime < 1 && !isFirstUpdate)
        {
            currentConfig.pivot = currentConfig.pivot + (targetConfig.pivot - currentConfig.pivot) * speed * Time.deltaTime;
            currentConfig.pitch = targetConfig.pitch;
            currentConfig.roll = targetConfig.roll;
            currentConfig.fov = targetConfig.fov;
            currentConfig.distance = targetConfig.distance;
            currentConfig.yaw = targetConfig.yaw;
        }
        else
        {
            currentConfig.pivot = targetConfig.pivot;
            currentConfig.pitch = targetConfig.pitch;
            currentConfig.roll = targetConfig.roll;
            currentConfig.fov = targetConfig.fov;
            currentConfig.distance = targetConfig.distance;
            currentConfig.yaw = targetConfig.yaw;
        }

        ApplyConfiguration(camera, currentConfig);

        isFirstUpdate = false;

    }

    public void ApplyConfiguration(Camera cam, CameraConfiguration config)
    {
        cam.transform.rotation = config.GetRotation();
        cam.transform.position = config.GetPosition();
        cam.fieldOfView = config.fov;
    }

    public void AddView(AView view)
    {
        activeViews.Add(view);
    }

    public void RemoveView(AView view)
    {
        activeViews.Remove(view);
    }

    public float ComputeAverageYaw()
    {
        Vector2 sum = Vector2.zero;
        foreach (AView view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();

            sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad),
            Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }


}
