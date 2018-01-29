using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController Instance;

    private List<HauntedObject> hauntedObjects;
    private int numberOfHauntedObjects;
    private int objectsScanned;

    public GameObject WitchPrefab;
    public AudioClip spawnSound;
    public AudioClip deathSound;
    private GameObject spawnedWitch;

    public float witchSpawnDelay;

	// Use this for initialization
	void Start () {
		if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        hauntedObjects = new List<HauntedObject>( FindObjectsOfType<HauntedObject>());
        numberOfHauntedObjects = hauntedObjects.Count;
        objectsScanned = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
