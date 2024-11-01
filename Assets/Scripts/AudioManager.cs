using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip[] DiceMoveClips;
    public AudioClip[] DiceSlideClips;
    public AudioClip[] PlateClips;
    public AudioClip[] DoorClips;
    public AudioClip[] ObjectClips;


    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
    }

    private void PickRandomPitch(float volume)
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.volume = volume;
    }
    private void PickRandomClip(AudioClip[] clips)
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
    }

    private void PlayRandomClip(AudioClip[] clips, float volume, bool delay = false, float delayTime = 0f)
    {
        PickRandomPitch(volume);
        PickRandomClip(clips);
        if(delay)
        {
            audioSource.PlayDelayed(delayTime);
        }
        else
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
    private void PlayRandomClip(AudioClip clip, float volume, bool delay = false, float delayTime = 0f)
    {
        PickRandomPitch(volume);
        if(delay)
        {
            audioSource.PlayDelayed(delayTime);
        }
        else
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayDiceMoveSFX(float volume = 0.3f, bool delay = false, float delayTime = 0f) => PlayRandomClip(DiceMoveClips, volume, delay, delayTime);
    public void PlayDiceSlideSFX(float volume = 0.3f, bool delay = false, float delayTime = 0f) => PlayRandomClip(DiceSlideClips, volume, delay, delayTime);
    public void PlayPlateSFX(float volume = 0.3f, bool delay = false, float delayTime = 0f) => PlayRandomClip(PlateClips, volume, delay, delayTime);
    public void PlayDoorSFX(float volume = 0.3f, bool delay = false, float delayTime = 0f) => PlayRandomClip(DoorClips, volume, delay, delayTime);
    public void PlayObjectSpawnSFX(float volume = 0.3f, bool delay = false, float delayTime = 0f) => PlayRandomClip(ObjectClips, volume, delay, delayTime);
}
