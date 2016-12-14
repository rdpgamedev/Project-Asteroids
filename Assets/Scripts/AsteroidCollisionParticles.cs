using UnityEngine;
using System.Collections;

public class AsteroidCollisionParticles : MonoBehaviour {
    private ParticleSystem particles;
	void Start () {
        particles = GetComponent<ParticleSystem>();
	}
	
	void Update () {
	    if (!particles.IsAlive())
        {
            Destroy(this.gameObject);
        }
	}
}
