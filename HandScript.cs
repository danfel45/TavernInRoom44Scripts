using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    [SerializeField] private PickUpDiceScript ground;
    
 

    public void LetGo()
    {
        ground.LetDiceGo();
    }

    public void ToggleAnimOn()
    {
        
            ground.animHappening = true;
      
    }

    public void ToggleAnimOff()
    {
        ground.animHappening = false;
    }
}
