using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    public static Field instance;
    public GameObject FIELDSEGMENT;

    public enum FieldType { ICE, ROCK };
    public enum TrackType { STRAIGHT, CURVE, SLALOM, HAIRPIN };
    List<GameObject> segments;

    void Awake ()
    {
        instance = this;
    }

	void Start ()
    {
        segments = new List<GameObject>();
        for (int i = 0; i < 5; ++i)
        {
            AddSegment();
        }
	}
	
	void Update ()
    {
        if ((segments[segments.Count - 1].transform.position - PlayerShip.instance.transform.position).magnitude < FieldSegment.MAXLENGTH * 3)
        {
            AddSegment();
        }
	}

    void AddSegment ()
    {
        GameObject segment = Instantiate<GameObject>(FIELDSEGMENT);
        FieldSegment fieldSegment = segment.GetComponent<FieldSegment>();
        Vector3 lastControlPoint;
        if (segments.Count > 0)
        {
            Bezier lastSegmentCurve = segments[segments.Count-1].GetComponent<Bezier>();
            lastControlPoint = lastSegmentCurve.GetControlPoint(2);
            segment.transform.position = lastSegmentCurve.GetControlPoint(3);
        }
        else
        {
            lastControlPoint = -Vector3.forward;
            segment.transform.position = transform.position;
        }
        fieldSegment.GenerateSegment((FieldType)Random.Range(0, 2), (TrackType)Random.Range(0, 2), lastControlPoint);
        segments.Add(segment);
    }

}
