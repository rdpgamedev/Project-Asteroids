using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {
    public float scale = 0.1f;
    public int MAX_COUNTER = 2;

    private int counter = 0;

    void Start ()
    {
        
    }

    void Update ()
    {
        if (counter == 0)
        {
            Vector3 direction = Random.onUnitSphere;
            transform.position += direction * scale;
        }
        ++counter;
        if (counter > MAX_COUNTER) counter = 0;
    }
}
