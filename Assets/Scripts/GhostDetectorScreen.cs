using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for handling the updating of the ghost detector screen
/// </summary>
public class GhostDetectorScreen : MonoBehaviour {

    public Text bodyText;
    public Text statusText;
    public float scanDistance;
    public Slider scanProgress;

    private GhostMeterProximityCollider ghostMeterProximityCollider;
    private RaycastHit rayCastHit;
    private Camera mainCamera;
    private bool isScanning;
    private HauntedObject hauntedObject;
    private Coroutine scanCoroutine;
    private PlayerController player;
    private bool hasMessage;

	// Use this for initialization
	void Start () {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mainCamera = FindObjectOfType<Camera>();
        player = FindObjectOfType<PlayerController>();
        ghostMeterProximityCollider = FindObjectOfType<GhostMeterProximityCollider>();
        scanProgress.gameObject.SetActive(false);
        isScanning = false;
        rayCastHit = new RaycastHit();
        hasMessage = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.F))
        {
            StopAllCoroutines();
            bodyText.enabled = true;
            if(Physics.Raycast(transform.position, mainCamera.transform.forward, out rayCastHit, scanDistance))
            {
                hauntedObject = rayCastHit.collider.gameObject.GetComponent<HauntedObject>();
                if (hauntedObject != null && hauntedObject.isScanned == false)
                {
                    isScanning = true;
                    scanCoroutine = StartCoroutine("ScanCoroutine", hauntedObject);
                }
                else
                {
                    bodyText.text = "No target found!";
                    StartCoroutine("FlashBodyText",true);
                }
            }
            else
            {
                bodyText.text = "No target found!";
                StartCoroutine("FlashBodyText", true);
            }
        }

        
        if (isScanning)
        {
            CheckScanInterrupt();
        }
        else if (ghostMeterProximityCollider.HauntingDetected && !hasMessage )
        {
            bodyText.text = string.Format("Haunting Detected!\nNumber of hauntings: {0}\nClosest haunting: {1:F1}", ghostMeterProximityCollider.NumberOfHauntedObjectsDetected, ghostMeterProximityCollider.ClosestHauntedObjectDistance );
        }
        else if(!hasMessage)
        {
            bodyText.text = string.Format("No hauntings detected nearby");
        }

        statusText.text = string.Format("HEALTH: {0:F0}", player.Health);
	}

    private void CheckScanInterrupt()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            CancelScan();
        }

        if (Physics.Raycast(transform.position, mainCamera.transform.forward, out rayCastHit, scanDistance))
        {
            hauntedObject = rayCastHit.collider.gameObject.GetComponent<HauntedObject>();
            if (hauntedObject == null)
            {
                CancelScan();
            }
        }
        else
        {
            CancelScan();
        }
    }

    private void CancelScan()
    {
        bodyText.text = "Scanning interrupted!";
        scanProgress.gameObject.SetActive(false);
        StopCoroutine(scanCoroutine);
        StartCoroutine("FlashBodyText", true);
        isScanning = false;
    }

    /// <summary>
    /// Flashes the screen body text
    /// </summary>
    /// <param name="clearText">Optional parameter. Default false. If true, clears the body text at the end of the coroutine. </param>
    /// <returns></returns>
    private IEnumerator FlashBodyText(bool clearText = false)
    {
        // Flash the text
        hasMessage = true;
        for (int i = 0; i <= 5; i++)
        {
            bodyText.enabled = !bodyText.enabled;
            yield return new WaitForSeconds(.5f);
        }
        bodyText.enabled = true;
        if (clearText)
        {
            bodyText.text = String.Empty;
        }
        hasMessage = false;
    }

    private IEnumerator ScanCoroutine(HauntedObject target)
    {
        bodyText.text = "Scanning...";
        scanProgress.gameObject.SetActive(true);
        scanProgress.value = scanProgress.minValue;
        float increment = scanProgress.maxValue / 10f;
        while (scanProgress.value < scanProgress.maxValue)
        {
            scanProgress.value += increment;
            yield return new WaitForSeconds(0.4f);
        }
        scanProgress.gameObject.SetActive(false);
        target.SetScanned();
        ghostMeterProximityCollider.RemoveHauntedObject(target);
        isScanning = false;
        bodyText.text = "HAUNTING SCANNED!";
        StartCoroutine("FlashBodyText", true);

    }

    
}
