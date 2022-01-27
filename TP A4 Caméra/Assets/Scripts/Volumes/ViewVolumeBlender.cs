using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    [SerializeField]
    List<AViewVolume> ActiveViewVolume = new List<AViewVolume> ();
    Dictionary<AView, List<AViewVolume>> VolumesPerViews = new Dictionary<AView, List<AViewVolume>>();
    public static ViewVolumeBlender instance;
    Texture tex;
    private void Awake() {
        instance = this;
    }
    private void Start() {
        int Max = 0;
        for (int i = 0; i < ActiveViewVolume.Count; i++) {
            if (ActiveViewVolume[i].Priority > Max) {
                Max = ActiveViewVolume[i].Priority;
            }
        }
        for (int i = 0; i < ActiveViewVolume.Count; i++) {
            if (ActiveViewVolume[i].Priority < Max) {
                ActiveViewVolume[i].View.weight = 0;
            } else {
                ActiveViewVolume[i].View.weight = Mathf.Max(ActiveViewVolume[i].View.weight, ActiveViewVolume[i].ComputeSelfWeight());
            }
        }
    }

    private void Update() {
        int Max = 0;
        for (int i = 0; i < ActiveViewVolume.Count; i++) {
            if (ActiveViewVolume[i].Priority > Max) {
                Max = ActiveViewVolume[i].Priority;
            }
        }
        for (int i = 0; i < ActiveViewVolume.Count; i++) {
            if (ActiveViewVolume[i].Priority < Max) {
                ActiveViewVolume[i].View.weight = 0;
            } else {
                if (ActiveViewVolume[i].isCutOnSwitch) {
                    CameraController.Instance.Cut();
                }
                ActiveViewVolume[i].View.weight = Mathf.Max(ActiveViewVolume[i].View.weight, ActiveViewVolume[i].ComputeSelfWeight());
            }
        }
    }

    public void AddVolume(AViewVolume aviewVolume) {
        ActiveViewVolume.Add(aviewVolume);
        if (!VolumesPerViews.ContainsKey(aviewVolume.View)) {
            VolumesPerViews.Add(aviewVolume.View, new List<AViewVolume>());
            aviewVolume.View.SetActive(true);
        }
        VolumesPerViews[aviewVolume.View].Add(aviewVolume);
    }

    public void RemoveVolume(AViewVolume aviewVolume) {
        ActiveViewVolume.Remove(aviewVolume);
        VolumesPerViews[aviewVolume.View].Remove(aviewVolume);
        if (VolumesPerViews[aviewVolume.View].Count == 0) {
            VolumesPerViews.Remove(aviewVolume.View);
            aviewVolume.View.SetActive(false);
        }
    }

    private void OnGUI() {
        for (int i = 0; i < ActiveViewVolume.Count; i++) {
            GUILayout.Label(tex);
            GUILayout.Label(ActiveViewVolume[i].name);
        }
    }

}
