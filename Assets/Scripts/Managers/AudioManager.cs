using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    private readonly float[] VOLUMES =
    {
        0.2f,   // Music
        0.2f,   // MusicFX
        0.5f    // SoundFX
    };

    public enum Type
    {
        Music,
        MusicFX,
        SoundFX,
        Count
    }

    private readonly Transform transform = new GameObject(nameof(AudioManager), typeof(AudioListener)).transform;
    private readonly AudioSource[] audioSources = new AudioSource[(int)Type.Count];
    private readonly HashSet<string> keys = new();

    public void Initialize()
    {
        transform.SetParent(Managers.Instance.transform);

        var names = Enum.GetNames(typeof(Type));
        for (int i = 0; i < audioSources.Length; i++)
        {
            Transform child = new GameObject(names[i]).transform;
            child.SetParent(transform);

            AudioSource audioSource = child.gameObject.AddComponent<AudioSource>();
            audioSource.loop = (Type)i == Type.Music;
            audioSource.volume = VOLUMES[i];
            audioSources[i] = audioSource;
        }
    }

    public void Play(string key, float volumeScale = 1.0f)
    {
        switch (GetType(key))
        {
            case Type.Music:
                Play_Music(key, volumeScale);
                break;
            case Type.SoundFX:
                Play_SoundFX(key, volumeScale);
                break;
            default:
                Debug.LogWarning($"{Define.FAILED_TO_}{nameof(Play)}({key})");
                break;
        }
    }

    private void Play_Music(string key, float volumeScale = 1.0f)
    {
        AudioSource audioSource = audioSources[(int)Type.Music];
        if (audioSource.clip != null)
        {
            Play_MusicFX(audioSource.clip, audioSource.time, audioSource.volume);
        }

        Managers.Resource.LoadAssetAsync<AudioClip>(key, clip =>
        {
            audioSource.clip = clip;
            audioSource.Play();
            audioSource.DOFade(VOLUMES[(int)Type.Music] * volumeScale, 2.0f).From(0.0f);
        });
    }

    private void Play_MusicFX(AudioClip clip, float time, float volume)
    {
        AudioSource audioSource = audioSources[(int)Type.MusicFX];
        audioSource.clip = clip;
        audioSource.time = time;
        audioSource.Play();
        audioSource.DOFade(0.0f, 1.0f).From(volume).OnComplete(() =>
        {
            audioSource.Stop();

            string key = clip.name;
            Managers.Resource.Release(key);
        });
    }

    private void Play_SoundFX(string key, float volumeScale = 1.0f)
    {
        if (keys.Add(key) == false)
        {
            return;
        }

        Managers.Resource.LoadAssetAsync<AudioClip>(key, clip =>
        {
            audioSources[(int)Type.SoundFX].PlayOneShot(clip, volumeScale);
            DOVirtual.DelayedCall(Define.EVENT_INTERVAL, () => keys.Remove(key));
        });
    }

    private Type GetType(string key)
    {
        string type = key[..key.IndexOf('_')];
        return (Type)Enum.Parse(typeof(Type), type);
    }
}