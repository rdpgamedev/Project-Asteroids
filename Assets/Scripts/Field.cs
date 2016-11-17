using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    public static Field instance;
    public GameObject FIELDSEGMENT;
    public int MAXASTEROIDS = 1500;

    public int asteroidCount = 0;
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
        fieldSegment.GenerateSegment((FieldType)Random.Range(0, 2), RandomTrackType(1f), lastControlPoint);
        segments.Add(segment);
    }

    TrackType RandomTrackType(float difficulty)
    {
        if (difficulty > 1f) difficulty = 1f;
        if (difficulty < 0f) difficulty = 0f;
        float choice = Random.Range(0f, 1f) * difficulty + 0.5f;
        if (choice < 0.25f)
        {
            return TrackType.STRAIGHT;
        }
        else if (choice < 0.5f)
        {
            return TrackType.CURVE;
        }
        else if (choice < 0.75f)
        {
            return TrackType.SLALOM;
        }
        else
        {
            return TrackType.HAIRPIN;
        }
    }

}
