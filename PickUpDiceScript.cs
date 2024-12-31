using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDiceScript : MonoBehaviour
{
    [SerializeField] private Animator handAnim;
    [SerializeField] public Transform diceHolder;
    [SerializeField] private GameObject diePrefab;

    private GameObject activeDie;
    private bool holdingDice;
    [SerializeField] private List<GameObject> diceList = new List<GameObject>();

    public bool animHappening;

    void Update()
    {
        if (holdingDice)
        {
            activeDie.transform.position = diceHolder.position;
        }

        if (diceList.Count > 0 && !animHappening)
        {
            PickUpDice(diceList[0]);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Die") || collision.collider.CompareTag("Coin"))
        {
            diceList.Add(collision.gameObject);
        }
    }

    public void PickUpDice(GameObject die)
    {
        holdingDice = true;
        activeDie = die;
        die.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        handAnim.SetBool("handMove", true);
        diceList.Remove(die);
    }

    public void LetDiceGo()
    {
        holdingDice = false;
        handAnim.SetBool("handMove", false);
    }
}
