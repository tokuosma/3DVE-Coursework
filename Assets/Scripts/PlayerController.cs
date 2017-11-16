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
	void Update () {
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
        //Gravity
        if (player.isGrounded == false)
        {

            Vector3 falling = new Vector3(0, -gravityAcc, 0);
            player.Move(falling * Time.deltaTime);

        }





    }
}
