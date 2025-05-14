using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager
{
    private readonly GameObject sfxPrefab;
    private readonly Transform poolRoot;
    private readonly VolumeSettings volumeSettings;

    private readonly Queue<AudioSource> audioPool = new();
    private readonly Dictionary<string, AudioClip> sfxDict = new();

    private const int poolSize = 10;

    public SFXManager(GameObject prefab, Transform root, List<SoundEffectData> sfxList, VolumeSettings volume)
    {
        sfxPrefab = prefab;
        poolRoot = root;
        volumeSettings = volume;

        foreach (var sfx in sfxList)
        {
            if (!sfxDict.ContainsKey(sfx.name))
                sfxDict[sfx.name] = sfx.clip;
        }
    }

    public void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var go = Object.Instantiate(sfxPrefab, poolRoot);
            var source = go.GetComponent<AudioSource>();
            source.playOnAwake = false;
            go.SetActive(false);
            audioPool.Enqueue(source);
        }
    }

    public void PlaySFXForName(string soundName, Vector3 position, GameObject parentObject)
    {
        if (!sfxDict.TryGetValue(soundName, out var clip))
        {
            clip = Resources.Load<AudioClip>("Audio/SFX/" + soundName);
            if (clip != null)
                sfxDict[soundName] = clip;
            else
            {
                Debug.LogWarning($"효과음 '{soundName}'을(를) 찾을 수 없습니다.");
                return;
            }
        }

        PlaySFX(clip, position, parentObject?.transform);
    }
    public void PlaySFXForName(string soundName, Vector3 position)
    {
        PlaySFXForName(soundName, position, null); // 새 오버로드 호출
    }


    public void PlaySFX(AudioClip clip, Vector3 position, Transform parent = null)
    {
        if (clip == null)
        {
            Debug.LogWarning("SFX 클립이 null입니다.");
            return;
        }

        var source = GetAudioSource();
        source.transform.position = position;

        if (parent != null)
            source.transform.SetParent(parent);

        float vol = volumeSettings.SFXVolume * volumeSettings.MasterVolume;
        CoroutineRunner.Instance.StartCoroutine(PlayAndReturn(source, clip, vol));
    }



    private AudioSource GetAudioSource()
    {
        if (audioPool.Count > 0)
            return audioPool.Dequeue();

        var go = Object.Instantiate(sfxPrefab, poolRoot);
        var source = go.GetComponent<AudioSource>();
        source.playOnAwake = false;
        go.SetActive(false);
        return source;
    }

    private IEnumerator PlayAndReturn(AudioSource source, AudioClip clip, float volume)
    {
        source.clip = clip;
        source.volume = volume;
        source.gameObject.SetActive(true);
        source.Play();

        yield return new WaitForSeconds(clip.length);

        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        audioPool.Enqueue(source);
    }
}
