﻿/*
 * Controls the input for the player object.
 * Reads player input and formats it into
 * usable data for the ThrusterControllers.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static float xaxisleft;
    public static float yaxisleft;
    public static float xaxisright;
    public static float yaxisright;

    public bool useSimplifiedControls = false;

    private KeyCode[] keys;
    private PlayerShip player;
    private BGM musicPlayer;

    void Start ()
    {
        //keys to check for player input
        keys = new KeyCode[] {
             KeyCode.Escape, KeyCode.P, KeyCode.R,
             KeyCode.Period, KeyCode.Comma
            };
        player = PlayerShip.instance;
        musicPlayer = BGM.instance;
	}
	
	void Update ()
    {
        if (Input.anyKeyDown) CheckInput();
        //smooth input axes axis
        xaxisleft = Input.GetAxis("Left Horizontal");
        yaxisleft = Input.GetAxis("Left Vertical");
        xaxisright = Input.GetAxis("Right Horizontal");
        yaxisright = Input.GetAxis("Right Vertical");

        if (useSimplifiedControls) mapSimplifiedControls();
        Debug.Log("SIMP Y Axis Left: " + yaxisleft);
        Debug.Log("SIMP Y Axis Right: " + yaxisright);
    }

    /*
     * Map horizontal and vertical axes of both sets of input
     * to a simplified version.
     * Left inputs:
     *   Horizontal - Roll
     *   Vertical - Pitch
     * Right inputs:
     *   Horizontal - Yaw
     *   Vertical - Thrust
     */
    void mapSimplifiedControls ()
    {
        //Placeholder axes to hold non-normalized axis values
        float simplifiedXAxisLeft = 0f;
        float simplifiedYAxisLeft = 0f;
        float simplifiedXAxisRight = 0f;
        float simplifiedYAxisRight = 0f;

        //Left axes
        float roll = xaxisleft; //adds to Y axes to impart roll
        simplifiedYAxisLeft += roll;
        simplifiedYAxisRight -= roll;
        float pitch = yaxisleft; //adds to Y axes to impart pitch
        simplifiedYAxisLeft += pitch;
        simplifiedYAxisRight += pitch;
        /*
        //Right axes
        float yaw = xaxisright; //adds to X axes to impart yaw
        simplifiedXAxisLeft += yaw;
        simplifiedXAxisRight += yaw;
        float thrust = yaxisright; //adds to X axes to impart thrust
        simplifiedXAxisLeft += thrust;
        simplifiedXAxisRight -= thrust;
        */
        //cap simplified axes to 1
        simplifiedXAxisLeft = Mathf.Min (simplifiedXAxisLeft, 1f);
        simplifiedYAxisLeft = Mathf.Min (simplifiedYAxisLeft, 1f);
        simplifiedXAxisRight = Mathf.Min (simplifiedXAxisRight, 1f);
        simplifiedYAxisRight = Mathf.Min (simplifiedYAxisRight, 1f);

        //cap simplified axes to -1
        simplifiedXAxisLeft = Mathf.Max(simplifiedXAxisLeft, -1f);
        simplifiedYAxisLeft = Mathf.Max(simplifiedYAxisLeft, -1f);
        simplifiedXAxisRight = Mathf.Max(simplifiedXAxisRight, -1f);
        simplifiedYAxisRight = Mathf.Max(simplifiedYAxisRight, -1f);

        //assign simplified axes to normal axis fields
        xaxisleft = simplifiedXAxisLeft;
        yaxisleft = simplifiedYAxisLeft;
        xaxisright = simplifiedXAxisRight;
        yaxisright = simplifiedYAxisRight;
    }

    float ClampAbs(float input, float clamp)
    {
        if (Mathf.Abs(input) <= Mathf.Abs(clamp)) return input;
        else
        {
            if (input * clamp > 0) return clamp;
            else return -clamp;
        }
    }

    void CheckInput()
    {
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
            case KeyCode.R:
                //Restart game
                //GameManager.instance.Restart();
                break;
            case KeyCode.P:
                //Pause game
                if (GameManager.instance.isPlaying) GameManager.instance.Pause();
                break;
            case KeyCode.Period:
                //Play next song
                musicPlayer.PlayNextSong();
                break;
            case KeyCode.Comma:
                //Play previous song
                musicPlayer.PlayPreviousSong();
                break;
            default:
                break;
        }
    }
}
