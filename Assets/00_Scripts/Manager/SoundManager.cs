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
    [Tooltip("배경음악 볼륨")]
    [Range(0f, 1f)]
    public float backgroundMusicVol = 0.5f;
    [Tooltip("효과음 볼륨")]
    [Range(0f, 1f)]
    public float sfxVol = 0.5f;

    [Header("오디오 클립")]
    public AudioClip backgroundMusic;
    public Dictionary<String, AudioClip> soundEffects = new Dictionary<string, AudioClip>();



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

        musicSource.volume = backgroundMusicVol;
        sfxSource.volume = sfxVol;
        musicSource.loop = true;
    }

    void Start()
    {
        PlayBackgroundMusic(backgroundMusic);
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
        //ToDo 음악멈추기
    }

    public void ResumeBackgroundMusic()
    {
        //ToDo 음악다시켜기
    }
    #endregion

    #region 효과음 관련
    /// <summary>
    /// 사운드 넣고 싶은거 넣으세요
    /// </summary>
    /// <param name="string"></param>
    /// <param name="AudioClip"></param>
    public void AddSoundEffect(string soundName, AudioClip clip)
    {
        if (!soundEffects.ContainsKey(soundName))
        {
            soundEffects.Add(soundName, clip);
        }
    }

    /// <summary>
    /// 사운드 효과 재생
    /// </summary>
    /// <param name="string"></param>
    /// 
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
                Debug.Log("현재 pitch: " + sfxSource.pitch);
            }
            else
            {
                Debug.Log("sound 못찾음 " + soundName);
            }
        }
    }
    /// <summary>
    /// 클립 직접 넘겨서 재생
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVol);
            Debug.Log($"사운드 출력{clip.name}");
        }
    }
    #endregion

    #region 볼륨
    /// <summary>
    /// 효과음 볼륨
    /// </summary>
    /// <param name="float"></param>
    public void SetSFXVolume(float volume)
    {
        sfxVol = Mathf.Clamp(volume, 0f, 1f);
        sfxSource.volume = sfxVol;
    }

    /// <summary>
    /// 배경음악 볼륨
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(float volume)
    {
        backgroundMusicVol = Mathf.Clamp(volume, 0f, 1f);
        musicSource.volume = backgroundMusicVol;
    }
    #endregion
}

