using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {
    public static BGM instance;
    public AudioClip[] songs;

    private int songIndex = 1;
    private AudioSource musicPlayer;

    private void Awake()
    {
        instance = this;
    }

	void Start () {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.PlayOneShot(songs[songIndex++]);
	}
	
	// Update is called once per frame
	void Update () {
        if (!musicPlayer.isPlaying) PlayNextSong();
	}

    public void PlaySong()
    {
        musicPlayer.Stop();
        musicPlayer.PlayOneShot(songs[songIndex]);
    }

    public void PlayNextSong ()
    {
        ++songIndex;
        if (songIndex >= songs.Length) songIndex = 0;
        PlaySong();
    }

    public void PlayPreviousSong ()
    {
        --songIndex;
        if (songIndex < 0) songIndex = songs.Length - 1;
        PlaySong();
    }
}
