using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour {
    public int Priority;
    public AView View;
    protected bool isActive { get; private set; }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public virtual float ComputeSelfWeight() {
        return 1.0f;
    }

    protected void SetActive(bool isActive) {
        if (isActive) {
            ViewVolumeBlender.instance.AddVolume(this);
        } else {
            ViewVolumeBlender.instance.RemoveVolume(this);
        }
        this.isActive = isActive;
    }
}
