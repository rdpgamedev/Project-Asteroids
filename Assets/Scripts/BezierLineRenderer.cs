using UnityEngine;
using System.Collections;

public class BezierLineRenderer : MonoBehaviour {
    public float speed = 5f;
    public int resolution = 100;
    
    private LineRenderer lineRenderer;
    private Bezier curve;
    private bool activated = false;
    private float step;
    private float timer;
    private int numPoints = 1;

	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        curve = GetComponent<Bezier>();
        step = 1f / speed;
	}
	
	void Update () {
	    if (activated)
        {
            timer += Time.deltaTime;
            if (timer > step)
            {
                timer -= step;
                ++numPoints;
                if (numPoints > resolution) activated = false;
            }
        }
	}

    public void Activate ()
    {
        activated = true;
        Vector3[] points = new Vector3[resolution + 1];
        for (int i = 0; i <= resolution; ++i)
        {
            points[i] = curve.GetPoint((float)i / (float)resolution);
        }
        lineRenderer.numPositions = resolution + 1;
        lineRenderer.SetPositions(points);
    }
}
