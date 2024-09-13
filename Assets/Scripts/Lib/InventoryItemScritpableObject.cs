using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "InventoryItem", menuName="Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
}

[System.Serializable]
public class InventorySlot
{
    public InventorySlot(InventoryItem item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
    public InventoryItem item;
    public int amount;
}