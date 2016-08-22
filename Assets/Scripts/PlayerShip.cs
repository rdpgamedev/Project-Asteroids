/*
 * Handles the state of the player ship.
 * This includes movement, combat, pickups, etc.
 */

using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour {

    //instance of PlayerShip for other scripts to access
    public static PlayerShip instance;

	// Use this for initialization
	void Start ()
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void DebugMoveForward()
    {
        Debug.Log("Moving forward.");
    }

    public void DebugMoveBackward()
    {
        Debug.Log("Moving backward.");
    }

    public void DebugMoveLeft()
    {
        Debug.Log("Moving left.");
    }

    public void DebugMoveRight()
    {
        Debug.Log("Moving right.");
    }

    public void DebugMoveUp()
    {
        Debug.Log("Moving up.");
    }

    public void DebugMoveDown()
    {
        Debug.Log("Moving down.");
    }

    public void DebugRollLeft()
    {
        Debug.Log("Rotating left.");
    }

    public void DebugRollRight()
    {
        Debug.Log("Rotating right.");
    }
}
