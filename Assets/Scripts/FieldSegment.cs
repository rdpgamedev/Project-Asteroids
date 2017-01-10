using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FieldType = Field.FieldType;
using TrackType = Field.TrackType;

public class FieldSegment : MonoBehaviour
{
    public static float MAXLENGTH = 1200f;
    public static float MINLENGTH = 1000f;
    public static float MAXHEIGHT = 1000f;
    public float linePointOffset;
    public GameObject LINEPOINT;
    public GameObject ASTEROID;
    public bool isActive;
    public int asteroidCount;
    public GameObject fieldParticles;

    private FieldType fieldtype;
    private TrackType tracktype;
    private Bezier curve;
    private float length;
    private float height;
    private int numpoints; //number of linepoints on curve; checkpoint is first point
    private float pointdensity = 80f; //more means less linepoints
    private List<GameObject> landmarks;
    private GameObject checkpoint;
    private GameObject nextCheckpoint;
    private bool destroy;
    private float fieldRad;
    

    void Awake ()
    {
        curve = this.GetComponent<Bezier>();
        landmarks = new List<GameObject>();
    }

    void Start ()
    {
        fieldRad = MAXLENGTH * 4f;
	}
	
	void Update ()
    {
	    if (destroy && checkDistance(fieldRad) )
        {
            Destroy(this.gameObject);
        }
	}

    void OnDestroy ()
    {
        Field.instance.asteroidCount -= asteroidCount;
    }

    public void GenerateSegment(FieldType _fieldtype, TrackType _tracktype, Vector3 lastControlPoint)
    {
        fieldtype = _fieldtype;
        tracktype = _tracktype;
        length = Random.Range(MINLENGTH, MAXLENGTH);
        height = Random.Range(0f, MAXHEIGHT);

        //Create Bezier curve
        Vector3 p0, p1 ,p2, p3;
        p0 = p1 = p2 = p3 = transform.position;
        Vector3 forward = p0 - lastControlPoint;
        forward.Normalize();
        //setup bezier points
        switch (tracktype)
        {
            case TrackType.STRAIGHT:
                {
                    p1 = p0 + forward * length / 3;
                    RandomlyOffset(p1, height);
                    p2 = p0 + forward * length * 2 / 3;
                    RandomlyOffset(p2, height);
                    p3 = p0 + forward * length;
                    RandomlyOffset(p3, height);
                    break;
                }
            case TrackType.CURVE:
                {
                    //CREATE END VECTOR between 30 and 90 DEGREES OF FORWARD
                    float theta = Random.Range(Mathf.PI / 6f, Mathf.PI / 2f);
                    Vector3 endDirection = RandomlyOffset(-forward, 0.05f, 0.1f);
                    Vector3 end = Vector3.RotateTowards(
                        forward, endDirection, theta, 0f);
                    end.Normalize();
                    p1 = p0 + forward * length / 3;
                    RandomlyOffset(p1, height);
                    p2 = p0 + forward * length / 2 + end * length / 6;
                    RandomlyOffset(p2, height);
                    p3 = p2 + end * length / 3;
                    RandomlyOffset(p3, height);
                    break;
                }
            case TrackType.SLALOM:
                {
                    Vector3 backDirection = RandomlyOffset(-forward, 0.05f, 0.1f);
                    Vector3 approxNormal = Vector3.RotateTowards(forward, backDirection, Mathf.PI / 2f, 0f);
                    approxNormal.Normalize();
                    p1 = p0 + forward * length / 3;
                    RandomlyOffset(p1, height);
                    p2 = p0 + forward * length * 2 / 3 + approxNormal * length / 6;
                    RandomlyOffset(p2, height);
                    p3 = p0 + forward * length - approxNormal * length / 6;
                    RandomlyOffset(p3, height);
                    break;
                }
            case TrackType.HAIRPIN:
                {
                    //CREATE END VECTOR WITHIN 90 and 180 DEGREES OF FORWARD
                    float theta = Random.Range(Mathf.PI / 2f, 3f * Mathf.PI / 4f);
                    Vector3 endDirection = RandomlyOffset(
                        -forward, 0.05f, 0.1f);
                    Vector3 end = Vector3.RotateTowards(
                        forward, endDirection, theta, 0f);
                    end.Normalize();
                    p1 = p0 + forward * length / 3;
                    RandomlyOffset(p1, height);
                    p2 = p0 + forward * length / 2 + end * length / 6;
                    RandomlyOffset(p2, height);
                    p3 = p2 + end * length / 3;
                    RandomlyOffset(p3, height);
                    pointdensity *= 0.7f;
                    break;
                }
            default:
                {
                break;
                }
        }
        p2.y /= 3;
        p3.y /= 3;
        curve.SetPoints(p0, p1, p2, p3);
        length = curve.ArcLength();
        numpoints = (int)(length / pointdensity);
        //Spawn Landmarks
        if (Random.Range(0, 5) > 3)
        { 
            Vector3 landmarkpoint = curve.GetPoint(0.5f);
            Vector3 normal = curve.GetNormal(0.5f);
            landmarkpoint += normal * Random.Range(800f, 1000f);
            GameObject landmark = SpawnLandmark(landmarkpoint);
            landmark.transform.parent = transform;
            landmarks.Add(landmark);
        }
        //Spawn Linepoints
        for (int i = 1; i < numpoints; ++i)
        {
            float t = (float)i / (float)numpoints;
            GameObject newLinePoint = Instantiate<GameObject>(LINEPOINT);
            newLinePoint.transform.position = RandomlyOffset(curve.GetPoint(t), linePointOffset);
            newLinePoint.transform.parent = transform;
            newLinePoint.GetComponent<Linepoint>().fieldSegment = this;
            newLinePoint.GetComponent<RandomModel>().ChooseOre();
        }
        //Spawn Asteroids
        int unspawnedAsteroids = 
            Field.instance.MINASTEROIDS + Field.instance.checkpointsMade * 3;
        for (int i = 0; i < (unspawnedAsteroids); ++i)
        {
            Vector3 point = curve.GetPoint((float)i / 100f);
            GameObject asteroid = SpawnAsteroid(point);
            bool collided = true; //tracking if collided with a landmark
            asteroid.transform.forward = curve.GetFirstDeriv((float)i / 100f);
            while (collided)
            {
                Vector3 offset = RandomlyOffsetXY(point, 500f) - point;
                asteroid.transform.position = point;
                asteroid.transform.position += asteroid.transform.right.normalized * offset.x;
                asteroid.transform.position += asteroid.transform.up.normalized * offset.y;
                collided = false;
                foreach (GameObject lm in landmarks)
                {
                    if (lm.GetComponent<MeshCollider>().bounds.Intersects(
                        asteroid.GetComponent<MeshCollider>().bounds))
                    {
                        collided = true;
                        break;
                    }
                }
            }
        }
        //Spawn Field Particles
        Vector3 midpoint = curve.GetPoint(0.5f);
        GameObject particles = Instantiate<GameObject>(fieldParticles);
        particles.transform.parent = transform;
        particles.transform.position = midpoint;
        particles.transform.forward = curve.GetFirstDeriv(0.5f);
        if (fieldtype == FieldType.ICE)
        {
            Color iceCloud = new Color(52f / 255f, 65f / 255f, 100f / 255f, 87f / 255f);
            var particlesMain = particles.GetComponent<ParticleSystem>().main;
            particlesMain.startColor = iceCloud;
        }
    }

    public void Destroy()
    {
        destroy = true;
    }

    public GameObject SpawnAsteroid (Vector3 position)
    {
        GameObject asteroid = Instantiate<GameObject>(ASTEROID);
        asteroid.GetComponent<RandomModel>().ChooseAsteroid(fieldtype);
        asteroid.transform.localScale *= Random.Range(30f, 45f);
        asteroid.GetComponent<Rigidbody>().velocity =
            new Vector3(Random.Range(-10f, 10f),
                        Random.Range(-10f, 10f),
                        Random.Range(-10f, 10f));
        asteroid.transform.position = position;
        asteroid.GetComponent<AsteroidCollision>().segment = this;
        asteroid.transform.parent = transform;
        ++Field.instance.asteroidCount;
        ++asteroidCount;
        return asteroid;
    }

    public GameObject SpawnLandmark (Vector3 position)
    {
        GameObject landmark = Instantiate<GameObject>(ASTEROID);
        landmark.transform.position = position;
        landmark.GetComponent<RandomModel>().ChooseAsteroid(fieldtype);
        landmark.transform.localScale *= Random.Range(300f, 450f);
        landmark.GetComponent<Rigidbody>().mass = 999999f;
        landmark.GetComponent<RandomRotation>().enabled = false;
        landmark.name = "Landmark";
        return landmark;
    }

    Vector3 RandomlyOffset(Vector3 point, float delta)
    {
        Vector3 newpoint = point;
        Vector3 offset = new Vector3(Random.Range(-delta, delta),
                                     Random.Range(-delta, delta),
                                     Random.Range(-delta, delta));
        newpoint += offset;
        return newpoint;
    }

    //offsets a Vector3 with two different max deltas, one for
    //the horizontal plane and one in the vertical axis.
    Vector3 RandomlyOffset(Vector3 point, float deltaH, float deltaX)
    {
        Vector3 newpoint = point;
        Vector3 offset = new Vector3(Random.Range(-deltaX, deltaX),
                                     Random.Range(-deltaH, deltaH),
                                     Random.Range(-deltaX, deltaX));
        newpoint += offset;
        return newpoint;
    }

    Vector3 RandomlyOffsetXY(Vector3 point, float deltaXY)
    {
        Vector3 newpoint = point;
        Vector3 offset = new Vector3(Random.Range(-deltaXY, deltaXY),
                                     Random.Range(-deltaXY, deltaXY),
                                     0f);
        newpoint += offset;
        return newpoint;
    }

    public void SetCheckpoint (GameObject _checkpoint)
    {
        checkpoint = _checkpoint;
        checkpoint.GetComponent<Animator>().enabled = false;
    }

    public GameObject GetCheckpoint ()
    {
        return checkpoint;
    }

    public void SetNextCheckpoint (GameObject _nextCheckpoint)
    {
        nextCheckpoint = _nextCheckpoint;
    }

    public void SetActive (bool setting)
    {
        isActive = setting;
        if (isActive)
        {
            nextCheckpoint.GetComponent<Animator>().enabled = true;
            nextCheckpoint.GetComponent<AudioSource>().PlayDelayed(1f);
        }
    }

    private bool checkDistance(float dist)
    {
        return ((PlayerShip.instance.transform.position - transform.position).magnitude > dist);
    }
}
