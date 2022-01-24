using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public CameraConfiguration configuration;

    
    private static CameraController _instance;
    public static CameraController Instance { get { return _instance; } }


    private void OnDrawGizmos()
    {
        configuration.DrawGizmos(Color.red);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        ApplyConfiguration(camera, configuration);
    }

    public void ApplyConfiguration(Camera cam, CameraConfiguration config)
    {
        cam.transform.rotation = config.GetRotation();
        cam.transform.position = config.GetPosition();
        cam.fieldOfView = config.fov;
    }

}
