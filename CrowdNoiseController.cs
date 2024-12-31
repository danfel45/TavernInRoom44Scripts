using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowdNoiseController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips; 
    [SerializeField] private AudioSource audioSource1;  
    [SerializeField] private AudioSource audioSource2;  
    [SerializeField] private AudioSource audioSource3;  
    [SerializeField] private Slider crowdNoiseSlider; 

    private void Start()
    {
       
        if (audioClips.Count > 0)
        {
            PlayRandomClipFromRandomPoint(audioSource1, "AudioSource1");
            PlayRandomClipFromRandomPoint(audioSource2, "AudioSource2");
            PlayRandomClipFromRandomPoint(audioSource3, "AudioSource3");
        }
        else
        {
            Debug.LogWarning("No audio clips assigned!");
        }

       
        if (crowdNoiseSlider != null)
        {
            crowdNoiseSlider.value = 0.3f;
            SetCrowdNoiseVolume(0.3f);
            crowdNoiseSlider.onValueChanged.AddListener(SetCrowdNoiseVolume);
        }
    }

    private void PlayRandomClipFromRandomPoint(AudioSource audioSource, string sourceName)
    {
        
        AudioClip randomClip = audioClips[Random.Range(0, audioClips.Count)];

        
        audioSource.clip = randomClip;

       
        float randomStartPoint = Random.Range(0f, randomClip.length);

       
        audioSource.time = randomStartPoint;
        audioSource.Play();

       
    }

   
    public void SetCrowdNoiseVolume(float volume)
    {
        audioSource1.volume = volume;
        audioSource2.volume = volume;
        audioSource3.volume = volume;
    }
}
