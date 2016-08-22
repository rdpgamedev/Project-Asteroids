/*
 * Controls the input for the player object.
 * Reads player input and sends commands to
 * the player object.
 */

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private KeyCode[] keys;
    private PlayerShip player;

    void Start ()
    {
        //keys to check for player input
        keys = new KeyCode[] {
             KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
             KeyCode.Q, KeyCode.E, KeyCode.Space, KeyCode.LeftShift,
             KeyCode.Escape, KeyCode.P,
             KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow
            };
        player = PlayerShip.instance;
	}
	
	void Update ()
    {
        if (Input.anyKeyDown) CheckInput();
	}

    void CheckInput()
    {
        Debug.Log("Key pressed down: " + Input.inputString);
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i])) PerformKey(keys[i]);
        }
    }

    void PerformKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Escape:
                //Quit game
                Application.Quit();
                break;
            case KeyCode.Space:
                //Move up
                break;
            case KeyCode.UpArrow:
                break;
            case KeyCode.DownArrow:
                break;
            case KeyCode.RightArrow:
                break;
            case KeyCode.LeftArrow:
                break;
            case KeyCode.A:
                //Move left
                player.DebugMoveLeft();
                break;
            case KeyCode.D:
                //Move right
                player.DebugMoveRight();
                break;
            case KeyCode.E:
                //Roll right
                player.DebugRollRight();
                break;
            case KeyCode.P:
                //Pause
                break;
            case KeyCode.Q:
                //Roll left
                player.DebugRollLeft();
                break;
            case KeyCode.S:
                //Move backward
                player.DebugMoveBackward();
                break;
            case KeyCode.W:
                //Move forward
                player.DebugMoveForward();
                break;
            case KeyCode.LeftShift:
                //Move down
                player.DebugMoveDown();
                break;
            default:
                break;
        }
    }
}
