using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBall : MonoBehaviour {

    public bool Charged { get { return charged; } }
    public GameObject ChargeParticles;
    public GameObject BlastPrefab;
    public float maxSize;
    public float chargeSpeed;

    public AudioClip chargeSound;
    public AudioClip fireSound;
    private AudioSource audioSource;
    private static float pitchMax = 2.1f;
    private static float pitchMin = 0.8f;

    private ParticleSystem ballParticleSystem;
    private ParticleSystem chargeParticleSystem;
    private bool charged;

    // Use this for initialization
    void Start () {

        ballParticleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        charged = false;

        StartCharge();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag != "Player")
        {
            var blast = Instantiate(BlastPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void StartCharge()
    {
        StartCoroutine("Charge");
    }

    /// <summary>
    /// Coroutine to gradually increase the size of the light ball
    /// </summary>
    /// <returns></returns>
    private IEnumerator Charge()
    {
        while (true)
        {
            var main = ballParticleSystem.main;
            var size = main.startSize;
            size.constantMax += chargeSpeed;
            GetComponent<Light>().intensity += 0.1f;
            if(audioSource.pitch < pitchMax)
                audioSource.pitch += 0.025f;
            main.startSize = size;

            // Continue increasing the size while ball smaller than the maximum size
            if(ballParticleSystem.main.startSize.constantMax < maxSize)
            {
                yield return new WaitForSeconds(.33f);
            }
            else
            {
                // Set as charged
                charged = true;
                yield break;
            }
        }
    }

    public void StartFade()
    {
        StartCoroutine("Fade");
        Destroy(gameObject, 1.0f);
        ChargeParticles.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    /// <summary>
    /// Coroutine to gradually decrease the size of the light ball when user interupts charging
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fade()
    {
        while (ballParticleSystem.main.startSize.constantMax > 0)
        {
            var main = ballParticleSystem.main;
            var size = main.startSize;
            if(audioSource.pitch > pitchMin)
                audioSource.pitch -= 0.1f;
            audioSource.volume -= 0.1f;
            size.constantMax -= 0.25f;
            GetComponent<Light>().intensity -= 0.25f;
            main.startSize = size;
            yield return new WaitForSeconds(.1f);
        }
    }

    /// <summary>
    /// Launches the ball
    /// </summary>
    public void Fire()
    {
        Vector3 velocity = transform.parent.forward * (-20.0f);
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.pitch = 1.0f;
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
        transform.parent = null;
        GetComponent<Rigidbody>().velocity = velocity;
        GetComponent<SphereCollider>().enabled = true;
        Destroy(gameObject, 2.0f);
    }
}
