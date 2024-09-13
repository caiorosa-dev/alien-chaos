using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlanetController : MonoBehaviour
{
    [SerializeField]
    private GameObject planetTurret;

    private bool isInstantiated = false;

    void Start()
    {
        GetComponent<PlanetUpgrade>().planetUpgrade.onUpgrade += (float newValue) =>
        {
            planetTurret.GetComponent<PlanetTurret>().fireRate = newValue;
            if (!isInstantiated)
            {
                GameObject newInstance = Instantiate(planetTurret);
                newInstance.transform.position = gameObject.transform.position;
                isInstantiated = true;
            }
        };
    }
}
