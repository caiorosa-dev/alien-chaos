using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField]
    GameObject smallAsteroid;
    [SerializeField]
    GameObject mediumAstroid;
    [SerializeField]
    GameObject bigAsteroid;
    [SerializeField]
    List<GameObject> DropableOres;
    [SerializeField]
    int maxAsteroidCounter;
    [SerializeField]
    List<Transform> SpawnPoints;

    private float nextSpawnTime = 0;
    private int amountOfExistingAsteroids = 0;

    void onAsteroidDeleted()
    {
        amountOfExistingAsteroids--;
    }

    void InstatiateAsteroid(GameObject asteroid)
    {
        GameObject newInstance = Instantiate(asteroid);
        newInstance.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].transform.position;
        newInstance.GetComponent<AsteroidDropSystem>().onDestroyCallback += onAsteroidDeleted;

        amountOfExistingAsteroids = GameObject.FindGameObjectsWithTag(newInstance.tag).Length;
    }

    GameObject getRandomOre()
    {
        return DropableOres[Random.Range(0, DropableOres.Count)];
    }

    GameObject getRandomAsteroid()
    {
        switch (Random.Range(1, 6))
        {
            case 1:
                this.bigAsteroid.GetComponent<AsteroidDropSystem>().dropedObject = getRandomOre();
                this.bigAsteroid.GetComponent<AsteroidDropSystem>().dropQuantity = 12;
                return bigAsteroid;
            case 2:
            case 3:
                this.mediumAstroid.GetComponent<AsteroidDropSystem>().dropedObject = getRandomOre();
                this.mediumAstroid.GetComponent<AsteroidDropSystem>().dropQuantity = 8;
                return mediumAstroid;

            default:
                this.smallAsteroid.GetComponent<AsteroidDropSystem>().dropedObject = getRandomOre();
                this.mediumAstroid.GetComponent<AsteroidDropSystem>().dropQuantity = 5;
                return smallAsteroid;
        }
    }

    void Update()
    {
        if (GameStateManager.Instance.getCurrentState() != GameState.Explore)
        {
            return;
        }

        if (amountOfExistingAsteroids < maxAsteroidCounter && Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + 0.5f;
            InstatiateAsteroid(getRandomAsteroid());
        }
    }
}
