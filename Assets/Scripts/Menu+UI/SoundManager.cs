using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NamedSound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    // SoundManager.Instance.PlayMusic("name");
    public AudioSource sfxSource;
    // SoundManager.Instance.Play("name");
    public AudioSource sfxLoopSource;
    // SoundManager.Instance.PlaySFXLoop("name");
    // SoundManager.Instance.StopSFXLoop();

    [Header("Sound Library")]
    public List<NamedSound> sounds = new List<NamedSound>();
    private Dictionary<string, NamedSound> soundLookup;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            soundLookup = new Dictionary<string, NamedSound>();
            foreach (var sound in sounds)
            {
                if (!soundLookup.ContainsKey(sound.name))
                    soundLookup.Add(sound.name, sound);
            }
            PlayMusic("MainMenu");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Play(string soundName)
    {
        if (soundLookup.TryGetValue(soundName, out NamedSound sound))
        {
            sfxSource.PlayOneShot(sound.clip);
        }
        else
        {
            Debug.LogWarning("SoundManager: Sound not found: " + soundName);
        }
    }
    public void PlayMusic(string soundName)
    {
        if (soundLookup.TryGetValue(soundName, out NamedSound sound))
        {
            StartCoroutine(FadeToNewMusic(sound));
        }
        else
        {
            Debug.LogWarning("SoundManager: Music not found: " + soundName);
        }
    }

    private IEnumerator FadeToNewMusic(NamedSound newSound)
    {
        yield return StartCoroutine(FadeOutMusic());

        musicSource.clip = newSound.clip;
        musicSource.Play();

        yield return StartCoroutine(FadeInMusic(1f));
    }

    private IEnumerator FadeOutMusic()
    {
        float startVolume = musicSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = 0;
    }

    private IEnumerator FadeInMusic(float targetVolume)
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
    public void PlaySFXLoop(string soundName)
    {
        if (!soundLookup.TryGetValue(soundName, out NamedSound sound))
        {
            Debug.LogWarning("SoundManager: Sound not found: " + soundName);
            return;
        }

        sfxLoopSource.clip = sound.clip;
        sfxLoopSource.Play();
    }

    public void StopSFXLoop()
    {
        if (sfxLoopSource.isPlaying)
            sfxLoopSource.Stop();
    }
}
