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
    GameObject firstCheckpoint;
    
    void Awake ()
    {
        instance = this;
    }

	void Start ()
    {
        Debug.Log("Starting Field");
        segments = new List<GameObject>();
        for (int i = 0; i < 5; ++i)
        {
            AddSegment();
        }
        firstCheckpoint = segments[0].GetComponent<FieldSegment>().GetCheckpoint();
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
        firstCheckpoint.GetComponent<Animator>().enabled = true;
        firstCheckpoint.GetComponent<Animator>().Play("Checkpoint");
        firstCheckpoint.GetComponent<AudioSource>().PlayDelayed(1f);
        activated = true;
    }

    public void Pause()
    {
        firstCheckpoint.GetComponent<AudioSource>().Pause();
    }

    public void UnPause()
    {
        if (firstCheckpoint != null) firstCheckpoint.GetComponent<AudioSource>().UnPause();
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

    public GameObject FindClosestSegment (GameObject segment, bool ignoreAdjacent)
    {
        if (segments.Count == 0) return null;
        Vector3 segmentCenter = segment.GetComponent<FieldSegment>().GetCurveCenter();
        float minDist = 9999f;
        GameObject firstSegment = segments[0];
        GameObject closestSegment = null;
        if ((!ignoreAdjacent || segment.GetComponent<FieldSegment>().IsSegmentAdjacent(firstSegment)) && !(segment == segments[0]))
        {
            minDist = (segmentCenter - segments[0].GetComponent<FieldSegment>().GetCurveCenter()).magnitude;
            closestSegment = segments[0];
        }
        for (int i = 1; i < segments.Count; ++i)
        {
            if ((ignoreAdjacent && segment.GetComponent<FieldSegment>().IsSegmentAdjacent(segments[i])) || (segment == segments[i])) continue;
            Vector3 center = segments[i].GetComponent<FieldSegment>().GetCurveCenter();
            if ((segmentCenter - center).magnitude < minDist)
            {
                minDist = (segmentCenter - center).magnitude;
                closestSegment = segments[i];
            }
        }
        return closestSegment;
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
        GameObject previousSegment = null;
        if (segments.Count > 0)
        {
            previousSegment = segments[segments.Count - 1];
            previousSegment.GetComponent<FieldSegment>().nextSegment = fieldSegment;
            fieldSegment.prevSegment = previousSegment.GetComponent<FieldSegment>();
        }
        segments.Add(segment);
        Vector3 firstBezierPoint = curve.GetControlPoint(0);
        GameObject checkpoint = Instantiate<GameObject>(CHECKPOINT);
        checkpoint.transform.parent = segment.transform;
        checkpoint.transform.position = firstBezierPoint;
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
        return TrackType.CURVE;
        if (difficulty > 1f) difficulty = 1f;
        if (difficulty < 0f) difficulty = 0f;
        float choice = Random.Range(0f, Mathf.Min(difficulty + 0.4f, 1f));
        if (choice < 0.2f)
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
