using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ForkliftController : MonoBehaviour
{
    public bool isOcupied;
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
    private float reverse;

    public WheelCollider[] colliders;
    public GameObject[] meshes;
    public GameObject pillars;
    public GameObject plate;
    public GameObject[] forks;
    public GameObject cam;
    public float sensitivity;

    private float clampAngle = 85;
    private float verticalRotation;
    private float horizontalRotation;
    [SerializeField]
    private GameObject steeringWheel;
    void Update(){
        for (int i = 0; i < colliders.Length; i++){
            colliders[i].GetWorldPose(out Vector3 pos, out Quaternion rot);
            meshes[i].transform.position = pos;
            meshes[i].transform.rotation = rot;
            colliders[i].brakeTorque = 50;
            colliders[i].motorTorque = 0;
        }

        if(!isOcupied){return;}

        throttleInput = Mathf.Lerp(throttleInput,Input.GetAxis("Vertical"), Time.deltaTime * 10);
        throttleInput = Mathf.Clamp(throttleInput,0,1);
        brakeInput = Mathf.Lerp(brakeInput,Input.GetAxis("Vertical"), Time.deltaTime * 10);
        brakeInput = Mathf.Clamp(brakeInput,-1,0);
        //brakeInput = Mathf.Abs(brakeInput);
        steeringInput = Mathf.Lerp(steeringInput,Input.GetAxis("Horizontal"), Time.deltaTime*7);

        if(Input.GetKey(KeyCode.LeftShift)){
            heightInput += Time.deltaTime;
            heightInput = Mathf.Clamp(heightInput,0,1);
            plate.transform.localPosition = new Vector3(0f,-1.297538f,0.5903233f + (heightInput * 1.8776767f));
        }
        if(Input.GetKey(KeyCode.Space)){
            heightInput -= Time.deltaTime;
            heightInput = Mathf.Clamp(heightInput,0,1);
            plate.transform.localPosition = new Vector3(0f,-1.297538f,0.5903233f + heightInput * 1.8776767f);
        }

        if(Input.GetButton("Fire1")){
            tiltInput += 2*Time.deltaTime;
            tiltInput = Mathf.Clamp(tiltInput,-1,1);
            pillars.transform.localRotation = Quaternion.Euler(5.5f*tiltInput,0,0);
        }
        if(Input.GetButton("Fire2")){
            tiltInput -= 2*Time.deltaTime;
            tiltInput = Mathf.Clamp(tiltInput,-1,1);
            pillars.transform.localRotation = Quaternion.Euler(5.5f*tiltInput,0,0);
        }

        if(Input.GetKey(KeyCode.R)){
            distanceInput += Time.deltaTime;
            distanceInput = Mathf.Clamp(distanceInput,0,1);
            for (int i = 0; i < 2; i++){
                float x = i%2==0? 1f:-1f;
                forks[i].transform.localPosition = new Vector3(distanceInput * 0.44f * x,0f,0f);
            }
        }
        if(Input.GetKey(KeyCode.F)){
            distanceInput -= Time.deltaTime;
            distanceInput = Mathf.Clamp(distanceInput,0,1);
            for (int i = 0; i < 2; i++){
                float x = i%2==0? 1f:-1f;
                forks[i].transform.localPosition = new Vector3(distanceInput * 0.44f * x,0f,0f);
            }
        }



        colliders[colliders.Length-1].motorTorque = throttleInput*forkliftPower;
        colliders[colliders.Length-1].steerAngle = -steeringInput * 45f;

        for (int i = 0; i < colliders.Length; i++){
            colliders[i].brakeTorque = (-brakeInput) * 300;
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

    }
}
