using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    //public float jumpSensitivity;
    public float mouseSensitivity = 5f;
    public float gravityAcc = 10f;


    CharacterController player;
    public GameObject eyes;

    float movementFB;
    float movementLR;

    float rotX;
    float rotY;
    

	// Use this for initialization
	void Start () {

        player = GetComponent<CharacterController>();

		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Player movement and view orientation
        movementFB = Input.GetAxis("Vertical") * speed;
        movementLR = Input.GetAxis("Horizontal") * speed;

        rotX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        Vector3 movement = new Vector3(movementFB, 0, -movementLR);
        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);


        transform.Rotate(0, rotX, 0);
        
        eyes.transform.Rotate(-rotY, 0, 0);



        // Vector3 clamped = new Vector3(
        // Mathf.Clamp(eyes.transform.localEulerAngles.x, 0.0f, 360.0f),
        // eyes.transform.localEulerAngles.y,
        // eyes.transform.localEulerAngles.z);

        // eyes.transform.localEulerAngles = clamped;
        //Debug.Log(eyes.transform.localRotation.eulerAngles.x);

        if (!(eyes.transform.localRotation.eulerAngles.x >= 270f && eyes.transform.localRotation.eulerAngles.x < 360f))
        {
            eyes.transform.localEulerAngles = new Vector3(Mathf.Clamp(eyes.transform.localEulerAngles.x, -1f, 80f ), eyes.transform.localEulerAngles.y, 0);
            Debug.Log("Lower");
            Debug.Log(Mathf.Clamp(eyes.transform.localEulerAngles.x, -1f, 80.0f));
            // Debug.Log(eyes.transform.localRotation.x);
        }
        if (!(eyes.transform.localRotation.eulerAngles.x >= -1f && eyes.transform.localRotation.eulerAngles.x <= 90f))
        {

            eyes.transform.localEulerAngles = new Vector3(Mathf.Clamp(Mathf.Abs(eyes.transform.localEulerAngles.x), 280.0f, 370f), eyes.transform.localEulerAngles.y, 0);
            Debug.Log("Upper");
            Debug.Log(Mathf.Abs(eyes.transform.localEulerAngles.x));
            Debug.Log(Mathf.Clamp(Mathf.Abs(eyes.transform.localEulerAngles.x), 280f, 370f));
        }


        //Gravity
        if (player.isGrounded == false)
        {

            Vector3 falling = new Vector3(0, -gravityAcc, 0);
            player.Move(falling * Time.deltaTime);

        }





    }
}
