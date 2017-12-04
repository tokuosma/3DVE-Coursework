using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlast : MonoBehaviour {

    private ParticleSystem blastParticleSystem;
	// Use this for initialization
	void Start () {
        blastParticleSystem = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!blastParticleSystem.isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
