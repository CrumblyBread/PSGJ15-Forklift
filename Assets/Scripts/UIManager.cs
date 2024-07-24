using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("Main Menu")]
    public GameObject MainMenu;
    public GameObject MainMenuScene;
    [Header("Game UI")]
    public GameObject GameUI;
    [Header("ForkliftUI")]
    public GameObject ForkiftUI;
    public Animator forkliftFRAnim;
    public GameObject hightIndicator;
    public GameObject tiltIndicator;
    public GameObject[] distanceIndicators;

    private void Awake(){
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SendIntoGame(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.instance.player.SetActive(true);
        GameUI.SetActive(true);
        MainMenu.SetActive(false);
        MainMenuScene.SetActive(false);
    }

    public void QuitButton(){
        Application.Quit();
    }

    public void GetIntoForklift(){
        GameUI.SetActive(false);
        ForkiftUI.SetActive(true);
        forkliftFRAnim.SetBool("Forward",true);
    }

    public void GetOutForklift(){
        GameUI.SetActive(true);
        ForkiftUI.SetActive(false);
        forkliftFRAnim.SetBool("Forward",true);
    }

    public void ForkliftReverser(bool value){
        forkliftFRAnim.SetBool("Forward",value);
    }

    public void forkHeightIndicator(float heightInput){
        hightIndicator.GetComponent<RectTransform>().localPosition = new Vector3(112.7f, (heightInput * 120)+85f, 0f);
    }

    public void forkHTiltIndicator(float tiltInput){
        tiltIndicator.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, -tiltInput * 10f);
    }

    public void forkDistanceIndicator(float distanceInput){
        distanceIndicators[0].GetComponent<RectTransform>().localPosition = new Vector3((-40) - (50f*distanceInput),92f,0f);
        distanceIndicators[1].GetComponent<RectTransform>().localPosition = new Vector3((50f*distanceInput),92f,0f);
    }
}
