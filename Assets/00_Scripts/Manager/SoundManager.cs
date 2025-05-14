using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 예시
/// 배경음악: SoundManager.Instance.PlayBackgroundMusic("사운드 이름");
/// 효과음: SoundManager.Instance.PlaySFXForName("사운드 이름");
/// 효과음: SoundManager.Instance.PlaySFXForName("등록한 사운드 클립");
/// </summary>

[System.Serializable]
public class SoundEffectData
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [Header("SFX AudioSource Pool")]
    [SerializeField] private GameObject sfxAudioSourcePrefab;
    [SerializeField] private Transform sfxPoolRoot;

    private Queue<AudioSource> sfxAudioSourcePool = new Queue<AudioSource>();
    private int poolSize = 10;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Settings")]

    [Tooltip("마스터 볼륨")]
    [Range(0f, 1f)]
    public float masterVol = 1f;

    [Tooltip("효과음 볼륨")]
    [Range(0f, 1f)]
    public float sfxVol = 0.5f;

    [Tooltip("배경음악 볼륨")]
    [Range(0f, 1f)]
    public float backgroundMusicVol = 0.5f;

    [Header("오디오 클립")]
    public AudioClip backgroundMusic;
    public List<SoundEffectData> sfxList = new List<SoundEffectData>();
    private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();

    private void InitializeSFXDictionary()
    {
        foreach (var sfx in sfxList)
        {
            if (!soundEffects.ContainsKey(sfx.name))
            {
                soundEffects[sfx.name] = sfx.clip;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
        if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        // 볼륨 로드
        masterVol = PlayerPrefs.GetFloat("MasterVol", masterVol);
        backgroundMusicVol = PlayerPrefs.GetFloat("MusicVol", backgroundMusicVol);
        sfxVol = PlayerPrefs.GetFloat("SFXVol", sfxVol);

        // 볼륨 설정
        SetMasterVolume(masterVol);
        SetSFXVolume(sfxVol);
        SetMusicVolume(backgroundMusicVol);

        // 딕셔너리 초기화
        InitializeSFXDictionary();

        // 배경음악 재생
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(backgroundMusic);
        }
        else
        {
            Debug.LogWarning("배경음악이 할당되지 않았습니다.");
        }
    }
    private void Start()
    {
        InitializeAudioSourcePool();
    }

    private void InitializeAudioSourcePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(sfxAudioSourcePrefab, sfxPoolRoot);
            AudioSource source = go.GetComponent<AudioSource>();
            source.playOnAwake = false;
            go.SetActive(false);
            sfxAudioSourcePool.Enqueue(source);
        }
    }


    private void OnEnable()
    {
        // 슬라이더 값 초기화
        SetMasterVolume(masterVol);
        SetSFXVolume(sfxVol);
        SetMusicVolume(backgroundMusicVol);
    }

    #region 배경음악 관련
    public void PlayBackgroundMusic(AudioClip music)
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


    public void PlaySFXForName(string soundName, Vector3 position)
    {
        if (soundEffects.TryGetValue(soundName, out AudioClip clip))
        {
            PlaySFX(clip, position);
        }
        else
        {
            // Resources 폴더에서 로드 시도
            clip = Resources.Load<AudioClip>("Audio/SFX/" + soundName);

            if (clip != null)
            {
                soundEffects[soundName] = clip;  // 캐싱
                PlaySFX(clip, position);
            }
            else
            {
                Debug.LogWarning($"효과음 '{soundName}'을(를) 찾을 수 없습니다.");
            }
        }
    }
    public void PlaySFXForClip(AudioClip clip, Vector3 position)
    {
        PlaySFX(clip, position);
    }

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        if (clip == null)
        {
            Debug.LogWarning("SFX클립 null");
            return;
        }

        AudioSource source = GetPooledAudioSource();
        if (source == null)
        {
            Debug.LogWarning("AudioSource없음");
            return;
        }

        source.transform.position = position;
        float volume = sfxVol * masterVol;
        StartCoroutine(PlayAndReturn(source, clip, volume));
    }

    private System.Collections.IEnumerator PlayAndReturn(AudioSource source, AudioClip clip, float volume)
    {
        source.clip = clip;
        source.volume = volume;
        source.gameObject.SetActive(true);
        source.Play();

        yield return new WaitForSeconds(clip.length);

        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        sfxAudioSourcePool.Enqueue(source);
    }
    private AudioSource GetPooledAudioSource()
{
    if (sfxAudioSourcePool.Count > 0)
    {
        return sfxAudioSourcePool.Dequeue();
    }
    else //audiosource가 부족하면 새로 생성
    {
        GameObject go = Instantiate(sfxAudioSourcePrefab, sfxPoolRoot);
        AudioSource source = go.GetComponent<AudioSource>();
        source.playOnAwake = false;
        go.SetActive(false);
        return source;
    }
}

    #endregion

    #region 볼륨
    public void SetMasterVolume(float volume)
    {
        masterVol = Mathf.Clamp(volume, 0f, 1f);
        PlayerPrefs.SetFloat("MasterVol", masterVol);
        sfxSource.volume = sfxVol * masterVol;
        musicSource.volume = backgroundMusicVol * masterVol;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVol = Mathf.Clamp(volume, 0f, 1f);
        PlayerPrefs.SetFloat("SFXVol", sfxVol);
        sfxSource.volume = sfxVol * masterVol; // masterVol도 반영
    }

    public void SetMusicVolume(float volume)
    {
        backgroundMusicVol = Mathf.Clamp(volume, 0f, 1f);
        PlayerPrefs.SetFloat("MusicVol", backgroundMusicVol);
        musicSource.volume = backgroundMusicVol * masterVol; // masterVol도 반영
    }
    #endregion
}
