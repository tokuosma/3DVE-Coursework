// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class MoveTo : MonoBehaviour {

	public Transform goal;
	private NavMeshAgent agent;
    
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (goal.transform.position);
		
	}	

	void Update(){
        agent.SetDestination(goal.transform.position);

    }

}