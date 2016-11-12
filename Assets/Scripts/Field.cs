using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    public static Field instance;
    public enum FieldType { ROCK, ICE };
    List<FieldSegment> segments;

    void Awake ()
    {
        instance = this;
        FieldType field = FieldType.ROCK;
    }

	void Start ()
    {
	    
	}
	
	void Update ()
    {
	
	}
}
