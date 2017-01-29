using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FieldType = Field.FieldType;
using TrackType = Field.TrackType;

public class FieldSegment : MonoBehaviour
{
    public static float MAXLENGTH = 1200f;
    public static float MINLENGTH = 1000f;
    public static float MAXHEIGHT = 300f;
    public static float LENGTHMULTIPLIER = 1.5f;
    public float linePointOffset;
    public GameObject LINEPOINT;
    public GameObject ASTEROID;
    public bool isActive;
    public int asteroidCount;
    public GameObject fieldParticles;
    public bool isGenerating;
    public GameObject nextCheckpoint;
    public FieldSegment nextSegment;
    public FieldSegment prevSegment;

    private FieldType fieldtype;
    private TrackType tracktype;
    private Bezier curve;
    private float length;
    private float height;
    private int numpoints; //number of linepoints on curve; checkpoint is first point
    private float pointdensity = 80f; //more means less linepoints
    private List<GameObject> landmarks;
    private GameObject checkpoint;
    private bool destroy;
    private float fieldRad;
    private float generateTime;
    

    void Awake ()
    {
        curve = this.GetComponent<Bezier>();
        landmarks = new List<GameObject>();
    }

    void Start ()
    {
        Debug.Log("Starting FieldSegment");
        fieldRad = MAXLENGTH * 4f;
	}
	
	void Update ()
    {
        if (Time.time - generateTime > 1.5f) isGenerating = false;
        if (destroy && CheckDistance(fieldRad) )
        {
            Field.instance.RemoveSegment(this.gameObject);
            Destroy(this.gameObject);
        }
	}

    void OnDestroy ()
    {
        Field.instance.asteroidCount -= asteroidCount;
    }

    public void GenerateSegment(FieldType _fieldtype, TrackType _tracktype, Vector3 lastControlPoint)
    {
        Debug.Log("Generating Segment");
        isGenerating = true;
        generateTime = Time.time;
        fieldtype = _fieldtype;
        tracktype = _tracktype;
        length = Random.Range(MINLENGTH, MAXLENGTH) * (GameManager.instance.difficulty * LENGTHMULTIPLIER  + 1f) ;
        height = Random.Range(0f, MAXHEIGHT);
        GameObject closestSegment = Field.instance.FindClosestSegment(this.gameObject, true);
        Vector3 closestSegmentCenter = FindClosestPoint(Field.instance.SegmentMidpoints());

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
                    //create end vector between 5 and 30 degrees of forward
                    float theta = Random.Range(Mathf.PI / 36f, Mathf.PI / 6f);
                    Vector3 backDirection = RandomlyOffset(-forward, 0.05f, 0.1f);
                    Vector3 end = Vector3.RotateTowards(forward, backDirection, theta, 0f);
                    end.Normalize();
                    Vector3 segmentDirection = (forward + end) / 2f;
                    Vector3 playerDirection = p0 - PlayerShip.instance.transform.position;
                    segmentDirection = (2f*segmentDirection.normalized + playerDirection.normalized).normalized;
                    p1 = p0 + forward * length / 3f;
                    p3 = p0 + segmentDirection * length;
                    p2 = p3 - end * length / 3f;
                    break;
                }
            case TrackType.CURVE:
                {
                    //create end vector between 60 and 90 degrees of forward
                    float theta = Random.Range(Mathf.PI / 3f, Mathf.PI / 2f);
                    Vector3 backDirection = RandomlyOffset(-forward, 0.05f, 0.1f);
                    Vector3 end = Vector3.RotateTowards(
                        forward, backDirection, theta, 0f);
                    end.Normalize();
                    Vector3 segmentDirection = (forward + end) / 2f;
                    Vector3 playerDirection = p0 - PlayerShip.instance.transform.position;
                    segmentDirection = (2f*segmentDirection.normalized + playerDirection.normalized).normalized;
                    p1 = p0 + forward * length / 2f;
                    p3 = p0 + segmentDirection * length;
                    p2 = p3 - end * length / 2f;
                    break;
                }
            case TrackType.SLALOM:
                {
                    //create segment direction between 30 and 90 degrees of forward
                    float theta = Random.Range(Mathf.PI / 6f, Mathf.PI / 2f);
                    Vector3 backDirection = RandomlyOffset(-forward, 0.05f, 0.1f);
                    Vector3 segmentDirection = Vector3.RotateTowards(forward, backDirection, theta, 0f);
                    segmentDirection.Normalize();
                    Vector3 playerDirection = p0 - PlayerShip.instance.transform.position;
                    segmentDirection = (2f*segmentDirection + playerDirection.normalized).normalized;
                    p1 = p0 + forward * length;
                    p3 = p0 + segmentDirection * length;
                    p2 = p3 - forward * length;
                    break;
                }
            case TrackType.HAIRPIN:
                {
                    //CREATE END VECTOR WITHIN 90 and 180 DEGREES OF FORWARD
                    float theta = Random.Range(Mathf.PI / 2f, Mathf.PI);
                    Vector3 backDirection = RandomlyOffset(-forward, 0.05f, 0.1f);
                    Vector3 end = Vector3.RotateTowards(
                        forward, backDirection, theta, 0f);
                    end.Normalize();
                    Vector3 segmentDirection = (forward + end) / 2f;
                    Vector3 playerDirection = p0 - PlayerShip.instance.transform.position;
                    segmentDirection = (1.5f*segmentDirection.normalized + playerDirection.normalized).normalized;
                    p1 = p0 + forward * length / 2f;
                    p3 = p0 + segmentDirection * length / 2f;
                    p2 = p3 - end * length / 2f;
                    pointdensity *= 0.7f;
                    break;
                }
            default:
                {
                break;
                }
        }
        //p2.y /= 3;
        //p3.y /= 3;
        curve.SetPoints(p0, p1, p2, p3);
        length = curve.ArcLength();
        numpoints = (int)(length / pointdensity);
        //Spawn Landmark
        if (Random.Range(0, 5) > 3)
        { 
            Vector3 landmarkpoint = curve.GetPoint(0.5f);
            if ((landmarkpoint - PlayerShip.instance.transform.position).magnitude > MAXLENGTH * 2 || !Field.instance.activated)
            {
                if ((landmarkpoint - closestSegmentCenter).magnitude > MINLENGTH / 4f)
                {
                    Vector3 normal = curve.GetNormal(0.5f);
                    normal.Normalize();
                    landmarkpoint += normal * Random.Range(800f, 1000f);
                    Debug.Log("Normal: " + normal + " for Landmark at " + (landmarkpoint - transform.position));
                    GameObject landmark = SpawnLandmark(landmarkpoint);
                    landmark.transform.parent = transform;
                    landmarks.Add(landmark);
                }
            }
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
        int unspawnedAsteroids = Mathf.Min(
            Field.instance.MINASTEROIDS + Field.instance.checkpointsMade * 2,
            60);
        for (int i = 0; i < (unspawnedAsteroids); ++i)
        {
            Vector3 point = curve.GetPoint((float)i / (float)unspawnedAsteroids);
            if ((point - PlayerShip.instance.transform.position).magnitude > MAXLENGTH * 2 || !Field.instance.activated)
            {
                GameObject asteroid = SpawnAsteroid(point);
                bool collided = true; //tracking if collided with a landmark
                asteroid.transform.forward = curve.GetFirstDeriv((float)i / (float)unspawnedAsteroids);
                Vector2 offset = Random.insideUnitCircle * 300f;
                asteroid.transform.position = point;
                asteroid.transform.position += asteroid.transform.right.normalized * offset.x;
                asteroid.transform.position += asteroid.transform.up.normalized * offset.y;
                Vector3 asteroidLandmarkOffset = new Vector3();
                while (collided)
                {
                    asteroid.transform.position += asteroidLandmarkOffset.normalized * 5f;
                    collided = false;
                    foreach (GameObject lm in landmarks)
                    {
                        if (lm.GetComponent<MeshCollider>().bounds.Intersects(
                            asteroid.GetComponent<MeshCollider>().bounds))
                        {
                            collided = true;
                            asteroidLandmarkOffset = asteroid.transform.position - lm.transform.position;
                            break;
                        }
                    }
                }
                //check for overlap and destroy asteroid if necessary
                if (closestSegment != null)
                {
                    Bezier closestCurve = closestSegment.GetComponent<FieldSegment>().curve;
                    Vector3 asteroidPos = asteroid.transform.position;
                    float approxDistanceToCurve = closestCurve.ClosestDistToCurve(asteroidPos, 5);
                    if (approxDistanceToCurve < 500f)
                    {
                        Destroy(asteroid);
                    }
                }
            }
        }
        //Spawn Field Particles
        Vector3 midpoint = curve.GetPoint(0.5f);
        GameObject particles = Instantiate<GameObject>(fieldParticles);
        particles.name = "Field Particles";
        particles.transform.parent = transform;
        particles.transform.position = midpoint;
        particles.transform.forward = curve.GetFirstDeriv(0.5f);
        ParticleSystem.MainModule particlesMain = particles.GetComponent<ParticleSystem>().main;
        if (fieldtype == FieldType.ICE)
        {
            Color iceCloud = new Color(52f / 255f, 65f / 255f, 100f / 255f, 87f / 255f);
            particlesMain.startColor = iceCloud;
        }
        GameObject lastSegment = Field.instance.LastSegment();
        int oldMaxParticles;
        if (lastSegment == null)
        {
            oldMaxParticles = 5000;
        }
        else
        {
            GameObject oldParticles = lastSegment.transform.FindChild("Field Particles").gameObject;
            oldMaxParticles = oldParticles.GetComponent<ParticleSystem>().main.maxParticles;
        }
        if (oldMaxParticles == 2000)
        {
            if (Random.Range(0f, 1f) < 0.6f)
            {
                particlesMain.maxParticles = 2000;
            }
            else
            {
                particlesMain.maxParticles = oldMaxParticles + Random.Range(-oldMaxParticles + 2000, 10000 - oldMaxParticles + 2000) / 3;
            }
        }
        else
        {
            if (Random.Range(0f, 1f) < 0.6f)
            {
                particlesMain.maxParticles = oldMaxParticles + Random.Range(-oldMaxParticles + 2000, 10000 - oldMaxParticles + 2000) / 3;
            }
            else
            {
                particlesMain.maxParticles = 2000;
            }
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
        float scale;
        asteroid.transform.localScale *= scale = Random.Range(30f, 45f);
        asteroid.GetComponent<Rigidbody>().mass *= scale * scale * scale;
        asteroid.GetComponent<Rigidbody>().velocity = Random.onUnitSphere;
        asteroid.GetComponent<Rigidbody>().velocity *= Random.Range(5f, 40f);
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
        landmark.GetComponent<Rigidbody>().isKinematic = true;
        landmark.GetComponent<MeshCollider>().convex = false;
        landmark.GetComponent<RandomRotation>().enabled = false;
        landmark.GetComponent<AsteroidCollision>().segment = this;
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

    private bool CheckDistance(float dist)
    {
        return ((PlayerShip.instance.transform.position - transform.position).magnitude > dist);
    }

    public void RepositionAsteroid(GameObject asteroid)
    {
        asteroid.transform.position = RandomlyOffset(asteroid.transform.position, 20f);
    }

    public Vector3 GetCurveCenter()
    {
        return curve.GetPoint(0.5f);
    }

    public bool IsSegmentAdjacent(GameObject segment)
    {
        FieldSegment fieldSegment = segment.GetComponent<FieldSegment>();
        if (segment == null) return false;
        return (fieldSegment == prevSegment || fieldSegment == nextSegment);
    }

    private Vector3 FindClosestPoint (List<Vector3> points)
    {
        Vector3 center = GetCurveCenter();
        if (points.Count > 0)
        {
            Vector3 closest = points[0];
            float dist = (center - closest).magnitude;
            foreach (Vector3 point in points)
            {
                float newDist = (center - point).magnitude;
                if (newDist < dist)
                {
                    closest = point;
                    dist = newDist;
                }
            }
            return center;
        }
        else
        {
            Vector3 offset = (center - transform.position) * 2;
            return (center - offset);
        }
    }
}
