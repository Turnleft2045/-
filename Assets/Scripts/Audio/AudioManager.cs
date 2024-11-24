using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [Header("事件jianting")]
    public PlayAuidoEventSO FXEvent;
    public PlayAuidoEventSO BGMEvet;
    public FloatEventSO volumeEvent;
    public VoidEventSO pauseEvent;

    [Header("广播")]
    public FloatEventSO syncVolumEvent;
    [Header("组件")]
    public AudioSource BGMSource;
    public AudioSource FXSource;
    public AudioMixer mixer;

    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvet.OnEventRaised += OnBGMEvent;
        volumeEvent.OnEventRaised += OnvolumeEvent;
        pauseEvent.OnEventRaised += OnpauseEvent;
    }
    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvet.OnEventRaised -= OnBGMEvent;
        volumeEvent.OnEventRaised -= OnvolumeEvent;
        pauseEvent.OnEventRaised -= OnpauseEvent;
    }
    private void OnpauseEvent()
    {
        float amout;
        mixer.GetFloat("MasterVolume",out amout);
        syncVolumEvent.RaiseEvent(amout);
    }
    private void OnvolumeEvent(float amount)
    {
        mixer.SetFloat("MasterVolume", amount*100-80);
    }

    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
    private void OnBGMEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }

}
