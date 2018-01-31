// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.PostProcessing;

public class Witch : MonoBehaviour
{
    public AudioClip spawnSound;
    public AudioClip deathSound;
    public AudioClip hitSound;
    public float attackDistance;
    private GameObject player;
    private PostProcessVolume postProcessVolume;
    private NavMeshAgent agent;
    private Animator animator;
    bool inRange;
    private bool canAttack;

    public float attackStrength;

    public bool InRange
    {
        get
        {
            return inRange;
        }

        private set
        {
            inRange = value;
        }
    }

    public bool CanAttack
    {
        get
        {
            return canAttack;
        }

        private set
        {
            canAttack = value;
        }
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        agent = GetComponent<NavMeshAgent>();
        postProcessVolume = GetComponentInChildren<PostProcessVolume>();
        agent.SetDestination(player.transform.position);
        animator = GetComponentInChildren<Animator>();
        CanAttack = true;
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
        InRange = (player.transform.position - transform.position).magnitude <= attackDistance;
        animator.SetBool("isAttacking", InRange);
        //if (InRange && CanAttack)
        //{
        //    player.GetComponent<PlayerController>().DamagePlayer(attackStrength);
        //}
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
        CanAttack = false;
        postProcessVolume.ResetValues();
        animator.SetTrigger("WitchHit");
        GetComponent<AudioSource>().PlayOneShot(deathSound);
        Destroy(Instantiate(GameController.Instance.DeathExplosionPrefab, transform.position, Quaternion.identity), 3.0f);
        GetComponent<Collider>().enabled = false;
        agent.isStopped = true;
        Destroy(gameObject, GetComponent<AudioSource>().clip.length);
    }
}