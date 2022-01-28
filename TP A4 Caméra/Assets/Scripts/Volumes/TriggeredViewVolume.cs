using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredViewVolume : AViewVolume {
    public GameObject target;
    BoxCollider bc;
    // Start is called before the first frame update
    void Start() {
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == target) {
            SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject == target) {
            SetActive(false);
        }
    }
}
