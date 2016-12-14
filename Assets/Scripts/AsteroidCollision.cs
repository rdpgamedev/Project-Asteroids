using UnityEngine;
using System.Collections;

public class AsteroidCollision : MonoBehaviour {
    public int MAXCHILDRENASTEROIDS = 4;
    public int colliders;
    public FieldSegment segment;
    public bool isChild = false;
    public GameObject AsteroidParticleSystem;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}



    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.name.Contains("Asteroid"))
        {
            if (!isChild)
            {
                GameObject oldAsteroid = collision.gameObject;
                Vector3 position = oldAsteroid.transform.position;
                Vector3 scale = oldAsteroid.transform.localScale;
                Destroy(collision.gameObject);
                if (segment != null)
                {
                    --segment.asteroidCount;
                }
                GameObject particleSystem = Instantiate<GameObject>(AsteroidParticleSystem);
                particleSystem.transform.parent = segment.transform;
                particleSystem.transform.position = position;
                //change to ice particles if ice asteroid
                if (GetComponent<MeshRenderer>().material.name.Contains("Ice"))
                {
                    Color iceBlue = new Color(60f / 255f, 75f / 255f, 75f / 255f, 0.2f);
                    particleSystem.GetComponent<ParticleSystem>().startColor = iceBlue;
                }
                //spawn smaller asteroids
                int asteroidCount = Random.Range(2, MAXCHILDRENASTEROIDS);
                for (int i = 0; i < asteroidCount; ++i)
                {
                    GameObject newAsteroid = segment.SpawnAsteroid(position);
                    newAsteroid.transform.localScale = scale / Mathf.Sqrt(asteroidCount);
                    newAsteroid.GetComponent<AsteroidCollision>().isChild = true;
                }
            }
        }
    }

    void OnTriggerEnter (Collider collider)
    {
        ++colliders;
        GetComponent<MeshCollider>().enabled = true;
    }

    void OnTriggerExit ()
    {
        --colliders;
        if (colliders == 0) GetComponent<MeshCollider>().enabled = false;
    }
}
