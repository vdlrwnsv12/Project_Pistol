using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 예시
/// 배경음악: SoundManager.Instance.PlayBackgroundMusic("사운드 이름");
/// 효과음: SoundManager.Instance.PlaySFX("사운드 이름");
/// </summary>

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Settings")]
    [Tooltip("마스터 볼륨")]
    [Range(0f, 1f)]
    public float masterVol = 1f;
    [Tooltip("배경음악 볼륨")]
    [Range(0f, 1f)]
    public float backgroundMusicVol = 0.5f;
    [Tooltip("효과음 볼륨")]
    [Range(0f, 1f)]
    public float sfxVol = 0.5f;

    [Header("오디오 클립")]
    public AudioClip backgroundMusic;
    public Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Ensure the music and sfx sources are assigned
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();  // 효과음 소스
        if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();  // 배경음악 소스

        musicSource.loop = true;
        PlayBackgroundMusic(backgroundMusic);

        // 초기 볼륨 설정
        SetMasterVolume(masterVol);
        SetSFXVolume(sfxVol);
        SetMusicVolume(backgroundMusicVol);
    }

    private void OnEnable()
    {
        // 슬라이더 값 초기화
        SetMasterVolume(masterVol);
        SetSFXVolume(sfxVol);
        SetMusicVolume(backgroundMusicVol);
    }

    #region 배경음악 관련
    void PlayBackgroundMusic(AudioClip music)
    {
        if (music != null)
        {
            musicSource.clip = music;
            musicSource.Play();
        }
    }

    public void PauseBackgroundMusic()
    {
        musicSource.Pause();
    }

    public void ResumeBackgroundMusic()
    {
        musicSource.Play();
    }
    #endregion

    #region 효과음 관련
    public void AddSoundEffect(string soundName, AudioClip clip)
    {
        if (!soundEffects.ContainsKey(soundName))
        {
            soundEffects.Add(soundName, clip);
        }
    }

    public void PlaySFX(string soundName)
    {
        if (soundEffects.TryGetValue(soundName, out AudioClip clip))
        {
            sfxSource.pitch = 1f;
            sfxSource.PlayOneShot(clip, sfxVol);
        }
        else
        {
            clip = Resources.Load<AudioClip>("Audio/SFX/" + soundName);

            if (clip != null)
            {
                soundEffects[soundName] = clip;
                sfxSource.pitch = 1f;
                sfxSource.PlayOneShot(clip, sfxVol);
            }
            else
            {
                Debug.Log("sound 못찾음 " + soundName);
            }
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVol);
        }
    }
    #endregion

    #region 볼륨
    public void SetMasterVolume(float volume)
    {
        masterVol = Mathf.Clamp(volume, 0f, 1f);
        sfxSource.volume = sfxVol * masterVol;
        musicSource.volume = backgroundMusicVol * masterVol;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVol = Mathf.Clamp(volume, 0f, 1f);
        sfxSource.volume = sfxVol * masterVol; // masterVol도 반영
    }

    public void SetMusicVolume(float volume)
    {
        backgroundMusicVol = Mathf.Clamp(volume, 0f, 1f);
        musicSource.volume = backgroundMusicVol * masterVol; // masterVol도 반영
    }
    #endregion
}
