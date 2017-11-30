using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    public Transform LightBallSpawnPoint;
    public GameObject LightBallPrefab;
    private GameObject CurrentBall;

	// Use this for initialization
	void Start () {
        CurrentBall = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(CurrentBall != null)
            {
                Destroy(CurrentBall);
            }
            CurrentBall = Instantiate(
                LightBallPrefab,
                LightBallSpawnPoint.position,
                LightBallSpawnPoint.rotation,
                LightBallSpawnPoint);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(CurrentBall != null)
            {
                if (CurrentBall.GetComponent<LightBall>().Charged)
                {
                    CurrentBall.GetComponent<LightBall>().Fire();
                }
                else
                {
                    CurrentBall.GetComponent<LightBall>().StartFade();
                    Destroy(CurrentBall, 2.0f);
                }
            }
        }
	}
}
