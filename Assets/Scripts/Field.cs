using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    public static Field instance;
    public enum FieldType { ROCK, ICE };
    public enum TrackType { STRAIGHT, CURVE, SLALOM, HAIRPIN };
    List<FieldSegment> segments;

    void Awake ()
    {
        instance = this;
    }

	void Start ()
    {
	    
	}
	
	void Update ()
    {
	
	}

    void AddSegment (Vector3 position)
    {

    }

}
