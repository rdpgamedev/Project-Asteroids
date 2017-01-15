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
        if (segment.isGenerating && !name.Contains("Landmark"))
        {
            segment.RepositionAsteroid(gameObject);
        }
        else if (collision.gameObject.name.Contains("Asteroid") && !isChild && !name.Contains("Landmark"))
        {
            GameObject oldAsteroid = collision.gameObject;
            if (GetComponent<Rigidbody>().mass < oldAsteroid.GetComponent<Rigidbody>().mass)
            {
                if (segment != null) --segment.asteroidCount;
                GameObject particleSystem = Instantiate<GameObject>(AsteroidParticleSystem);
                particleSystem.transform.parent = segment.transform;
                particleSystem.transform.position = transform.position;
                //change to ice particles if ice asteroid
                if (GetComponent<MeshRenderer>().material.name.Contains("Ice"))
                {
                    Color iceBlue = new Color(60f / 255f, 75f / 255f, 75f / 255f, 0.2f);
                    var particlesMain = particleSystem.GetComponent<ParticleSystem>().main;
                    particlesMain.startColor = iceBlue;
                }
                //spawn smaller asteroids
                int asteroidCount = Random.Range(2, MAXCHILDRENASTEROIDS);
                for (int i = 0; i < asteroidCount; ++i)
                {
                    GameObject newAsteroid = segment.SpawnAsteroid(transform.position);
                    newAsteroid.transform.localScale = transform.localScale * (float)System.Math.Pow(asteroidCount, (-1 / 3));
                    newAsteroid.transform.localScale *= 0.5f;
                    newAsteroid.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 2;
                    newAsteroid.GetComponent<AsteroidCollision>().isChild = true;
                }
                Destroy(gameObject);
                --segment.asteroidCount;
            }
            else
            {
                gameObject.name = "Asteroid Collided";
            }
        }

        /*
        if (segment.isGenerating && !name.Contains("Landmark"))
        {
            segment.RepositionAsteroid(gameObject);
        }
        else if (collision.gameObject.name.Contains("Asteroid") && !isChild && !name.Contains("Landmark"))
        {
            GameObject oldAsteroid = collision.gameObject;
            if (GetComponent<Rigidbody>().mass > oldAsteroid.GetComponent<Rigidbody>().mass) return;
            Vector3 position = oldAsteroid.transform.position;
            Vector3 scale = oldAsteroid.transform.localScale;
            if (segment != null)
            {
                --segment.asteroidCount;
            }
            else
            {
                Debug.Log("Segment is null. Changing name.");
                gameObject.name = "MY SEGMENT IS NULL HELP";
            }
            GameObject particleSystem = Instantiate<GameObject>(AsteroidParticleSystem);
            particleSystem.transform.parent = segment.transform;
            particleSystem.transform.position = position;
            //change to ice particles if ice asteroid
            if (GetComponent<MeshRenderer>().material.name.Contains("Ice"))
            {
                Color iceBlue = new Color(60f / 255f, 75f / 255f, 75f / 255f, 0.2f);
                var particlesMain = particleSystem.GetComponent<ParticleSystem>().main;
                particlesMain.startColor = iceBlue;
            }
            //spawn smaller asteroids
            int asteroidCount = Random.Range(2, MAXCHILDRENASTEROIDS);
            for (int i = 0; i < asteroidCount; ++i)
            {
                GameObject newAsteroid = segment.SpawnAsteroid(position);
                newAsteroid.transform.localScale = scale / Mathf.Sqrt(asteroidCount);
                newAsteroid.GetComponent<AsteroidCollision>().isChild = true;
            }
            Destroy(gameObject);
            --segment.asteroidCount;
        }
        */
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
