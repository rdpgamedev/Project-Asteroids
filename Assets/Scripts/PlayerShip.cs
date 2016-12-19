/*
 * Handles the state of the player ship.
 * This includes movement, combat, pickups, etc.
 */

using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour
{

    //instance of PlayerShip for other scripts to access
    public static PlayerShip instance;
    public GameObject leftthruster;
    public GameObject rightthruster;
    public float MAXTHRUST = 60f;
    public float thrustScale = 20f;
    public float thrusterOffsetMin = 0.35f;

    void Awake ()
    {
        instance = this;
    }

	void Start ()
    {
	}
	
	void Update ()
    {
        float velocity = GetComponent<Rigidbody>().velocity.magnitude;
        GameManager.instance.score += (int)(velocity * 
            Time.deltaTime * GameManager.instance.multiplier);
        if (thrustScale > MAXTHRUST) thrustScale = MAXTHRUST;
	}

    void OnCollisionEnter(Collision collision)
    {
        
    }
}
