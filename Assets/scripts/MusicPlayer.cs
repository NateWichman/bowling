using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] songs;
    int currentSong;

    private AudioSource audioSource;

    private void Awake()
    {

        audioSource = GetComponent<AudioSource>();

        PlayRandomSong();

    }

    private void FixedUpdate()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomSong();
        }
    }

    private void PlayRandomSong()
    {
        currentSong = Random.Range(0, songs.Length);
        audioSource.clip = songs[currentSong];
        audioSource.Play();
    }
}