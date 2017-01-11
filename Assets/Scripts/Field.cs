using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    public static Field instance;
    public GameObject FIELDSEGMENT;
    public GameObject CHECKPOINT;
    public int MINASTEROIDS = 25;

    public int asteroidCount = 0;
    public enum FieldType { ICE, ROCK };
    public enum TrackType { STRAIGHT, CURVE, SLALOM, HAIRPIN };
    public int checkpointsMade = 0;
    public bool activated;


    List<GameObject> segments;
    List<GameObject> checkpoints;
    
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
        GameObject firstCheckpoint = segments[0].GetComponent<FieldSegment>().GetCheckpoint();
        firstCheckpoint.transform.forward = transform.forward;
    }
	
	void Update ()
    {
        GameObject lastsegment = segments[segments.Count - 1];
        Vector3 lastSegPos = lastsegment.transform.position;
        Vector3 shipPos = PlayerShip.instance.transform.position;
        while ((lastSegPos - shipPos).magnitude < FieldSegment.MAXLENGTH * 3)
        {
            AddSegment();
            lastsegment = segments[segments.Count - 1];
            lastSegPos = lastsegment.transform.position;
        }
        if (GameManager.instance.isPlaying && !activated) Activate();
	}

    public void Activate ()
    {
        GameObject firstCheckpoint = segments[0].GetComponent<FieldSegment>().GetCheckpoint();
        firstCheckpoint.GetComponent<Animator>().enabled = true;
        firstCheckpoint.GetComponent<Animator>().Play("Checkpoint");
        firstCheckpoint.GetComponent<AudioSource>().PlayDelayed(1f);
        activated = true;
        Time.timeScale = 1f;
    }

    public GameObject LastSegment()
    {
        if (segments.Count < 1) return null;
        return segments[segments.Count - 1];
    }

    public List<Vector3> SegmentMidpoints()
    {
        List<Vector3> midpoints = new List<Vector3>();
        foreach (GameObject segment in segments)
        {
            midpoints.Add(segment.GetComponent<FieldSegment>().GetCurveCenter());
        }
        return midpoints;
    }

    public void RemoveSegment (GameObject segment)
    {
        segments.Remove(segment);
    }

    void AddSegment ()
    {
        GameObject segment = Instantiate<GameObject>(FIELDSEGMENT);
        segment.transform.parent = transform;
        FieldSegment fieldSegment = segment.GetComponent<FieldSegment>();
        Bezier curve = segment.GetComponent<Bezier>();
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
        fieldSegment.GenerateSegment((FieldType)Random.Range(0, 2), 
                                      RandomTrackType(GameManager.instance.difficulty), 
                                      lastControlPoint);
        segments.Add(segment);
        Vector3 lastBezierPoint = curve.GetControlPoint(0);
        GameObject checkpoint = Instantiate<GameObject>(CHECKPOINT);
        checkpoint.transform.parent = segment.transform;
        checkpoint.transform.position = lastBezierPoint;
        Vector3 forward = curve.GetFirstDeriv(0f);
        Vector3 up = -curve.GetNormal(0f);
        checkpoint.transform.rotation = Quaternion.LookRotation(forward, up);
        fieldSegment.SetCheckpoint(checkpoint);
        if (segments.Count > 1)
        {
            GameObject prevSegment = segments[segments.Count - 2];
            prevSegment.GetComponent<FieldSegment>().SetNextCheckpoint(checkpoint);
        }
        ++checkpointsMade;
    }

    TrackType RandomTrackType(float difficulty)
    {
        if (difficulty > 1f) difficulty = 1f;
        if (difficulty < 0f) difficulty = 0f;
        float choice = Random.Range(0f, Mathf.Min(difficulty + 0.4f, 1f));
        if (choice < 0.3f)
        {
            return TrackType.STRAIGHT;
        }
        else if (choice < 0.6f)
        {
            return TrackType.CURVE;
        }
        else if (choice < 0.9f)
        {
            return TrackType.SLALOM;
        }
        else
        {
            return TrackType.HAIRPIN;
        }
    }

}
