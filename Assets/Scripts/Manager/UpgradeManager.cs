using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeLevel
{
    public List<InventorySlot> requiredItems = new List<InventorySlot>();
    public float newPropertyLevel;
}

[System.Serializable]
public class Upgrade
{
    public string name;

    public float currentValue;
    public int levelIndex = 0;

    public List<UpgradeLevel> upgradeLevels;
    public delegate void UpgradeLevelDelegate(float newValue);
    public UpgradeLevelDelegate onUpgrade;

    public bool DoUpgrade()
    {
        if (levelIndex >= upgradeLevels.Count) return false;
        InventoryManager inventory = InventoryManager.Instance;
        UpgradeLevel currentUpgradeLevel = upgradeLevels[levelIndex];

        if (inventory.IfHasRemoveList(currentUpgradeLevel.requiredItems))
        {
            levelIndex ++;
            if (levelIndex > upgradeLevels.Count)
            {
                upgradeLevels = new List<UpgradeLevel>();
            }
            currentValue = currentUpgradeLevel.newPropertyLevel;
            AudioManager.Instance.Play(ClipType.Upgrade);
            if (onUpgrade != null)
            {
                onUpgrade(currentValue);
            }
            return true;
        }
        return false;
    }
}

public class UpgradeManager : Singleton<UpgradeManager>
{
    public Upgrade damageUpgrade;
    public Upgrade speedUpgrade;
    public Upgrade turboDurationUpgrade;
    public Upgrade fireRateUpgrade;
    public Upgrade healthUpgrade;
    public void HandleUpgradeDamage()
    {
        damageUpgrade.DoUpgrade();
    }
    public void HandleUpgradeSpeed()
    {
        speedUpgrade.DoUpgrade();
    }
    public void HandleUpgradeBostDuration()
    {
        turboDurationUpgrade.DoUpgrade();
    }
    public void HandleUpgradeFirerate()
    {
        fireRateUpgrade.DoUpgrade();
    }
    public void HandleUpgradeHealth()
    {
        healthUpgrade.DoUpgrade();
    }
}
