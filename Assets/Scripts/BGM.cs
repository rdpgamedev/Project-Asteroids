using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {
    public static BGM instance;
    public AudioClip[] songs;
    public AudioClip startSong;

    private int songIndex = 0;
    private AudioSource musicPlayer;

    private void Awake()
    {
        instance = this;
    }

	void Start () {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.Stop();
        musicPlayer.PlayOneShot(startSong);
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
        if (++songIndex >= songs.Length) songIndex = 0;
        PlaySong();
    }

    public void PlayPreviousSong ()
    {
        --songIndex;
        if (songIndex < 0) songIndex = songs.Length - 1;
        PlaySong();
    }
}
