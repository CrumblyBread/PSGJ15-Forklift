using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ForkliftController : MonoBehaviour
{
    private bool isOcupied;
    [Header("Inputs")]
    [SerializeField]
    private float throttleInput;
    [SerializeField]
    private float brakeInput;
    [SerializeField]
    private float steeringInput;
    [SerializeField]
    private float heightInput;
    [SerializeField]
    private float distanceInput;
    [SerializeField]
    private float tiltInput;


    [Header("Parameters")]
    [SerializeField]
    private float forksDistance;
    [SerializeField]
    private float forkTilt;
    [SerializeField]
    private float forkHeight;
    public float forkliftPower;
    private float reverse = 1;

    public WheelCollider[] colliders;
    public GameObject[] meshes;
    public GameObject pillars;
    public GameObject plate;
    public GameObject[] forks;
    public GameObject cam;
    public GameObject hardhat;
    public GameObject frontLights;
    public float sensitivity;

    private float clampAngle = 85;
    private float verticalRotation;
    private float horizontalRotation;
    [SerializeField]
    private GameObject steeringWheel;
    public GameObject exitPoint;
    void Update(){

        for (int i = 0; i < colliders.Length; i++){
            colliders[i].GetWorldPose(out Vector3 pos, out Quaternion rot);
            meshes[i].transform.position = pos;
            meshes[i].transform.rotation = rot;
            colliders[i].brakeTorque = 50;
            colliders[i].motorTorque = 0;
        }

        frontLights.SetActive(isOcupied);

        /*if(Mathf.Abs(transform.rotation.x) > 0.01f){
            transform.SetPositionAndRotation(transform.position,Quaternion.Euler(0f,transform.rotation.y,transform.rotation.z));
        }

        if(Mathf.Abs(transform.rotation.z) > 0.01f){
            transform.SetPositionAndRotation(transform.position,Quaternion.Euler(transform.rotation.x,transform.rotation.y,0f));
        }*/

        if(!isOcupied){
            return;
        }

        throttleInput = Mathf.Lerp(throttleInput,Input.GetAxis("Vertical"), Time.deltaTime * 10);
        throttleInput = Mathf.Clamp(throttleInput,0,1);
        brakeInput = Mathf.Lerp(brakeInput,Input.GetAxis("Vertical"), Time.deltaTime * 10);
        brakeInput = Mathf.Clamp(brakeInput,-1,0);
        //brakeInput = Mathf.Abs(brakeInput);
        steeringInput = Mathf.Lerp(steeringInput,Input.GetAxis("Horizontal"), Time.deltaTime*7);

        if(Input.GetKey(KeyCode.LeftShift)){
            heightInput += 0.66f*Time.deltaTime;
            heightInput = Mathf.Clamp(heightInput,0,1);
            plate.transform.localPosition = new Vector3(0f,-1.297538f,0.5903233f + (heightInput * 1.8776767f));
            UIManager.instance.forkHeightIndicator(heightInput);
        }
        if(Input.GetKey(KeyCode.Space)){
            heightInput -= 0.66f*Time.deltaTime;
            heightInput = Mathf.Clamp(heightInput,0,1);
            plate.transform.localPosition = new Vector3(0f,-1.297538f,0.5903233f + heightInput * 1.8776767f);
            UIManager.instance.forkHeightIndicator(heightInput);
        }

        if(Input.GetButton("Fire1")){
            tiltInput += 2*Time.deltaTime;
            tiltInput = Mathf.Clamp(tiltInput,-1,1);
            pillars.transform.localRotation = Quaternion.Euler(5.5f*tiltInput,0,0);
            UIManager.instance.forkHTiltIndicator(tiltInput);
        }
        if(Input.GetButton("Fire2")){
            tiltInput -= 2*Time.deltaTime;
            tiltInput = Mathf.Clamp(tiltInput,-1,1);
            pillars.transform.localRotation = Quaternion.Euler(5.5f*tiltInput,0,0);
            UIManager.instance.forkHTiltIndicator(tiltInput);
        }

        if(Input.GetKey(KeyCode.R)){
            distanceInput += Time.deltaTime;
            distanceInput = Mathf.Clamp(distanceInput,0,1);
            for (int i = 0; i < 2; i++){
                float x = i%2==0? 1f:-1f;
                forks[i].transform.localPosition = new Vector3(distanceInput * 0.44f * x,0f,0f);
            }
            UIManager.instance.forkDistanceIndicator(distanceInput);
        }
        if(Input.GetKey(KeyCode.F)){
            distanceInput -= Time.deltaTime;
            distanceInput = Mathf.Clamp(distanceInput,0,1);
            for (int i = 0; i < 2; i++){
                float x = i%2==0? 1f:-1f;
                forks[i].transform.localPosition = new Vector3(distanceInput * 0.44f * x,0f,0f);
            }
            UIManager.instance.forkDistanceIndicator(distanceInput);
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            if(reverse < 0){reverse = 1;UIManager.instance.ForkliftReverser(true);return;}
            if(reverse > 0){reverse = -1;UIManager.instance.ForkliftReverser(false); return;}
        }



        colliders[colliders.Length-1].motorTorque = throttleInput*forkliftPower * reverse;
        colliders[colliders.Length-1].steerAngle = -steeringInput * 15f;

        for (int i = 0; i < colliders.Length; i++){
            colliders[i].brakeTorque = -brakeInput;
            if(brakeInput > 0.9f){
                colliders[i].motorTorque = 0;
            }
        }
        
        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");

        steeringWheel.transform.localRotation = Quaternion.Euler(-124.76f,0,steeringInput*360f*1.5f);

        verticalRotation += _mouseVertical * Time.deltaTime * sensitivity;
        horizontalRotation += _mouseHorizontal * Time.deltaTime * sensitivity;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);
        horizontalRotation = Mathf.Clamp(horizontalRotation, -clampAngle, clampAngle);

        cam.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f); 

        if(Input.GetKeyDown(KeyCode.E)){
            GetOut();
        }

    }

    public void GetIn(){
        Invoke("GI",0.1f);
    }
    public void GI(){
        isOcupied = true;
        cam.SetActive(true);
        PlayerController.instance.gameObject.SetActive(false);
        hardhat.SetActive(PlayerController.instance.hasHelmet);
        UIManager.instance.GetIntoForklift();

    }

    public void GetOut(){
        isOcupied = false;
        cam.SetActive(false);
        PlayerController.instance.gameObject.SetActive(true);
        hardhat.SetActive(false);
        PlayerController.instance.gameObject.transform.SetPositionAndRotation(exitPoint.transform.position,exitPoint.transform.rotation);
        UIManager.instance.GetOutForklift();
    }
}
