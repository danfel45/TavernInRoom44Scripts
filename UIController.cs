using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
   // [SerializeField] private GameObject menuUI;
    
           
    [SerializeField] private float transitionDuration = 0.3f; 
    [SerializeField] private TextMeshProUGUI resultText;      
    [SerializeField] private float displayDuration = 2f;      

    [SerializeField] private bool mouseOver = false;

    private PlayerControls playerControls;
    private Queue<string> resultQueue = new Queue<string>();
    private bool isDisplayingResult = false;

    [SerializeField] private AudioClip popSoundEfect;
    [SerializeField] private AudioSource audioSource;

  

    private void Awake()
    {
        playerControls = new PlayerControls();
        

    }


    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

   

    

    public void DisplayResult(string side)
    {
        resultQueue.Enqueue(side);
        if (!isDisplayingResult)
        {
            StartCoroutine(ProcessResultQueue());
        }
    }

    private IEnumerator ProcessResultQueue()
    {
        isDisplayingResult = true;

        while (resultQueue.Count > 0)
        {
            string side = resultQueue.Dequeue();
            yield return DisplayResultCoroutine(side);
        }

        isDisplayingResult = false;
    }

    private IEnumerator DisplayResultCoroutine(string side)
    {
        float elapsedTime = 0f;
        float startOpacity = 0f;
        float targetOpacity = 1f;

        resultText.text = side;

        
        while (elapsedTime < transitionDuration)
        {
            resultText.color = new Color(resultText.color.r, resultText.color.g, resultText.color.b, Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / transitionDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        resultText.color = new Color(resultText.color.r, resultText.color.g, resultText.color.b, targetOpacity);

       
        yield return new WaitForSeconds(displayDuration);

       
        elapsedTime = 0f;
        startOpacity = 1f;
        targetOpacity = 0f;

        while (elapsedTime < transitionDuration)
        {
            resultText.color = new Color(resultText.color.r, resultText.color.g, resultText.color.b, Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / transitionDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        resultText.color = new Color(resultText.color.r, resultText.color.g, resultText.color.b, targetOpacity);
    }

    public void PlayClickUISound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);  
        audioSource.PlayOneShot(popSoundEfect);
    }
}
