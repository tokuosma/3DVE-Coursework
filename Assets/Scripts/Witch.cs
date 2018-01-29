// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.PostProcessing;

public class Witch : MonoBehaviour {

    public AudioClip spawnSound;
    public AudioClip deathSound;
	private GameObject player;
    private PostProcessVolume postProcessVolume;
	private NavMeshAgent agent;
    private Vector3 startPosition;

    void Start () {
        player= FindObjectOfType<PlayerController>().gameObject;
		agent = GetComponent<NavMeshAgent> ();
        postProcessVolume = GetComponentInChildren<PostProcessVolume>();
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
        postProcessVolume.ResetValues();
        Instantiate(GameController.Instance.DeathExplosionPrefab, transform.position, Quaternion.identity);
        GetComponent<Collider>().enabled = false;
        transform.Find("Model").gameObject.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(deathSound);
        agent.isStopped = true ;
        Destroy(gameObject, deathSound.length  );
    }
}