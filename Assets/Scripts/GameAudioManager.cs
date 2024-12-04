using UnityEngine;
using System.Collections.Generic;

public class GameAudioManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        [HideInInspector]
        public AudioSource source;
    }

    public static GameAudioManager Instance { get; private set; }

    [Header("背景音乐设置")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    public bool playMusicOnStart = true;
    public float fadeInDuration = 2f;

    [Header("音效设置")]
    public SoundEffect[] soundEffects;
    [Range(0f, 1f)]
    public float soundEffectsVolume = 1f;

    private AudioSource musicSource;
    private Dictionary<string, AudioSource> soundSources = new Dictionary<string, AudioSource>();

    void Awake()
    {
        // 单例模式设置
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeAudio()
    {
        // 设置背景音乐
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.volume = 0; // 从0开始，用于淡入效果
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        // 设置音效
        foreach (var sound in soundEffects)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume * soundEffectsVolume;
            source.pitch = sound.pitch;
            source.playOnAwake = false;
            sound.source = source;
            soundSources[sound.name] = source;
        }
    }

    void Start()
    {
        if (playMusicOnStart)
        {
            StartBackgroundMusic();
        }
    }

    public void StartBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.Play();
            StartCoroutine(FadeInMusic());
        }
    }

    private System.Collections.IEnumerator FadeInMusic()
    {
        float elapsed = 0;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, musicVolume, elapsed / fadeInDuration);
            yield return null;
        }
        musicSource.volume = musicVolume;
    }

    public void PlaySound(string name)
    {
        if (soundSources.ContainsKey(name))
        {
            soundSources[name].Play();
        }
        else
        {
            Debug.LogWarning($"Sound {name} not found!");
        }
    }

    public void StopSound(string name)
    {
        if (soundSources.ContainsKey(name))
        {
            soundSources[name].Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = Mathf.Clamp01(volume);
        foreach (var source in soundSources.Values)
        {
            source.volume = source.volume * soundEffectsVolume;
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }
}