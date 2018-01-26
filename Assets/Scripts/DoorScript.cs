using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    private Animator anim;
    public Collider other;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("triggered");
            anim.SetBool("open", true);

        }

    }

    // Update is called once per frame
    void Update () {
    }
}
