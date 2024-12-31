using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] public float throwStrengthMin = 3f;
    [SerializeField] public float throwStrengthMax = 15f;
    [SerializeField] public float torqueMin = 0.1f;
    [SerializeField] public float torqueMax = 2f;
    [SerializeField] public Rigidbody rb;

    public AudioClip[] collisionSounds;
    private AudioSource audioSource;

    [SerializeField] public Transform[] diceFaces;

    [SerializeField] private UIController uiController;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

       
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.maxDistance = 300.0f;
        audioSource.minDistance = 5f;

        
        RandomizeRotation();

        uiController = GameObject.Find("TopCanvas").GetComponent<UIController>();
    }

    void RandomizeRotation()
    {
        if (!this.gameObject.CompareTag("Coin"))
        {
            transform.rotation = Random.rotation;
        }
        
    }

    public void RollDice()
    {
        if (rb.velocity == Vector3.zero)
        {
           
            Vector3 upwardForce = Vector3.up * Random.Range(throwStrengthMin, throwStrengthMax);

           
            float sideForceX = Random.Range(-throwStrengthMax / 2, throwStrengthMax / 2);
            float sideForceZ = Random.Range(-throwStrengthMax / 2, throwStrengthMax / 2);
            Vector3 sidewaysForce = new Vector3(sideForceX, 0, sideForceZ);

           
            Vector3 totalForce = upwardForce + sidewaysForce;

           
            rb.AddForce(totalForce, ForceMode.Impulse);

            rb.AddTorque(transform.forward * Random.Range(torqueMin, torqueMax) +
                         transform.up * Random.Range(torqueMin, torqueMax) +
                         transform.right * Random.Range(torqueMin, torqueMax));

            StartCoroutine(CheckIfDiceHasStopped());
        }
    }

    IEnumerator CheckIfDiceHasStopped()
    {
        yield return new WaitForSeconds(1f); 

        while (rb.velocity.magnitude > 0.1f || rb.angularVelocity.magnitude > 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }

        GetNumberOnTopFace();
    }

    void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.CompareTag("Table") || collision.gameObject.CompareTag("Die") || collision.gameObject.CompareTag("Coin"))
        {
           
            if (collisionSounds.Length > 0)
            {
               
                int randomIndex = Random.Range(0, collisionSounds.Length);
                audioSource.clip = collisionSounds[randomIndex];
            }

           
            audioSource.volume = Random.Range(0.8f, 1.2f);
            audioSource.pitch = Random.Range(0.8f, 1.2f);

          
            audioSource.Play();
        }
    }

    private string GetNumberOnTopFace()
    {
        if (diceFaces == null) return null;

        int topFace = 0;
        float lastYPosition = diceFaces[0].position.y;

        for (int i = 0; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }

      
        string topFaceName = diceFaces[topFace].gameObject.name;
        uiController.DisplayResult(topFaceName);
        return topFaceName;
    }
}
