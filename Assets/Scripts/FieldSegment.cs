using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FieldType = Field.FieldType;
using TrackType = Field.TrackType;

public class FieldSegment : MonoBehaviour
{
    public static float MAXLENGTH = 2000f;
    public static float MINLENGTH = 1000f;
    public static float MAXHEIGHT = 200f;

    private FieldType fieldtype;
    private TrackType tracktype;
    private Bezier curve;
    private float length;
    private float height;

	void Start ()
    {
        curve = this.GetComponent<Bezier>();
	}
	
	void Update ()
    {
	
	}

    void GenerateSegment(FieldType _fieldtype, TrackType _tracktype, Vector3 lastControlPoint)
    {
        fieldtype = _fieldtype;
        tracktype = _tracktype;
        length = Random.Range(MINLENGTH, MAXLENGTH);
        height = Random.Range(0f, MAXHEIGHT);

        //Create Bezier curve
        Vector3 p0, p1 ,p2, p3;
        p0 = transform.position;
        Vector3 forward = p0 - lastControlPoint;
        forward.Normalize();
        //setup bezier points
        switch (tracktype)
        {
            case TrackType.STRAIGHT:
                p1 = p0 + forward * length / 3;
                RandomlyOffset(p1, height);
                p2 = p0 + forward * length * 2 / 3;
                RandomlyOffset(p2, height);
                p3 = p0 + forward * length;
                RandomlyOffset(p3, height);
                break;
            case TrackType.CURVE:
                //CREATE END VECTOR WITHIN 90 DEGREES OF FORWARD
                break;
            case TrackType.SLALOM:
                break;
            case TrackType.HAIRPIN:
                //CREATE END VECTOR WITHIN 90 and 180 DEGREES OF FORWARD
                break;
            default:
                break;
        }
        //Spawn Landmarks

        //Spawn Linepoints

        //Spawn Asteroids

    }

    Vector3 RandomlyOffset(Vector3 point, float delta)
    {
        Vector3 newpoint = point;
        Vector3 offset = new Vector3(Random.Range(-delta, delta),
                                     Random.Range(-delta, delta),
                                     Random.Range(-delta, delta));
        newpoint += offset;
        return newpoint;
    }

    //offsets a Vector3 with two different max deltas, one for
    //the horizontal plane and one in the vertical axis.
    Vector3 RandomlyOffset(Vector3 point, float deltaH, float deltaX)
    {
        Vector3 newpoint = point;
        Vector3 offset = new Vector3(Random.Range(-deltaX, deltaX),
                                     Random.Range(-deltaH, deltaH),
                                     Random.Range(-deltaX, deltaX));
        newpoint += offset;
        return newpoint;
    }
}
