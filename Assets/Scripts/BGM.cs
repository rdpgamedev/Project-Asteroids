using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {
    public static BGM instance;
    public AudioClip[] songs;
    public AudioClip startSong;

    private int songIndex = 1;
    private AudioSource musicPlayer;

    private void Awake()
    {
        instance = this;
    }

	void Start () {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.volume = 0.3f;
        PlaySong();
    }
	
	void Update () {
        if (!musicPlayer.isPlaying) PlayNextSong();
	}

    public void Activate()
    {
        musicPlayer.Stop();
        musicPlayer.volume = 1f;
        musicPlayer.PlayOneShot(startSong);
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

    public void SetVolume (float vol)
    {
        if (vol < 0f) vol = 0f;
        if (vol > 1f) vol = 1f;
        musicPlayer.volume = vol;
    }
}
