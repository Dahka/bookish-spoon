using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip cardFlipAudioClip;
    [SerializeField] AudioClip cardMatchAudioClip;
    [SerializeField] AudioClip cardMissAudioClip;
    [SerializeField] AudioClip victoryAudioClip;

    public enum AudioType { CardFlip, CardMatch, CardMiss, Victory }

    //Function to play the four types of Audio
    public void PlayAudio(AudioType audioType)
    {
        AudioClip clip = null;
        switch(audioType)
        {
            case AudioType.CardFlip:
                clip = cardFlipAudioClip; break;
            case AudioType.CardMatch:
                clip = cardMatchAudioClip; break;
            case AudioType.CardMiss:
                clip = cardMissAudioClip; break;
            case AudioType.Victory:
                clip = victoryAudioClip; break;
        }
        if(clip == null)
        {
            Debug.LogError($"Could not find audio clip for type: {audioType}");
            return;
        }
        audioSource.PlayOneShot(clip);
    }

}
