using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauntedObject : MonoBehaviour {

    public bool isScanned;
    private static float emissionIncrement = 0.03f;
	// Use this for initialization
	void Start () {
        isScanned = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetScanned()
    {
        isScanned = true;
        StartCoroutine("ObjectScanned");
        //Destroy(this);
    }

    private IEnumerator ObjectScanned()
    {
        var renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        Color baseColor = new Color(0f, 0.9586205f, 1f, 1f); //Replace this with whatever you want for your base color at emission level '1'

        mat.EnableKeyword("_EMISSION");
        float emissionScale = 0f;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emissionScale);
        while(emissionScale < 1)
        {
            finalColor = baseColor * emissionScale;
            mat.SetColor("_EmissionColor", finalColor);
            emissionScale += emissionIncrement;
            yield return new WaitForSeconds(0.01f);
        }
        while (emissionScale > 0)
        {
            finalColor = baseColor * emissionScale;
            mat.SetColor("_EmissionColor", finalColor);
            emissionScale -= emissionIncrement / 2;
            yield return new WaitForSeconds(0.01f);
        }

        Destroy(this);
    }
}
