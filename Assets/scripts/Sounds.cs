using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip StrikeSound;
    public AudioClip SpareSounds;

    private AudioSource _audioSource;

    public static Sounds Instance;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    public void PlayStrikeSound()
    {
        _audioSource.PlayOneShot(StrikeSound);
    }

    public void PlaySpareSound()
    {
        _audioSource.PlayOneShot(SpareSounds);
    }
}
