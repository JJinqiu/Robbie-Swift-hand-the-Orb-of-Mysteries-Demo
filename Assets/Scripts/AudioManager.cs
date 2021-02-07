using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _current;

    [Header("环境声音")] public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("FX音效")] public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    public AudioClip doorFXClip;
    public AudioClip startLevelClip;
    public AudioClip winClip;

    [Header("Robbie音效")] public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip deathClip;
    public AudioClip jumpVoiceClip;
    public AudioClip deathVoiceClip;
    public AudioClip orbVoiceClip;


    private AudioSource m_AmbientSource;
    private AudioSource m_MusicSource;
    private AudioSource m_FxSource;
    private AudioSource m_PlayerSource;
    private AudioSource m_VoiceSource;

    public AudioMixerGroup ambientGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup fXGroup;
    public AudioMixerGroup playerGroup;
    public AudioMixerGroup voiceGroup;

    private void Awake()
    {
        if (_current != null)
        {
            Destroy(gameObject);
            return;
        }

        _current = this;
        DontDestroyOnLoad(gameObject);
        m_AmbientSource = gameObject.AddComponent<AudioSource>();
        m_MusicSource = gameObject.AddComponent<AudioSource>();
        m_FxSource = gameObject.AddComponent<AudioSource>();
        m_PlayerSource = gameObject.AddComponent<AudioSource>();
        m_VoiceSource = gameObject.AddComponent<AudioSource>();

        m_AmbientSource.outputAudioMixerGroup = ambientGroup;
        m_MusicSource.outputAudioMixerGroup = musicGroup;
        m_FxSource.outputAudioMixerGroup = fXGroup;
        m_PlayerSource.outputAudioMixerGroup = playerGroup;
        m_VoiceSource.outputAudioMixerGroup = voiceGroup;
        
        
        StartLevelAudio();
    }

    private void StartLevelAudio()
    {
        m_AmbientSource.clip = _current.ambientClip;
        m_AmbientSource.loop = true;
        m_AmbientSource.Play();

        m_MusicSource.clip = _current.musicClip;
        m_MusicSource.loop = true;
        m_MusicSource.Play();

        m_FxSource.clip = startLevelClip;
        m_FxSource.Play();
    }

    public static void PlayerWonAudio()
    {
        _current.m_FxSource.clip = _current.winClip;
        _current.m_FxSource.Play();
        _current.m_PlayerSource.Stop();
    }

    public static void PlayDoorOpenAudio()
    {
        _current.m_FxSource.clip = _current.doorFXClip;
        _current.m_FxSource.PlayDelayed(1f);
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

    public static void PlayOrbAudio()
    {
        _current.m_FxSource.clip = _current.orbFXClip;
        _current.m_FxSource.Play();

        _current.m_VoiceSource.clip = _current.orbVoiceClip;
        _current.m_VoiceSource.Play();
    }
}