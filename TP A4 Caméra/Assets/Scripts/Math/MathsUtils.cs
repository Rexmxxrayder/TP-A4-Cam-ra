using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 public static class MathsUtils {
    public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 target) {

        Vector3 ab = b - a;
        Vector3 ac = target - a;

        float dot = Mathf.Clamp(Vector3.Dot(ac, ab.normalized), 0, ab.magnitude);

        Vector3 projC = a + ab.normalized * dot;

        Debug.Log(projC);

        return projC;
    }

    public static Vector3 LinearBezier(Vector3 a, Vector3 b, float t) {
        Vector3 position;
        position.x = Mathf.Lerp(a.x, b.x, t);
        position.y = Mathf.Lerp(a.y, b.y, t);
        position.z = Mathf.Lerp(a.z, b.z, t);
        return position;
    }

    public static Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t) {
        Vector3 d = LinearBezier(a, b, t);
        Vector3 e = LinearBezier(b, c, t);
        Vector3 f = LinearBezier(d, e, t);
        return f;
    }

    public static Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) {
        Vector3 e = QuadraticBezier(a, b, c, t);
        Vector3 f = QuadraticBezier(b, c, d, t);
        Vector3 g = LinearBezier(e, f, t);
        return g;
    }
}
