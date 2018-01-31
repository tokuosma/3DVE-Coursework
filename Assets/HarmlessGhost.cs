using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class HarmlessGhost : MonoBehaviour {

    private NavMeshAgent agent;
    private List<LabyrinthSpawnPoint> labyrinthSpawnPoints;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        labyrinthSpawnPoints = new List<LabyrinthSpawnPoint>(FindObjectsOfType<LabyrinthSpawnPoint>());
        agent.SetDestination(getRandomSpawnPoint().transform.position);
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(1f, 2f);
    }
	
    private LabyrinthSpawnPoint getRandomSpawnPoint()
    {
        try
        {
            LabyrinthSpawnPoint labyrinthSpawnPoint = labyrinthSpawnPoints[Mathf.FloorToInt(UnityEngine.Random.Range(0, labyrinthSpawnPoints.Count - 1))];
            return labyrinthSpawnPoint;
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("No labyrinth spawn points set!");
            return null;
        }
        catch (Exception) { throw; }
    }

	// Update is called once per frame
	void Update () {
		if(agent.remainingDistance <= 2)
        {
            agent.SetDestination(getRandomSpawnPoint().transform.position);
        }
	}
}
