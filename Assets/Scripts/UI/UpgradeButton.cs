using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum UpgradeOption
{
    health,
    damage,
    fireRate,
    speed,
    boostTime
}

public class UpgradeButton : MonoBehaviour
{
    public Upgrade upgradeItem;
    [SerializeField]
    GameObject inventorySlotUIObject;
    List<GameObject> InventorySlotsIntansce = new List<GameObject>();
    [SerializeField]
    UpgradeOption upgradeOption;

    private void Start()
    {
        switch (upgradeOption)
        {
            case UpgradeOption.health:
                upgradeItem = UpgradeManager.Instance.healthUpgrade;
                break;
            case UpgradeOption.damage:
                upgradeItem = UpgradeManager.Instance.damageUpgrade;
                break;
            case UpgradeOption.fireRate:
                upgradeItem = UpgradeManager.Instance.fireRateUpgrade;
                break;
            case UpgradeOption.speed:
                upgradeItem = UpgradeManager.Instance.speedUpgrade;
                break;
            case UpgradeOption.boostTime:
                upgradeItem = UpgradeManager.Instance.turboDurationUpgrade;
                break;
        }
    }

    public void OnMouseOver()
    {
        int i = 0;
        foreach (InventorySlot inventorySlot in upgradeItem.upgradeLevels[upgradeItem.levelIndex].requiredItems)
        {
            InventorySlotUI inventorySlotUI = inventorySlotUIObject.GetComponent<InventorySlotUI>();
            inventorySlotUI.slot = inventorySlot;
            GameObject newInventoryUI = Instantiate(inventorySlotUIObject);
            newInventoryUI.transform.parent = this.gameObject.transform;
            newInventoryUI.GetComponent<RectTransform>().localPosition = new Vector2(-70 + i * - 60, 0);

            InventorySlotsIntansce.Add(newInventoryUI);

            i++;
        }
    }
    public void OnMouseExit()
    {
        foreach (GameObject instance in InventorySlotsIntansce)
        {
            Destroy(instance);
        }
        InventorySlotsIntansce.Clear();
    }

    public void onMouseClick()
    {
        upgradeItem.DoUpgrade();
    }
}
