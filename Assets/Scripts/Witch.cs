// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Witch : MonoBehaviour {

	public Transform PlayerTransform;
	private NavMeshAgent agent;
    private Vector3 startPosition;
    
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (PlayerTransform.transform.position);
        startPosition = transform.position;
	}	

	void Update(){
        agent.SetDestination(PlayerTransform.transform.position);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            transform.position = startPosition;
        }
    }
}