using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AView : MonoBehaviour
{

    public float weight;
    public bool isActiveOnStart;

    abstract public CameraConfiguration GetConfiguration();

    protected void Start()
    {
        if (isActiveOnStart)
            SetActive(true);
    }

    protected void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
