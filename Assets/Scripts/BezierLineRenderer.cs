using UnityEngine;
using System.Collections;

public class BezierLineRenderer : MonoBehaviour {
    public float time = 5f;
    public int resolution = 100;
    
    private LineRenderer lineRenderer;
    private Bezier curve;
    private bool activated = false;
    private float timer;
    private int numPoints = 1;

	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        curve = GetComponent<Bezier>();
	}
	
	void Update () {
	    if (activated)
        {
            timer += Time.deltaTime;
            float endAlpha = 0f;
            if (timer > time)
            {
                activated = false;
                timer = 5f;
                endAlpha = 0.5f;
            }
            GradientAlphaKey start = new GradientAlphaKey(0.5f, 0f);
            GradientAlphaKey mid = new GradientAlphaKey(0.5f, timer / time);
            GradientAlphaKey midEpsilon = new GradientAlphaKey(endAlpha, timer / time + 0.001f);
            GradientAlphaKey end = new GradientAlphaKey(endAlpha, 1f);
            Gradient lineGradient = new Gradient();
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { start, mid, midEpsilon, end };
            GradientColorKey[] colorKeys = lineRenderer.colorGradient.colorKeys;
            lineGradient.SetKeys(colorKeys, alphaKeys);
            lineRenderer.colorGradient = lineGradient;
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
