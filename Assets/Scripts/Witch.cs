// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.PostProcessing;

public class Witch : MonoBehaviour {

    public AudioClip spawnSound;
    public AudioClip deathSound;

	private GameObject player;
    private PostProcessingProfile defaultProfile;
	private NavMeshAgent agent;
    private Vector3 startPosition;

    void Start () {
        player= FindObjectOfType<PlayerController>().gameObject;
		agent = GetComponent<NavMeshAgent> ();
        defaultProfile = FindObjectOfType<Camera>().gameObject.GetComponent<PostProcessingBehaviour>().profile;
        agent.SetDestination (player.transform.position);
        startPosition = transform.position;
	}	

	void Update(){
        agent.SetDestination(player.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Die();
        }
    }

    private void Die()
    {
        //PLAY ANIMATION HERE
        GetComponentInChildren<PostProcessVolume>().enabled = false;
        GetComponent<AudioSource>().PlayOneShot(deathSound);
        agent.isStopped = true ;
        FindObjectOfType<Camera>().gameObject.GetComponent<PostProcessingBehaviour>().profile = defaultProfile;
        Destroy(gameObject, deathSound.length);
    }
}