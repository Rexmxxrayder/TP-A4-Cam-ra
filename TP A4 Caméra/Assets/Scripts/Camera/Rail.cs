using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour {
    public Transform[] railPoint;
    [SerializeField]
    public bool isLoop;
    float length;
    void Start() {
        length = GetLength();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnDrawGizmos() {
        DrawGizmos(Color.magenta);
    }

    public void DrawGizmos(Color color) {
        Gizmos.color = color;
        for (int i = 0; i < railPoint.Length - 1; i++) {
            Gizmos.DrawLine(railPoint[i].position, railPoint[i + 1].position);
        }
        if (isLoop) {
            Gizmos.DrawLine(railPoint[railPoint.Length - 1].position, railPoint[0].position);
        }
    }

    public float GetLength() {
        float length = 0;
        for (int i = 0; i < railPoint.Length - 1; i++) {
            length += Vector3.Distance(railPoint[i].position, railPoint[i + 1].position);
        }
        if (isLoop) {
            length += Vector3.Distance(railPoint[railPoint.Length - 1].position, railPoint[0].position);
        }
        return length;
    }

    public Vector3 GetPosition(float distance) {
        Vector3 position = Vector3.zero;

        int currentPoint = 0;
        float currentLength = Vector3.Distance(railPoint[currentPoint].position, railPoint[currentPoint + 1].position);
        if (distance < 0) {
            return railPoint[0].position;
        }
        if (distance > length) {
            if (isLoop) {
                return railPoint[0].position;
            } else {
                return railPoint[railPoint.Length - 1].position;
            }
        }
        while (distance > currentLength) {
            distance -= currentLength;
            currentPoint++;
            if (isLoop && currentPoint == railPoint.Length -1) {
                currentLength = Vector3.Distance(railPoint[currentPoint].position, railPoint[0].position);
            } else {

                  currentLength = Vector3.Distance(railPoint[currentPoint].position, railPoint[currentPoint + 1].position);
            }
        }
        float invLerp = Mathf.InverseLerp(0, currentLength, distance);
        Debug.Log(currentPoint);
        if (currentPoint == railPoint.Length - 1 && isLoop) {
            position.x = Mathf.Lerp(railPoint[currentPoint].position.x, railPoint[0].position.x, invLerp);
            position.y = Mathf.Lerp(railPoint[currentPoint].position.y, railPoint[0].position.y, invLerp);
            position.z = Mathf.Lerp(railPoint[currentPoint].position.z, railPoint[0].position.z, invLerp);
        } else {
            position.x = Mathf.Lerp(railPoint[currentPoint].position.x, railPoint[currentPoint + 1].position.x, invLerp);
            position.y = Mathf.Lerp(railPoint[currentPoint].position.y, railPoint[currentPoint + 1].position.y, invLerp);
            position.z = Mathf.Lerp(railPoint[currentPoint].position.z, railPoint[currentPoint + 1].position.z, invLerp);
        }
        return position;
    }
}
