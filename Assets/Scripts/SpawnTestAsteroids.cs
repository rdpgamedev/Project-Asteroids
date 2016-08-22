/*
 * Spawns NUMBER_OF_ASTEROIDS test asteroids (SPHEREs) with random transforms 
 * in a cube space from negative and positive RADIUS in the 3 axes.
 * MINSCALES sets the minimum size of the test asteroids and the X, Y, and
 * ZSCALES determine the maximum size of the asteroids in those axes.
 */

using UnityEngine;
using System.Collections;

public class SpawnTestAsteroids : MonoBehaviour {
    public int NUMBER_OF_ASTEROIDS = 20;
    public GameObject SPHERE;
    public float RADIUS = 50f;
    public float MINSCALE = 5f;
    public float XSCALE = 15f,
        YSCALE = 8f,
        ZSCALE = 8f;
	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < NUMBER_OF_ASTEROIDS; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-RADIUS, RADIUS), 
                Random.Range(-RADIUS, RADIUS), 
                Random.Range(-RADIUS, RADIUS));
            GameObject asteroid = Instantiate<GameObject>(SPHERE);
            asteroid.transform.Translate(randomPosition);
            asteroid.transform.rotation = Random.rotation;
            asteroid.transform.localScale = new Vector3(
                Random.Range(MINSCALE, XSCALE),
                Random.Range(MINSCALE, YSCALE),
                Random.Range(MINSCALE, ZSCALE));
        }
	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //CREATE A SPAWN ASTEROID FUNCTION
}
