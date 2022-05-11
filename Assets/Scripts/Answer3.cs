using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Answer3 : MonoBehaviour, IPointerClickHandler
{
    public bool isProtected = false;
    public float protectedDuration = 1f;

    private float protectedDone;

    void Update()
    {
        if (isProtected)
        {
            if (Time.time > protectedDone)
            {
                isProtected = false;
            }
        }    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isProtected)
        {
            protectedDone = Time.time + protectedDuration;
            isProtected = true;

            GameController gC = GameController.instance;
            if (gC.options[gC.number-1] == "C")
            {
                gC.ShootRight(3);
            }
            else
            {
                gC.ShootError(gC.options[gC.number-1].ToCharArray()[0]-'A' + 1, 3);
            }   
        } 
    }
}
