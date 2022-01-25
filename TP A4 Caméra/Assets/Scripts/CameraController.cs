using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    private CameraConfiguration configurationM;

    private List<AView> activeViews;

    
    private static CameraController instance;
    public static CameraController Instance { get { return instance; } }


    private void OnDrawGizmos()
    {
        //configuration.DrawGizmos(Color.red);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        configurationM = new CameraConfiguration();
    }

    private void Update()
    {
        //ApplyConfiguration(camera, configuration);

        configurationM.Reset();

        float weightTotal = 0;

        foreach(AView view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();

            configurationM.pitch += config.pitch * view.weight;
            configurationM.roll += config.roll * view.weight;
            configurationM.fov += config.fov * view.weight;
            configurationM.pivot += config.pivot * view.weight;
            configurationM.distance += config.distance * view.weight;

            weightTotal += view.weight;

        }

        configurationM.pitch /= weightTotal;
        configurationM.roll /= weightTotal;
        configurationM.fov /= weightTotal;
        configurationM.pivot /= weightTotal;
        configurationM.distance /= weightTotal;
        configurationM.yaw = ComputeAverageYaw();

        ApplyConfiguration(camera, configurationM);

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
