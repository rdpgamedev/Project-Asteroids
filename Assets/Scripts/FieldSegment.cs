using UnityEngine;
using System.Collections;
using FieldType = Field.FieldType;
using TrackType = Field.TrackType;

public class FieldSegment : MonoBehaviour
{
    private FieldType fieldtype;
    private TrackType tracktype;
    private Bezier curve;

	void Start ()
    {
        curve = this.GetComponent<Bezier>();
	}
	
	void Update ()
    {
	
	}

    void GenerateSegment(FieldType _fieldtype, TrackType _tracktype)
    {
        fieldtype = _fieldtype;
        tracktype = _tracktype;

        //Create Bezier curve

        //Spawn Landmarks

        //Spawn Linepoints

        //Spawn Asteroids

    }
}
