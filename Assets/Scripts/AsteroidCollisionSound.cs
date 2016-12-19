using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class AsteroidCollisionSound : MonoBehaviour {
    
    public AudioClip[] sounds; 

    private AudioSource audioSource;
    private GameObject gameCamera;

    void Start () {
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        audioSource = GetComponent<AudioSource>();
        int randomIndex = Random.Range(0, sounds.Length);
        AudioClip clip = sounds[randomIndex];
        if (isVisible())
        {
            audioSource.PlayOneShot(clip);
        }
	}

    bool isVisible ()
    {
        Vector3 viewportpos = gameCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
        return (viewportpos.x >= 0 && viewportpos.x <= 1 && viewportpos.y >= 0 && viewportpos.y <= 1);
    }
}
