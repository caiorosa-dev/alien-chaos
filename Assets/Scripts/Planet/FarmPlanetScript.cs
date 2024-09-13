using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmPlanetScript : MonoBehaviour
{
    // Start is called before the first frame update
    int currentLevel = 0;
    float nextSpawnTime;
    [SerializeField]
    GameObject spawnedOre;
    void Start()
    {
        GetComponent<PlanetUpgrade>().planetUpgrade.onUpgrade += (float newValue)=>
        {
            currentLevel = (int)newValue;
        };
    }   

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime && currentLevel != 0)
        {
            nextSpawnTime = Time.time + currentLevel;
            spawnedOre.transform.position = gameObject.transform.position;
            Instantiate(spawnedOre);
        }

    }
}
