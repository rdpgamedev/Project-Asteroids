using UnityEngine;
using System.Collections;

public class AsteroidCollision : MonoBehaviour {
    public int MAXCHILDRENASTEROIDS = 3;
    public int MAXDIVISIONS = 2;
    public int colliders;
    public FieldSegment segment;
    public GameObject AsteroidParticleSystem;
    public GameObject AsteroidChildParticles;
    public int divisions = 0;

	void Start () {

	}
	
	void Update () {
	
	}

    public void ExplodeDelayed (float time)
    {
        if (time == 0f) Explode();
        else Invoke("Explode", time);
    }

    public void Explode()
    {
        ++divisions;
        if (divisions > MAXDIVISIONS) return;
        Debug.Log(divisions);
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
        int asteroidCount = Random.Range(2, MAXCHILDRENASTEROIDS + 1);
        for (int i = 0; i < asteroidCount; ++i)
        {
            GameObject newAsteroid = segment.SpawnAsteroid(transform.position);
            newAsteroid.transform.position += Random.onUnitSphere * 2f;
            newAsteroid.transform.localScale = transform.localScale * (float)System.Math.Pow(asteroidCount, (-1 / 3));
            newAsteroid.transform.localScale *= 0.5f;
            newAsteroid.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 2;
            newAsteroid.GetComponent<Rigidbody>().maxDepenetrationVelocity = 40f;
            newAsteroid.GetComponent<AsteroidCollision>().divisions = divisions;
            GameObject dustParticles = Instantiate<GameObject>(AsteroidChildParticles);
            dustParticles.transform.parent = newAsteroid.transform;
            dustParticles.transform.localPosition = Vector3.zero;
            dustParticles.transform.localScale = Vector3.one;
            //change color to asteroid's color
            var particlesMain = dustParticles.GetComponent<ParticleSystem>().main;
            particlesMain.startColor = newAsteroid.GetComponent<MeshRenderer>().material.color;
            //change shell particles
            GameObject shellParticles = dustParticles.transform.FindChild("ShellParticles").gameObject;
            var shellParticlesMain = shellParticles.GetComponent<ParticleSystem>().main;
            shellParticlesMain.startColor = newAsteroid.GetComponent<MeshRenderer>().material.color;
            var shellParticlesShape = shellParticles.GetComponent<ParticleSystem>().shape;
            shellParticlesShape.mesh = newAsteroid.GetComponent<MeshFilter>().sharedMesh;
        }
        Destroy(gameObject);
        --segment.asteroidCount;
    }

    void OnCollisionEnter (Collision collision)
    {
        if (segment.isGenerating && !name.Contains("Landmark"))
        {
            segment.RepositionAsteroid(gameObject);
        }
        else if (collision.gameObject.name.Contains("Asteroid") && !name.Contains("Landmark"))
        {
            GameObject oldAsteroid = collision.gameObject;
            if (GetComponent<Rigidbody>().mass < oldAsteroid.GetComponent<Rigidbody>().mass)
            {
                Explode();
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
