using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume { 
    public Transform target;
    public float outerRadius;
    public float innerRadius;
    float distance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance= Vector3.Distance(transform.position, target.position);
        if (distance <= outerRadius && !isActive) {
            SetActive(true);
        }

        if (distance >= outerRadius && isActive) {
            SetActive(false);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, outerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, innerRadius);
    }

    public override float ComputeSelfWeight() {
        return Mathf.InverseLerp(innerRadius, outerRadius, distance);
    }
}
