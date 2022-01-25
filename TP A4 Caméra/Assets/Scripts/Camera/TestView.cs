using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CameraController.Instance.AddView(gameObject.GetComponent<FixedView>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
