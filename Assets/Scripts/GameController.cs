using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class GameController : MonoBehaviour {

    public static GameController Instance;

    private List<HauntedObject> hauntedObjects;
    public int numberOfHauntedObjects;
    public int objectsScanned;
    public bool isWinnable;

    private List<SpawnPoint> spawnPoints;
    private List<LabyrinthSpawnPoint> labyrinthSpawnPoints;


    public GameObject WitchPrefab;
    public GameObject DeathExplosionPrefab;
    public GameObject LabyrinthEscapePrefab;
    public PostProcessingProfile defaultPlayerProfile;
    public AudioClip labyrinthSound;
    public AudioClip rescueSound;
    public GameObject spawnedWitch;
    public PlayerController player;

    private Vector3 playerStartPosition;

    public float witchSpawnDelay;
    private bool spawningEnabled;

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
        labyrinthSpawnPoints = new List<LabyrinthSpawnPoint>(FindObjectsOfType<LabyrinthSpawnPoint>());
        var witch = FindObjectOfType<Witch>();
        if(witch != null)
        {
            spawnedWitch = witch.gameObject;
            spawningEnabled = false;
        }
        else
        {
            spawningEnabled = true ;
        }
        isWinnable = false;
        player = FindObjectOfType<PlayerController>();
        playerStartPosition = player.transform.position;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(spawnedWitch == null && spawningEnabled)
        {
            StartCoroutine("SpawnWitch");
        }
        if (player.Health <= 0)
        {
            SendPlayerToLabyrinth();
        }
	}

    private IEnumerator SpawnWitch()
    {
        spawningEnabled = false;
        SpawnPoint spawnPoint = spawnPoints[(int)Mathf.Floor(Random.Range(0, spawnPoints.Count - 1))];
        var timer = 0f;
        while (timer < witchSpawnDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        spawnedWitch = Instantiate(WitchPrefab, spawnPoint.transform.position, Quaternion.identity);
        spawningEnabled = true;
    }

    private void SendPlayerToLabyrinth()
    {
        LabyrinthSpawnPoint playerSpawnPoint = labyrinthSpawnPoints[Mathf.FloorToInt(UnityEngine.Random.Range(0, labyrinthSpawnPoints.Count - 1))];
        LabyrinthSpawnPoint exitSpawnPoint = null;
        do
        {
            exitSpawnPoint = labyrinthSpawnPoints[Mathf.FloorToInt(UnityEngine.Random.Range(0, labyrinthSpawnPoints.Count - 1))];
        } while (exitSpawnPoint == playerSpawnPoint);
        player.Health = PlayerController.maxHealth;
        player.transform.position = playerSpawnPoint.transform.position;

        GetComponent<AudioSource>().PlayOneShot(labyrinthSound);
        Instantiate(LabyrinthEscapePrefab, exitSpawnPoint.transform.position, Quaternion.identity);
        Destroy(Instantiate(DeathExplosionPrefab, player.transform.position, Quaternion.identity),3.0f);
        Destroy(spawnedWitch);
        spawningEnabled = false;
    }

    public void RescuePlayer()
    {
        spawningEnabled = true;
        player.transform.position = playerStartPosition;
        GetComponent<AudioSource>().PlayOneShot(rescueSound);
    }

    public void ObjectScanned(HauntedObject hauntedObject)
    {
        objectsScanned++;
        if(objectsScanned >= numberOfHauntedObjects)
        {
            isWinnable = true;
        }
    }

    public void Win()
    {
        LevelManager.Instance.LoadScene("WinScene");
    }
}
