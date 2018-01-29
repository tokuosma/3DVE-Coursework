using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController Instance;

    private List<HauntedObject> hauntedObjects;
    private int numberOfHauntedObjects;
    private int objectsScanned;

    private List<SpawnPoint> spawnPoints;

    public GameObject WitchPrefab;
    
    private GameObject spawnedWitch;

    public float witchSpawnDelay;
    private bool isSpawning;

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
        spawnPoints = new List<SpawnPoint>(FindObjectsOfType<SpawnPoint>());
        spawnedWitch = FindObjectOfType<Witch>().gameObject;

	}
	
	// Update is called once per frame
	void Update () {
		if(spawnedWitch == null && !isSpawning)
        {
            StartCoroutine("SpawnWitch");
        }
	}

    private IEnumerator SpawnWitch()
    {
        isSpawning = true;
        SpawnPoint spawnPoint = spawnPoints[(int)Mathf.Floor(Random.Range(0, spawnPoints.Count - 1))];
        var timer = 0f;
        while (timer < witchSpawnDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        spawnedWitch = Instantiate(WitchPrefab, spawnPoint.transform.position, Quaternion.identity);
        isSpawning = false;
    }

}
