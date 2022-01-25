using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    List<AViewVolume> ActiveViewVolume = new List<AViewVolume> ();
    Dictionary<AView, List<AViewVolume>> VolumesPerViews = new Dictionary<AView, List<AViewVolume>>();
    public static ViewVolumeBlender instance;
    private void Awake() {
        instance = this;
    }

    private void Update() {
        //int Max = 0;
        //for (int i = 0; i < ActiveViewVolume.Count; i++) {
        //    if (ActiveViewVolume[i].Priority > Max) {
        //        Max = ActiveViewVolume[i].Priority;
        //    }
        //}
    }

    public void AddVolume(AViewVolume aviewVolume) {
        ActiveViewVolume.Add(aviewVolume);
        if (!VolumesPerViews.ContainsKey(aviewVolume.View)) {
            VolumesPerViews.Add(aviewVolume.View, new List<AViewVolume>());
            aviewVolume.View.gameObject.SetActive(true);
        }
        VolumesPerViews[aviewVolume.View].Add(aviewVolume);
    }

    public void RemoveVolume(AViewVolume aviewVolume) {
        ActiveViewVolume.Remove(aviewVolume);
        VolumesPerViews[aviewVolume.View].Remove(aviewVolume);
        if (VolumesPerViews[aviewVolume.View].Count == 0) {
            VolumesPerViews.Remove(aviewVolume.View);
            aviewVolume.View.gameObject.SetActive(false);
        }
    }

}
