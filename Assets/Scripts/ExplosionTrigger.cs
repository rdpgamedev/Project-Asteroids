using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour {
    public float MAX_SIZE = 2400f;
    public float END_TIME = 5f;
    public float EXPLOSION_DELAY = 0f;

    private float size = 0f;
    private float oldsize = 0f;
    private float yScale;
    private ParticleSystem explosionSystem;
    private float timer = 0f;

    void Start () {
        yScale = transform.localScale.y;
        explosionSystem = transform.parent.FindChild("CheckpointExplosion").FindChild("Particle System 2").gameObject.GetComponent<ParticleSystem>();
    }

    void Update () {
        timer += Time.deltaTime;
        if (timer > END_TIME) Destroy(this.gameObject);
        oldsize = size;
        size = explosionSystem.sizeOverLifetime.size.Evaluate(timer/END_TIME) * MAX_SIZE;
        transform.localScale = new Vector3(size, yScale, size);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Asteroid"))
        {
            //check distance
            if ((other.transform.position - transform.position).magnitude > oldsize / 2)
            {
                other.GetComponent<AsteroidCollision>().ExplodeDelayed(EXPLOSION_DELAY);
            }
        }
    }
}
