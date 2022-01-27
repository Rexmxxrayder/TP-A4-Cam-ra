using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AView : MonoBehaviour
{

    public float weight;

    abstract public CameraConfiguration GetConfiguration();

    private void OnDrawGizmos()
    {
       GetConfiguration().DrawGizmos(Color.blue);
    }

    void Start()
    {
    }

    public void SetActive(bool isActive)
    {
        if(isActive)
            CameraController.Instance.AddView(this);
        else
            CameraController.Instance.RemoveView(this);
    }
}
