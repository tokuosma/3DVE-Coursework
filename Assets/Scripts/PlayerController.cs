using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    //public float jumpSensitivity;
    public float mouseSensitivity = 5f;

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
        //Player movement
        movementFB = Input.GetAxis("Vertical") * speed;
        movementLR = Input.GetAxis("Horizontal") * speed;

        rotX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        Vector3 movement = new Vector3(movementFB, 0, -movementLR);
        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);


        transform.Rotate(0, rotX, 0);
        eyes.transform.Rotate(-rotY, 0, 0);
        //Mouse movement





    }
}
