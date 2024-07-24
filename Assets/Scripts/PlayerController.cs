using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]public static PlayerController instance;
    public GameObject cam;
    public Rigidbody rb;
    public bool hasHelmet;
    public GameObject flashlight;
    public float moveSpeed;
    public float clampAngle;
    public float sensitivity = 300f;
    [Header("Constraints")]
    public bool canMove;
    public bool canLook;
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

    private void FixedUpdate() {
        Move();
        Look();
        //TODO: move to update?
        if(Input.GetKeyDown(KeyCode.E)){
            RaycastHit hitInfo;
            if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hitInfo,1.5f)){
                if(hitInfo.transform.gameObject.GetComponent<ForkliftController>() != null){
                    hitInfo.transform.gameObject.GetComponent<ForkliftController>().GetIn();
                    return;
                }
                if(hitInfo.transform.gameObject.GetComponent<Interactable>() != null){
                    hitInfo.transform.gameObject.GetComponent<Interactable>().Interact();
                    return;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.F)){
            flashlight.SetActive(!flashlight.activeSelf);
        }
    }
    
    private void Move(){
        if(!canMove){
            return;
        }

        Vector2 _inputDirection = Vector2.zero;
        _inputDirection.y += Input.GetAxis("Vertical");
        _inputDirection.x += Input.GetAxis("Horizontal");

        //if (Physics.Raycast(this.transform.position,-transform.up,0.2f)) {
	        Vector3 targetVelocity = new Vector3(_inputDirection.x, 0, _inputDirection.y);
	        targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= moveSpeed;
 
	        Vector3 velocity = rb.velocity;
	        Vector3 velocityChange = targetVelocity - velocity;
	        velocityChange.y = 0;
	        rb.AddForce(velocityChange, ForceMode.VelocityChange);
	    //}

    }
    private float verticalRotation;
    private float horizontalRotation;
    public void Look(){
        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");

        verticalRotation += _mouseVertical * Time.deltaTime * sensitivity;
        horizontalRotation += _mouseHorizontal * Time.deltaTime * sensitivity;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f); 
        this.gameObject.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }

}
