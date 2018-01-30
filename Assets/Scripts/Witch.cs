// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.PostProcessing;

public class Witch : MonoBehaviour
{

    public AudioClip spawnSound;
    public AudioClip deathSound;
    public float attackDistance;
    private GameObject player;
    private PostProcessVolume postProcessVolume;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        agent = GetComponent<NavMeshAgent>();
        postProcessVolume = GetComponentInChildren<PostProcessVolume>();
        agent.SetDestination(player.transform.position);
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
        animator.SetBool("isAttacking", (player.transform.position - transform.position).magnitude <= attackDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Die();
        }
    }

    private void Die()
    {
        //PLAY ANIMATION HERE
        postProcessVolume.ResetValues();
        animator.SetTrigger("WitchHit");
        GetComponent<AudioSource>().PlayOneShot(deathSound);
        Destroy(Instantiate(GameController.Instance.DeathExplosionPrefab, transform.position, Quaternion.identity), 3.0f);
        GetComponent<Collider>().enabled = false;
        agent.isStopped = true;
        Destroy(gameObject, GetComponent<AudioSource>().clip.length);
    }
}