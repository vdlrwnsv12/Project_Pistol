using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledAudio : MonoBehaviour, IPoolable
{
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void OnGetFromPool()
    {

    }
    public void OnReturnToPool()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
}
