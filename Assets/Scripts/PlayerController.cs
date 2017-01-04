/*
 * Controls the input for the player object.
 * Reads player input and sends commands to
 * the player object.
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
        xaxisleft += (Input.GetAxis("Left Horizontal") - xaxisleft) * Time.deltaTime * 4;
        yaxisleft += (Input.GetAxis("Left Vertical") - yaxisleft) * Time.deltaTime * 4;
        xaxisright += (Input.GetAxis("Right Horizontal") - xaxisright) * Time.deltaTime * 4;
        yaxisright += (Input.GetAxis("Right Vertical") - yaxisright) * Time.deltaTime * 4;
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
                GameManager.instance.Restart();
                break;
            case KeyCode.P:
                //Pause game
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
