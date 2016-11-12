/*
 * Controls the input for the player object.
 * Reads player input and sends commands to
 * the player object.
 */

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static float xaxisleft;
    public static float yaxisleft;
    public static float xaxisright;
    public static float yaxisright;

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
        //smooth input axes axis = axis + delta * 0.3
        xaxisleft = xaxisleft + (Input.GetAxis("Left Horizontal") - xaxisleft) * Time.deltaTime * 4;
        yaxisleft = yaxisleft + (Input.GetAxis("Left Vertical") - yaxisleft) * Time.deltaTime * 4;
        xaxisright = xaxisright + (Input.GetAxis("Right Horizontal") - xaxisright) * Time.deltaTime * 4;
        yaxisright = yaxisright + (Input.GetAxis("Right Vertical") - yaxisright) * Time.deltaTime * 4;
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
