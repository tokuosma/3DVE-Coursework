using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    public float mouseSensitivity = 5f;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public float gravityAcc = 10f;
    public bool gravity = true;


    CharacterController player;
    public Camera eyes;

    float movementFB;
    float movementLR;

    float rotX;
    float rotY;

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }




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



        Vector3 movement = new Vector3(movementLR, 0, movementFB);
        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);


        //Rotate player and eyes
        transform.Rotate(0, rotX, 0);
        eyes.transform.Rotate(-rotY, 0, 0);


        //Clamp rotation according to minimum and maximum angles (MinimumX and MaximumX)
        eyes.transform.localRotation = ClampRotationAroundXAxis(eyes.transform.localRotation);


        //if eyes rotate on any axes besides x (up - down), return them to zero
        if(eyes.transform.localRotation.eulerAngles.y != 0f || eyes.transform.localRotation.eulerAngles.z != 0)
        {
            eyes.transform.localEulerAngles = new Vector3(eyes.transform.localEulerAngles.x, 0f, 0f);
        }



        //Gravity
        if (gravity)
        {


            if (player.isGrounded == false)
            {

                Vector3 falling = new Vector3(0, -gravityAcc, 0);
                player.Move(falling * Time.deltaTime);

            }

        }



    }
}
