// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Witch : MonoBehaviour {

	public Transform PlayerTransform;
	private NavMeshAgent agent;
    
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (PlayerTransform.transform.position);
		
	}	

	void Update(){
        agent.SetDestination(PlayerTransform.transform.position);

    }

}