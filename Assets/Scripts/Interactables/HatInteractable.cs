using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatInteractable : Interactable
{
    public GameObject cameraHat;
    public override void Interact(){
        cameraHat.SetActive(true);
        this.gameObject.SetActive(false);
        PlayerController.instance.hasHelmet = true;
    }
}
