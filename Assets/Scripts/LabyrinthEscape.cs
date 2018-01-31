using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthEscape : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            GameController.Instance.RescuePlayer();
            Destroy(gameObject);
        }
    }
}
