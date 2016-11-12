using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bezier : MonoBehaviour
{

    Vector3[] points;

    void Awake ()
    {
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
}
