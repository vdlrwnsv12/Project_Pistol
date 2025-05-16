using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 예시
/// 소리가 난 위치에서 따라다녀야 할 경우: SoundManager.Instance.PlaySFXForName("사운드 이름" or 클립, "소리 나야 할 위치", "소리의 부모로 설정할 객체");
/// 효과음 이름으로 재생: SoundManager.Instance.PlaySFXForName("등록한 사운드 이름", "소리가 나야 할 위치");
/// 효과음 클립으로 재성: SoundManager.Instance.PlaySFXForClip(사운드 클립, 소리가 나야 할 위치);
/// </summary>

[System.Serializable]
public class SoundEffectData
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [Header("Audio Prefabs")]
    [SerializeField] private GameObject sfxAudioSourcePrefab;
    [SerializeField] private Transform sfxPoolRoot;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private List<SoundEffectData> sfxList = new();
    public float MasterVol => volumeSettings.MasterVolume;
    public float SFXVol => volumeSettings.SFXVolume;
    public float BGMVol => volumeSettings.MusicVolume;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private VolumeSettings volumeSettings;
    private SFXManager sfxManager;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);

        if (sfxAudioSourcePrefab == null)
        {
            sfxAudioSourcePrefab = Resources.Load<GameObject>("Prefabs/PoolableAudioSource");//무조건 있어야함
        }

        sfxPoolRoot = gameObject.transform;


        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;

        volumeSettings = new VolumeSettings(musicSource, sfxSource);
        volumeSettings.LoadVolumes();

        sfxManager = new SFXManager(sfxAudioSourcePrefab, sfxPoolRoot, sfxList, volumeSettings);

        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(backgroundMusic);
        }
        else
        {
            Debug.LogWarning("배경음악이 할당되지 않았습니다.");
        }
    }
    private void OnEnable()
    {
        volumeSettings.ApplyVolumes();
    }

    #region Background Music
    public void PlayBackgroundMusic(AudioClip music)
    {
        if (music != null)
        {
            musicSource.clip = music;
            musicSource.Play();
        }
    }

    public void PauseBackgroundMusic() => musicSource.Pause();
    public void ResumeBackgroundMusic() => musicSource.Play();
    #endregion

    #region SFX
    public void PlaySFXForName(string soundName, Vector3 position) =>
    sfxManager.PlaySFXForName(soundName, position);
    public void PlaySFXForName(string soundName, Vector3 position, GameObject parent) =>
        sfxManager.PlaySFXForName(soundName, position, parent);

    public void PlaySFXForClip(AudioClip clip, Vector3 position) =>
        sfxManager.PlaySFX(clip, position);
    #endregion

    #region Volume Controls
    public void SetMasterVolume(float volume) => volumeSettings.SetMasterVolume(volume);
    public void SetSFXVolume(float volume) => volumeSettings.SetSFXVolume(volume);
    public void SetMusicVolume(float volume) => volumeSettings.SetMusicVolume(volume);
    #endregion
}