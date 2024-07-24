using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;

    public GameObject currentForklift;
    public GameObject forkliftPrefab;
    public GameObject forkliftSpawn;

    private void Awake() {
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

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Backspace)){
            ResetForklift();
        }
    }

    public void ResetForklift(){
        Destroy(currentForklift);
        GameObject go = Instantiate(forkliftPrefab,forkliftPrefab.transform.position,forkliftSpawn.transform.rotation);
        currentForklift = go;
    }
}
