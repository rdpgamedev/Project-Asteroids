using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bezier : MonoBehaviour
{

    public static float RESOLUTION_CONSTANT = 5f;

    Vector3[] points;

    void Awake ()
    {
        points = new Vector3[4];
        for (int i = 0; i < 4; ++i)
        {
            points[i] = transform.position;
        }
    }

    void Start ()
    {
	}
	
	void Update ()
    {
	
	}

    public void SetPoints (Vector3 p0, Vector3 p1, Vector3 p2,Vector3 p3)
    {
        points[0] = p0;
        points[1] = p1;
        points[2] = p2;
        points[3] = p3;
    }

    public Vector3 GetPoint (float t)
    {
        if (t > 1) t = 1f;
        if (t < 0) t = 0f;
        return (Mathf.Pow((1f - t), 3) * points[0] +
                3 * Mathf.Pow((1f - t), 2) * t * points[1] +
                3 * (1f - t) * Mathf.Pow(t, 2) * points[2] +
                Mathf.Pow(t, 3) * points[3]);
    }

    public Vector3 GetFirstDeriv(float t)
    {
        if (t > 1) t = 1f;
        if (t < 0) t = 0f;
        return (3 * Mathf.Pow((1f - t), 2) * (points[1] - points[0]) +
            6 * (1-t) * t * (points[2] - points[1]) +
            3 * Mathf.Pow(t, 2) * (points[3] - points[2]));
    }

    public Vector3 GetSecondDeriv (float t)
    {
        if (t > 1) t = 1f;
        if (t < 0) t = 0f;
        return (6 * (1-t) * (points[2] - 2 * points[1] + points[0]) +
            6 * t * (points[3] - 2 * points[2] + points[1]));
    }

    public Vector3 GetNormal (float t)
    {
        if (t >= 1) t = 1f - 0.01f;
        if (t < 0) t = 0f;
        Vector3 tangent1 = GetFirstDeriv(t).normalized;
        Vector3 tangent2 = GetFirstDeriv(t + 0.01f).normalized;
        Vector3 rotationAxis = Vector3.Cross(tangent2, tangent1);
        return (Quaternion.AngleAxis(90f, rotationAxis) * tangent1);
    }

    //find arclength from t1 to t2
    public float ArcLength (float t1, float t2)
    {
        float polygonlength = ((points[1] - points[0]) + 
                               (points[2] - points[1]) + 
                               (points[3] - points[2])).magnitude;
        float linelength = (points[3] - points[0]).magnitude;
        float err = (polygonlength - linelength) / linelength;
        int resolution = (int)(err * RESOLUTION_CONSTANT);
        t1 = (t1 < 0 ? 0 : t1);
        t1 = (t1 > 1 ? 1 : t1);
        t2 = (t2 < 0 ? 0 : t2);
        t2 = (t2 > 1 ? 1 : t2);
        if (t1 > t2)
        {
            float tmp = t1;
            t1 = t2;
            t2 = tmp;
        }
        float lengthsum = 0f;
        float tlength = t2 - t1;
        float step = tlength / resolution;
        for (int i = 1; i < resolution; ++i)
        {
            lengthsum += (GetPoint(t1 + step * i) - 
                          GetPoint(t1 + step * (i - 1))).magnitude;
        }
        lengthsum += (GetPoint(t2) - 
                      GetPoint(t2-(tlength/resolution))).magnitude;
        return lengthsum;
    }

    //find arclength from t=0 to t1
    public float ArcLength (float t1)
    {
        return ArcLength(0f, t1);
    }

    //find arclength from t=0 to t=1
    public float ArcLength ()
    {
        return ArcLength(0f, 1f);
    }

    public Vector3 GetControlPoint(int point)
    {
        point = (point < 0 ? 0 : point);
        point = (point > 3 ? 3 : point);
        return points[point];
    }

    //finds approximate closest distance to this bezier curve through polling iterations number of points on curve
    //iterations minimum is 2, default is 5
    public float ClosestDistToCurve(Vector3 pos, int iterations = 5)
    {
        if (iterations < 2) iterations = 2;
        float[] distances = new float[iterations];
        for (int i = 0; i <= iterations - 1; ++i)
        {
            distances[i] = (pos - GetPoint((float)i / (float)(iterations - 1))).magnitude;
        }
        return Mathf.Min(distances);
    }
}
