using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AView : MonoBehaviour
{

    public float weight;
    public bool isActiveOnStart;

    abstract public CameraConfiguration GetConfiguration();

    private void OnDrawGizmos()
    {
       GetConfiguration().DrawGizmos(Color.blue);
    }

    void Start()
    {
        SetActive(isActiveOnStart);
    }

    void SetActive(bool isActive)
    {
        if(isActive)
            CameraController.Instance.AddView(this);
        else
            CameraController.Instance.RemoveView(this);
    }
}
