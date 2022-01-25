using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    private CameraConfiguration configurationM;

    public FixedView currentConfig;
    public FixedView targetConfig;

    public float speed;

    private List<AView> activeViews = new List<AView>();

    
    private static CameraController instance;
    public static CameraController Instance { get { return instance; } }


    //private void OnDrawGizmos()
    //{
    //    configuration.DrawGizmos(Color.red);
    //}

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


        if (speed * Time.deltaTime < 1)
            camera.transform.position = camera.transform.position + (targetConfig.transform.position - camera.transform.position) * speed * Time.deltaTime;
        else
            camera.transform.position = camera.transform.position;

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
