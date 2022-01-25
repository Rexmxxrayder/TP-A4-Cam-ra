using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour {
    public Vector3 a, b, c, d;
    public float gizmoPoint = 100;
    public Vector3[] hizmoPoint;

    Vector3 GetPosition(float t) {
        Vector3 position = MathsUtils.CubicBezier(a, b, c, d, t);
        return position;
    }

    Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix) {
        Vector3 position = GetPosition(t);
        position = localToWorldMatrix.MultiplyPoint(position);
        return position;
    }


    private void OnDrawGizmos() {
        DrawGizmos(Color.blue, gameObject.transform.localToWorldMatrix);
    }

    public void DrawGizmos(Color color, Matrix4x4 localToWorldMatrix) {
        Gizmos.color = color;
        for (float i = 0; i < gizmoPoint - 1; i++) {
            Gizmos.DrawLine(GetPosition(1 / gizmoPoint * i, localToWorldMatrix), GetPosition(1 / gizmoPoint * (i + 1), localToWorldMatrix));
            Debug.Log(1f / gizmoPoint * i);
        }
    }
}
