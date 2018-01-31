// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.PostProcessing;

public class Witch : MonoBehaviour
{
    public AudioClip spawnSound;
    public AudioClip deathSound;
    public AudioClip finalDeathSound;
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
        animator.SetBool("isAttacking", InRange && canAttack);
        //if (InRange && CanAttack)
        //{
        //    player.GetComponent<PlayerController>().DamagePlayer(attackStrength);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            if (GameController.Instance.isWinnable)
            {
                StartCoroutine("DieForGood");
            }
            else
            {
                Die();
            }
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

    private IEnumerator DieForGood()
    {
        GetComponent<Collider>().enabled = false;
        agent.isStopped = true;
        CanAttack = false;
        postProcessVolume.ResetValues();
        animator.SetTrigger("WitchHit");

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 0.5f;
        audioSource.clip = finalDeathSound;
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            Vector3 explosionPosition = transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
            Destroy(Instantiate(GameController.Instance.DeathExplosionPrefab, explosionPosition, Quaternion.identity), 2.0f);
            yield return new WaitForSecondsRealtime(0.40f);
        }
        Destroy(gameObject);
        GameController.Instance.Win();
    }
}