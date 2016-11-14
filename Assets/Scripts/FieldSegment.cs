using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FieldType = Field.FieldType;
using TrackType = Field.TrackType;

public class FieldSegment : MonoBehaviour
{
    public static float MAXLENGTH = 2000f;
    public static float MINLENGTH = 1000f;
    public static float MAXHEIGHT = 1000f;
    public GameObject LINEPOINT;
    public GameObject ASTEROID;

    private FieldType fieldtype;
    private TrackType tracktype;
    private Bezier curve;
    private float length;
    private float height;
    private int numpoints; //number of linepoints on curve
    private float pointdensity = 100;

    void Awake ()
    {
        curve = this.GetComponent<Bezier>();
    }

    void Start ()
    {
        
	}
	
	void Update ()
    {
	    if (numpoints < 0)
        {
            Destroy(this);
        }
	}

    public void GenerateSegment(FieldType _fieldtype, TrackType _tracktype, Vector3 lastControlPoint)
    {
        fieldtype = _fieldtype;
        tracktype = _tracktype;
        length = Random.Range(MINLENGTH, MAXLENGTH);
        height = Random.Range(0f, MAXHEIGHT);

        //Create Bezier curve
        Vector3 p0, p1 ,p2, p3;
        p0 = p1 = p2 = p3 = transform.position;
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
                //CREATE END VECTOR between 30 and 90 DEGREES OF FORWARD
                float theta = Random.Range(Mathf.PI/6f, Mathf.PI/2f);
                Vector3 endDirection = RandomlyOffset(-forward, 0.05f, 0.1f);
                Vector3 end = Vector3.RotateTowards(forward, endDirection, theta, 0f);
                end.Normalize();
                p1 = p0 + forward * length / 3;
                RandomlyOffset(p1, height);
                p2 = p0 + forward * length / 2 + end * length / 6;
                RandomlyOffset(p2, height);
                p3 = p2 + end * length / 3;
                RandomlyOffset(p3, height);
                break;
            case TrackType.SLALOM:
                break;
            case TrackType.HAIRPIN:
                //CREATE END VECTOR WITHIN 90 and 180 DEGREES OF FORWARD
                break;
            default:
                break;
        }
        p2.y /= 3;
        p3.y /= 3;
        curve.SetPoints(p0, p1, p2, p3);
        length = curve.ArcLength();
        numpoints = (int)(length / pointdensity);
        //Spawn Landmarks

        //Spawn Linepoints
        for (int i = 0; i < numpoints; ++i)
        {
            float t = (float)i / (float)numpoints;
            GameObject newLinePoint = Instantiate<GameObject>(LINEPOINT);
            newLinePoint.transform.position = curve.GetPoint(t);
        }
        //Spawn Asteroids
        for (int i = 0; i < 100; ++i)
        {
            Vector3 point = curve.GetPoint((float)i / 100f);
            point = RandomlyOffset(point, 1500f);
            GameObject asteroid = Instantiate<GameObject>(ASTEROID);
            asteroid.transform.position = point;
            asteroid.GetComponent<RandomModel>().ChooseAsteroid(fieldtype);
            asteroid.transform.localScale *= Random.Range(14f, 30f);
        }

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
