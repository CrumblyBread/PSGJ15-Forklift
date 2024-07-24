using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lunchInteractable : Interactable
{
    public GameObject[] lunch;

    public override void Interact(){
        for (int i = 0; i < lunch.Length; i++){
            lunch[i].SetActive(false);
        }
        //TODO: count lunches in GameManager
        //TODO: play sound?
    }

}
