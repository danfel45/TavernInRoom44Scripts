using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering.Universal;

public class DiceEnabler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI d4Text;
    [SerializeField] private TextMeshProUGUI d6Text;
    [SerializeField] private TextMeshProUGUI d8Text;
    [SerializeField] private TextMeshProUGUI d10Text;
    [SerializeField] private TextMeshProUGUI d12Text;
    [SerializeField] private TextMeshProUGUI d20Text;
    [SerializeField] private TextMeshProUGUI customGroupText;

    [SerializeField] private Color originalColor;
    [SerializeField] private Color selectedColor;

    [SerializeField] private GameObject d4Prefab;
    [SerializeField] private GameObject d6Prefab;
    [SerializeField] private GameObject d8Prefab;
    [SerializeField] private GameObject d10Prefab;
    [SerializeField] private GameObject d12Prefab;
    [SerializeField] private GameObject d20Prefab;

    [SerializeField] private Button increaseD4Button;
    [SerializeField] private Button decreaseD4Button;
    [SerializeField] private Button increaseD6Button;
    [SerializeField] private Button decreaseD6Button;
    [SerializeField] private Button increaseD8Button;
    [SerializeField] private Button decreaseD8Button;
    [SerializeField] private Button increaseD10Button;
    [SerializeField] private Button decreaseD10Button;
    [SerializeField] private Button increaseD12Button;
    [SerializeField] private Button decreaseD12Button;
    [SerializeField] private Button increaseD20Button;
    [SerializeField] private Button decreaseD20Button;

    [SerializeField] private GameObject[] destroyParticlePrefabs;

    [SerializeField] private bool d4Enabled = true;
    [SerializeField] private bool d6Enabled = true;
    [SerializeField] private bool d8Enabled = true;
    [SerializeField] private bool d10Enabled = true;
    [SerializeField] private bool d12Enabled = true;
    [SerializeField] private bool d20Enabled = true;

    [SerializeField] private PickUpDiceScript pud;

    private const int MaxDiceCount = 4;

    private List<GameObject> d4Dice = new List<GameObject>();
    private List<GameObject> d6Dice = new List<GameObject>();
    private List<GameObject> d8Dice = new List<GameObject>();
    private List<GameObject> d10Dice = new List<GameObject>();
    private List<GameObject> d12Dice = new List<GameObject>();
    private List<GameObject> d20Dice = new List<GameObject>();

    [SerializeField] private List<GameObject> customDiceGroup = new List<GameObject>();


    [SerializeField] private CanvasGroup enablersUICanvasGroup;
    [SerializeField] private float lowOpacity = 0.2f;
    [SerializeField] private float fullOpacity = 1f;
    [SerializeField] private float transitionDuration = 0.3f;
    [SerializeField] private bool mouseOver;

    [SerializeField] private AudioClip popSoundEfect;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private int d4total;
    [SerializeField] private int d6total;
    [SerializeField] private int d8total;
    [SerializeField] private int d10total;
    [SerializeField] private int d12total;
    [SerializeField] private int d20total;
    [SerializeField] private int customGroupTotal;


    [SerializeField] private UIController uiController;

    private Coroutine flashCoroutine;

    [SerializeField] private bool customGroupFlash;
    private bool canAddToGroup;

    

    private void Start()
    {
        if (d4Text != null)
        {
            originalColor = d4Text.color;
        }

        UpdateButtonInteractivity();
    }

    private void Awake()
    {
        d4Enabled = true;
        d6Enabled = true;
        d8Enabled = true;
        d10Enabled = true;
        d12Enabled = true;
        d20Enabled = true;
        canAddToGroup= true;

        d4Dice.Add(GameObject.Find("D4"));
        d6Dice.Add(GameObject.Find("D6"));
        d8Dice.Add(GameObject.Find("D8"));
        d10Dice.Add(GameObject.Find("D10"));
        d12Dice.Add(GameObject.Find("D12"));
        d20Dice.Add(GameObject.Find("D20"));
    }

    private void Update()
    {
       

        if(customDiceGroup.Count == 0)
        {
            customGroupFlash = false;
        }
        

        if (customGroupFlash)
        {
            customGroupText.GetComponent<Button>().interactable = true;
           
        }
        else
        {
            customGroupText.GetComponent<Button>().interactable = false;
           
        }

    }

    
    

    
    public void AddToCustomGroup(GameObject die)
    {
       
        if(!customDiceGroup.Contains(die))
        {
            if(canAddToGroup)
            {
                customDiceGroup.Add(die);
                customGroupFlash = true;
                die.layer = 6;
            }
            
        }
        else
        {
            customDiceGroup.Remove(die);
            die.layer = 0;
        }

    }

    
    public void IncreaseD4()
    {
        if (d4Dice.Count < MaxDiceCount)
        {
            d4Text.color = originalColor;
            d4Dice.Add(PickUpDice(d4Prefab));
            d4Enabled = true;
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void DecreaseD4()
    {
        if (d4Dice.Count > 0)
        {
            DestroyDice(d4Dice[d4Dice.Count - 1]);
            d4Dice.RemoveAt(d4Dice.Count - 1);
            if (d4Dice.Count == 0)
            {
                d4Text.color = selectedColor;
                d4Enabled = false;
            }
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void IncreaseD6()
    {
        if (d6Dice.Count < MaxDiceCount)
        {
            d6Text.color = originalColor;
            d6Dice.Add(PickUpDice(d6Prefab));
            d6Enabled = true;
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void DecreaseD6()
    {
        if (d6Dice.Count > 0)
        {
            DestroyDice(d6Dice[d6Dice.Count - 1]);
            d6Dice.RemoveAt(d6Dice.Count - 1);
            if (d6Dice.Count == 0)
            {
                d6Text.color = selectedColor;
                d6Enabled = false;
            }
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void IncreaseD8()
    {
        if (d8Dice.Count < MaxDiceCount)
        {
            d8Text.color = originalColor;
            d8Dice.Add(PickUpDice(d8Prefab));
            d8Enabled = true;
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void DecreaseD8()
    {
        if (d8Dice.Count > 0)
        {
            DestroyDice(d8Dice[d8Dice.Count - 1]);
            d8Dice.RemoveAt(d8Dice.Count - 1);
            if (d8Dice.Count == 0)
            {
                d8Text.color = selectedColor;
                d8Enabled = false;
            }
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void IncreaseD10()
    {
        if (d10Dice.Count < MaxDiceCount)
        {
            d10Text.color = originalColor;
            d10Dice.Add(PickUpDice(d10Prefab));
            d10Enabled = true;
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void DecreaseD10()
    {
        if (d10Dice.Count > 0)
        {
            DestroyDice(d10Dice[d10Dice.Count - 1]);
            d10Dice.RemoveAt(d10Dice.Count - 1);
            if (d10Dice.Count == 0)
            {
                d10Text.color = selectedColor;
                d10Enabled = false;
            }
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void IncreaseD12()
    {
        if (d12Dice.Count < MaxDiceCount)
        {
            d12Text.color = originalColor;
            d12Dice.Add(PickUpDice(d12Prefab));
            d12Enabled = true;
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void DecreaseD12()
    {
        if (d12Dice.Count > 0)
        {
            DestroyDice(d12Dice[d12Dice.Count - 1]);
            d12Dice.RemoveAt(d12Dice.Count - 1);
            if (d12Dice.Count == 0)
            {
                d12Text.color = selectedColor;
                d12Enabled = false;
            }
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void IncreaseD20()
    {
        if (d20Dice.Count < MaxDiceCount)
        {
            d20Text.color = originalColor;
            d20Dice.Add(PickUpDice(d20Prefab));
            d20Enabled = true;
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

    public void DecreaseD20()
    {
        if (d20Dice.Count > 0)
        {
            DestroyDice(d20Dice[d20Dice.Count - 1]);
            d20Dice.RemoveAt(d20Dice.Count - 1);
            if (d20Dice.Count == 0)
            {
                d20Text.color = selectedColor;
                d20Enabled = false;
            }
            UpdateButtonInteractivity();
            PlayClickUISound();
        }
    }

   

    private GameObject PickUpDice(GameObject diePrefab)
    {
        GameObject temp = Instantiate(diePrefab, pud.diceHolder.position, Quaternion.identity);
        pud.PickUpDice(temp);
        return temp;
    }

    private void DestroyDice(GameObject die)
    {
        if (die != null)
        {
            Vector3 diePosition = die.transform.position;
            Destroy(die);

            int randomIndex = Random.Range(0, destroyParticlePrefabs.Length);
            GameObject particleSystem = Instantiate(destroyParticlePrefabs[randomIndex], diePosition, Quaternion.identity);
            ParticleSystem ps = particleSystem.GetComponent<ParticleSystem>();
            AudioSource audioSource = particleSystem.GetComponent<AudioSource>();
            audioSource.pitch = Random.Range(0.8f, 1.2f);

            if (ps != null)
            {
                Destroy(particleSystem, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(particleSystem, 5f);
            }
        }
    }

    private void UpdateButtonInteractivity()
    {
        increaseD4Button.interactable = d4Dice.Count < MaxDiceCount;
        decreaseD4Button.interactable = d4Dice.Count > 0;

        increaseD6Button.interactable = d6Dice.Count < MaxDiceCount;
        decreaseD6Button.interactable = d6Dice.Count > 0;

        increaseD8Button.interactable = d8Dice.Count < MaxDiceCount;
        decreaseD8Button.interactable = d8Dice.Count > 0;

        increaseD10Button.interactable = d10Dice.Count < MaxDiceCount;
        decreaseD10Button.interactable = d10Dice.Count > 0;

        increaseD12Button.interactable = d12Dice.Count < MaxDiceCount;
        decreaseD12Button.interactable = d12Dice.Count > 0;

        increaseD20Button.interactable = d20Dice.Count < MaxDiceCount;
        decreaseD20Button.interactable = d20Dice.Count > 0;
    }

   /* public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }*/

    public void PlayClickUISound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);  
        audioSource.PlayOneShot(popSoundEfect);
    }

    private void RollDiceGroup(Dice die, System.Action<int> updateTotalCallback)
    {
      
        Vector3 upwardForce = Vector3.up * Random.Range(die.throwStrengthMin, die.throwStrengthMax);

       
        float sideForceX = Random.Range(-die.throwStrengthMax / 2, die.throwStrengthMax / 2);
        float sideForceZ = Random.Range(-die.throwStrengthMax / 2, die.throwStrengthMax / 2);
        Vector3 sidewaysForce = new Vector3(sideForceX, 0, sideForceZ);

       
        Vector3 totalForce = upwardForce + sidewaysForce;

        
        die.rb.AddForce(totalForce, ForceMode.Impulse);

        die.rb.AddTorque(transform.forward * Random.Range(die.torqueMin, die.torqueMax) +
                         transform.up * Random.Range(die.torqueMin, die.torqueMax) +
                         transform.right * Random.Range(die.torqueMin, die.torqueMax));

        StartCoroutine(CheckIfDiceHasStoppedForGroup(die, updateTotalCallback));

    }

    IEnumerator CheckIfDiceHasStoppedForGroup(Dice die, System.Action<int> updateTotalCallback)
    {
        yield return new WaitForSeconds(1f); 

        while (die.rb.velocity.magnitude > 0.1f || die.rb.angularVelocity.magnitude > 0.1f)
        {
            yield return new WaitForSeconds(0.1f);

        }



        int topFace = GetNumberOnTopFaceForGroup(die) + 1;
        updateTotalCallback(topFace); 
       
    }

    IEnumerator DisplayTotal(List<GameObject> diceList, int side)
    {
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject die in diceList)
        {
            while (die.GetComponent<Dice>().rb.velocity.magnitude > 0.1f || die.GetComponent<Dice>().rb.angularVelocity.magnitude > 0.1f)
            {
                Debug.Log("Moving");
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Done");
        switch(side)
        {
            case 1:
                uiController.DisplayResult(customGroupTotal.ToString());
                customDiceGroup.Clear();
                canAddToGroup = true;
                break;
            case 4:
                uiController.DisplayResult(d4total.ToString());
                break;
            case 6:
                uiController.DisplayResult(d6total.ToString());
                break;
            case 8:
                uiController.DisplayResult(d8total.ToString());
                break;
            case 10:
                uiController.DisplayResult(d10total.ToString());
                break;
            case 12:
                uiController.DisplayResult(d12total.ToString());
                break;
            case 20:
                uiController.DisplayResult(d20total.ToString());
                break;
        }

    }

    private int GetNumberOnTopFaceForGroup(Dice die)
    {
        if (die.diceFaces == null) return 0;

        int topFace = 0;
        float lastYPosition = die.diceFaces[0].position.y;

        for (int i = 0; i < die.diceFaces.Length; i++)
        {
            if (die.diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = die.diceFaces[i].position.y;
                topFace = i;
            }
        }

        return topFace;
    }

    public void CustomGroupRoll()
    {
        customGroupTotal = 0;
        canAddToGroup = false;

        customGroupFlash = false;
        foreach (GameObject die in customDiceGroup)
        {

            if (die.GetComponent<Dice>().rb.velocity.magnitude != 0)
            {
                return;
            }
        }
        foreach (GameObject die in customDiceGroup)
        {
            die.gameObject.layer = 0;
            RollDiceGroup(die.GetComponent<Dice>(), topFace => customGroupTotal += topFace);
        }
        StartCoroutine(DisplayTotal(customDiceGroup, 1));
        
    }

    public void RollAllD4()
    {
        d4total = 0;

        foreach (GameObject die in d4Dice)
        {
            if (die.GetComponent<Dice>().rb.velocity.magnitude != 0)
            {
                return;
            }
        }
        foreach (GameObject die in d4Dice)
        {
            RollDiceGroup(die.GetComponent<Dice>(), topFace => d4total += topFace);
        }
        StartCoroutine(DisplayTotal(d4Dice, 4));

    }

    public void RollAllD6()
    {
        d6total = 0;
        foreach (GameObject die in d6Dice)
        {
            if (die.GetComponent<Dice>().rb.velocity.magnitude != 0)
            {
                return;
            }
        }
        foreach (GameObject die in d6Dice)
        {
            RollDiceGroup(die.GetComponent<Dice>(), topFace => d6total += topFace);
        }
        StartCoroutine(DisplayTotal(d6Dice, 6));
    }

    public void RollAllD8()
    {
        d8total = 0;
        foreach (GameObject die in d8Dice)
        {
            if (die.GetComponent<Dice>().rb.velocity.magnitude != 0)
            {
                return;
            }
        }
        foreach (GameObject die in d8Dice)
        {
            RollDiceGroup(die.GetComponent<Dice>(), topFace => d8total += topFace);
        }
        StartCoroutine(DisplayTotal(d8Dice, 8));
    }

    public void RollAllD10()
    {
        d10total = 0;
        foreach (GameObject die in d10Dice)
        {
            if (die.GetComponent<Dice>().rb.velocity.magnitude != 0)
            {
                return;
            }
        }
        foreach (GameObject die in d10Dice)
        {
            RollDiceGroup(die.GetComponent<Dice>(), topFace => d10total += topFace);
        }
        StartCoroutine(DisplayTotal(d10Dice, 10));
    }

    public void RollAllD12()
    {
        d12total = 0;
        foreach (GameObject die in d12Dice)
        {
            if (die.GetComponent<Dice>().rb.velocity.magnitude != 0)
            {
                return;
            }
        }
        foreach (GameObject die in d12Dice)
        {
            RollDiceGroup(die.GetComponent<Dice>(), topFace => d12total += topFace);
        }
        StartCoroutine(DisplayTotal(d12Dice, 12));
    }

    public void RollAllD20()
    {
        d20total = 0;
        foreach (GameObject die in d20Dice)
        {
            if (die.GetComponent<Dice>().rb.velocity.magnitude != 0)
            {
                return;
            }
        }
        foreach (GameObject die in d20Dice)
        {
            RollDiceGroup(die.GetComponent<Dice>(), topFace => d20total += topFace);
        }
        StartCoroutine(DisplayTotal(d20Dice, 20));
    }



}