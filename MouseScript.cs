using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseScript : MonoBehaviour
{
    private PlayerControls playerControls;
    private RaycastHit hit;
    private GameObject pickedObject;
    private Vector3 initialPickUpPosition;

    [SerializeField] private Texture2D idleCursor;
    //[SerializeField] private Texture2D hoverCursor;
    [SerializeField] private List<GameObject> dieList;
    [SerializeField] private GameObject activeDie;
    [SerializeField] private bool held;
    [SerializeField] private float pickUpHeightOffset = 0.5f; 

    [SerializeField] private bool shift;

    [SerializeField] private DiceEnabler diceEnabler;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Dice.Roll.performed += Click;
        playerControls.Dice.Hold.performed += Hold;
        playerControls.Dice.Shift.performed += Shift;

        shift = false;
    }

    private void Update()
    {

      
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
           
        }

        if (held && pickedObject != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(pickedObject.transform.position).z;
            Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            objectPosition.y = initialPickUpPosition.y; 
            pickedObject.transform.position = objectPosition;
        }
    }

    private void Click(InputAction.CallbackContext context)
    {
        if (!shift)
        {
            if (hit.collider.tag == "Die" || hit.collider.tag == "Coin")
            {
                RollDie(hit.collider.gameObject);
                activeDie = hit.collider.gameObject;
            }
        }
        else
        {
            if (hit.collider.tag == "Die")
            {
                diceEnabler.AddToCustomGroup(hit.collider.gameObject);
            }
        }

    }


    private void Shift(InputAction.CallbackContext context)
    {
        if (!shift)
        {
            shift = true;
        }
        else
        {
            shift = false;
        }
    }

    private void Hold(InputAction.CallbackContext context)
    {
        if (!held)
        {
            if (hit.collider != null && (hit.collider.tag == "Die" || hit.collider.tag == "Coin"))
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null && rb.velocity == Vector3.zero)
                {
                    held = true;
                    pickedObject = hit.collider.gameObject;
                    initialPickUpPosition = pickedObject.transform.position;
                    initialPickUpPosition.y += pickUpHeightOffset; 
                    pickedObject.transform.position = initialPickUpPosition;
                }
            }
        }
        else
        {
            held = false;
            pickedObject = null;
        }
    }

    private void RollDie(GameObject die)
    {
        die.GetComponent<Dice>().RollDice();
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}