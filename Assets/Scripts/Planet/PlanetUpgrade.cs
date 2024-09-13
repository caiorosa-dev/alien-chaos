using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlanetUpgrade : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isActive = false;
    public Upgrade planetUpgrade;
    public GameObject inventorySlotUI;
    private List<GameObject> requiredItensIcons = new List<GameObject>();

    private Light2D backgroundLight;
    private Collider2D planetColider;
    [SerializeField]
    private ContactFilter2D contactFilter;
    private List<Collider2D> colidedObject = new List<Collider2D>(1);
    [SerializeField]
    GameObject canvasParent;

    Coroutine fadeLightCoroutine;

    private void Start()
    {
        planetColider = GetComponent<Collider2D>();
        backgroundLight = GetComponentInChildren<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        planetColider.OverlapCollider(contactFilter, colidedObject);
        foreach (var item in colidedObject)
        {
            if (item.gameObject.tag == "Player")
            {
                if (!isActive)
                {
                    fadeLightToActive();
                    renderRequiredItems();
                }
                isActive = true;
                if (Input.GetKeyDown(KeyCode.Space) && isActive)
                {
                    if (planetUpgrade.DoUpgrade())
                    {
                        deleteRequiredItems();
                        renderRequiredItems();
                    }
                }
            }
            if (item.gameObject.tag == "Asteroid")
            {
                Destroy(item.gameObject);
            }
        }
        if (isActive && colidedObject.Count <= 0)
        {
            isActive = false;
            deleteRequiredItems();
            fadeLightToNotActive();
        }
    }

    void renderRequiredItems()
    {
        backgroundLight.intensity = 1;
        UpgradeLevel currentLevel = planetUpgrade.upgradeLevels[planetUpgrade.levelIndex];
        int i = 0;
        foreach (InventorySlot inventorySlot in currentLevel.requiredItems)
        {
            inventorySlotUI.GetComponent<InventorySlotUI>().slot = inventorySlot;
            GameObject newInstance = Instantiate(inventorySlotUI);
            newInstance.transform.SetParent(canvasParent.transform);
            newInstance.transform.localPosition = new Vector2(i * 2.3f, 0);
            newInstance.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            requiredItensIcons.Add(newInstance);
            i++;
        }
    }

    void fadeLightToActive()
    {
        if (fadeLightCoroutine != null) StopCoroutine(fadeLightCoroutine);

        fadeLightCoroutine = StartCoroutine(FadeLightIntensity(1, 1));
    }

    void fadeLightToNotActive()
    {
        if (fadeLightCoroutine != null) StopCoroutine(fadeLightCoroutine);

        fadeLightCoroutine = StartCoroutine(FadeLightIntensity(0.5f, 1));
    }

    public IEnumerator FadeLightIntensity(float target, float nSeconds)
    {
        var current = backgroundLight.intensity;
        if (nSeconds <= 0) nSeconds = 1;
        while (current != target)
        {
            current = Mathf.MoveTowards(current, target, Time.deltaTime / nSeconds);
            backgroundLight.intensity = current;
            yield return 0;
        }
    }

    void deleteRequiredItems()
    {
        isActive = false;
        foreach (GameObject iconInstance in requiredItensIcons)
        {
            Destroy(iconInstance);
        }
        requiredItensIcons.Clear();
    }
}
