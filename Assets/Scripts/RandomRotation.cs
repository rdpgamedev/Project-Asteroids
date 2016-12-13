using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour
{
    //Tweakable variables from editor.
    public float MAXIMUM_SPEED = 0.1f;

    //Remaining variables for the script.
    float deltaAngleX, deltaAngleY, deltaAngleZ;
    Vector3 eulerRotation;


	// Use this for initialization
	void Start ()
    {
        deltaAngleX = Random.Range(-MAXIMUM_SPEED, MAXIMUM_SPEED);
        deltaAngleY = Random.Range(-MAXIMUM_SPEED, MAXIMUM_SPEED);
        deltaAngleZ = Random.Range(-MAXIMUM_SPEED, MAXIMUM_SPEED);
        eulerRotation = new Vector3(deltaAngleX, deltaAngleY, deltaAngleZ);
        GetComponent<Rigidbody>().angularVelocity = eulerRotation;
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
}

