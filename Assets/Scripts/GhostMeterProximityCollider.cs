using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostMeterProximityCollider : MonoBehaviour
{
    public float pingMinDelay;
    private GhostDetectorScreen ghostDetectorScreen;
    private List<HauntedObject> hauntedObjects;
    private AudioSource ping;

    private float closestHauntedObjectDistance;
    private float maxDistance;


    public bool HauntingDetected { get { return this.hauntedObjects.Count > 0; } }

    public float ClosestHauntedObjectDistance { get { return closestHauntedObjectDistance; } }

    public float NumberOfHauntedObjectsDetected { get { return hauntedObjects.Count; } }

    // Use this for initialization
    void Start()
    {
        ghostDetectorScreen = FindObjectOfType<GhostDetectorScreen>();
        hauntedObjects = new List<HauntedObject>();
        ping = GetComponent<AudioSource>();
        closestHauntedObjectDistance = float.PositiveInfinity;
        maxDistance = GetComponent<BoxCollider>().size.x / 2.0f;

        StartCoroutine("ProximityPing");
    }

    // Update is called once per frame
    void Update()
    {
        if (hauntedObjects.Count > 0)
        {
            closestHauntedObjectDistance = Mathf.Clamp(hauntedObjects.AsQueryable().Min(x => (x.transform.position - transform.position).magnitude), 1, maxDistance);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var hauntedObject = other.gameObject.GetComponent<HauntedObject>();
        if (hauntedObject != null && hauntedObject.isScanned == false)
        {
            Debug.Log("Haunted object in range!");
            hauntedObjects.Add(hauntedObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var hauntedObject = other.gameObject.GetComponent<HauntedObject>();
        if (hauntedObject != null)
        {
            Debug.Log("Out of range!");
            hauntedObjects.Remove(item: hauntedObject);
        }
    }

    private IEnumerator ProximityPing()
    {
        while (true)
        {
            if(HauntingDetected)
            {
                ping.Play();
            }
            yield return new WaitForSeconds(CalculatePingDelay());
        }
    }

    private float CalculatePingDelay()
    {
        if(HauntingDetected)
        {
            return Mathf.Clamp( ping.clip.length * Mathf.Pow((closestHauntedObjectDistance / maxDistance),1.5F), pingMinDelay, ping.clip.length  ) ;
        }
        else
        {
            return ping.clip.length;
        }
    }

    public void RemoveHauntedObject(HauntedObject hauntedObject)
    {
        hauntedObjects.Remove(hauntedObject);
    }



}
