using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _current;

    [Header("环境声音")] public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("FX音效")] public AudioClip deathFXClip;
    
    [Header("Robbie音效")] public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip deathClip;
    public AudioClip jumpVoiceClip;
    public AudioClip deathVoiceClip;
    

    private AudioSource m_AmbientSource;
    private AudioSource m_MusicSource;
    private AudioSource m_FxSource;
    private AudioSource m_PlayerSource;
    private AudioSource m_VoiceSource;

    private void Awake()
    {
        _current = this;
        DontDestroyOnLoad(gameObject);
        m_AmbientSource = gameObject.AddComponent<AudioSource>();
        m_MusicSource = gameObject.AddComponent<AudioSource>();
        m_FxSource = gameObject.AddComponent<AudioSource>();
        m_PlayerSource = gameObject.AddComponent<AudioSource>();
        m_VoiceSource = gameObject.AddComponent<AudioSource>();
        
        StartLevelAudio();
    }

    private void StartLevelAudio()
    {
        _current.m_AmbientSource.clip = _current.ambientClip;
        _current.m_AmbientSource.loop = true;
        _current.m_AmbientSource.Play();

        _current.m_MusicSource.clip = _current.musicClip;
        _current.m_MusicSource.loop = true;
        _current.m_MusicSource.Play();
        
    }
    
    public static void PlayFootStepAudio()
    {
        var index = Random.Range(0, _current.walkStepClips.Length);
        _current.m_PlayerSource.clip = _current.walkStepClips[index];
        _current.m_PlayerSource.Play();
    }

    public static void PlayCrouchFootStepAudio()
    {
        var index = Random.Range(0, _current.crouchStepClips.Length);
        _current.m_PlayerSource.clip = _current.crouchStepClips[index];
        _current.m_PlayerSource.Play();
    }

    public static void PlayJumpAudio()
    {
        _current.m_PlayerSource.clip = _current.jumpClip;
        _current.m_PlayerSource.Play();

        _current.m_VoiceSource.clip = _current.jumpVoiceClip;
        _current.m_VoiceSource.Play();
    }

    public static void PlayDeathAudio()
    {
        _current.m_PlayerSource.clip = _current.deathClip;
        _current.m_PlayerSource.Play();

        _current.m_VoiceSource.clip = _current.deathVoiceClip;
        _current.m_VoiceSource.Play();

        _current.m_FxSource.clip = _current.deathFXClip;
        _current.m_FxSource.Play();
    }
}
