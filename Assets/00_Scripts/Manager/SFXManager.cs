using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager
{
    private readonly GameObject sfxPrefab;
    private readonly Transform poolRoot;
    private readonly VolumeSettings volumeSettings;
    private readonly Dictionary<string, AudioClip> sfxDict = new();

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

        var pooled = ObjectPoolManager.Instance.GetObject(sfxPrefab.GetComponent<PooledAudio>(), position, Quaternion.identity, clip.length);
        var PooledAudio = pooled.GetComponent<PooledAudio>();

        if (parent != null)
            PooledAudio.transform.SetParent(parent);

        float vol = volumeSettings.SFXVolume * volumeSettings.MasterVolume;
        PooledAudio.Play(clip, vol);
    }
}
