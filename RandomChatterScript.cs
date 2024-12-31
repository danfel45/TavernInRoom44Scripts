using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomChatterScript : MonoBehaviour
{
    [SerializeField] private List<AudioClip> soundObjects;  
    [SerializeField] private AudioSource soundSource;       
    [SerializeField] private Slider crowdNoiseSlider;       

    [SerializeField] private int[] times;                   
    [SerializeField] private List<Transform> positions;     

    private int count = 0;  
    private float timer = 0f;  

    private void Awake()
    {
        
        times = new int[soundObjects.Count];

       
        for (int i = 0; i < soundObjects.Count; i++)
        {
            times[i] = Random.Range(0, 300); 
        }

       
        Shuffle(soundObjects);

        
        SetRandomPosition();

      
        if (crowdNoiseSlider != null)
        {
            crowdNoiseSlider.onValueChanged.AddListener(SetVolume);
        }

        soundSource.volume = 0.2f;
    }

   
    void Update()
    {
        
        timer += Time.deltaTime;

       
        if (count < times.Length && timer >= times[count])
        {
           
            if (!soundSource.isPlaying && soundObjects.Count > 0)
            {
              
                soundSource.clip = soundObjects[0]; 
                soundSource.Play();

              
                soundObjects.RemoveAt(0);

               
                List<int> timesList = new List<int>(times);
                timesList.RemoveAt(count);
                times = timesList.ToArray();

                
                timer = 0f;

                
                if (soundObjects.Count == 0)
                {
                    //Debug.Log("All sounds have been played.");
                    
                    enabled = false;
                    return;
                }
            }
        }

       
        if (soundSource.clip != null && !soundSource.isPlaying)
        {
           
            soundSource.clip = null;

           
            SetRandomPosition();
        }
    }

   
    private void SetRandomPosition()
    {
        if (positions.Count > 0)
        {
            int randomIndex = Random.Range(0, positions.Count);
            soundSource.transform.position = positions[randomIndex].position;
        }
    }

   
    private void SetVolume(float volume)
    {
        soundSource.volume = volume;
    }

   
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
