using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<InventorySlot> inventory = new List<InventorySlot>();
    public delegate void InventorySlotDelegate();
    public InventorySlotDelegate onInventoryChange;

    public void Add(InventoryItem item, int amount)
    {

        foreach (InventorySlot currentSlot in inventory)
        {
            if (currentSlot.item.id == item.id)
            {
                currentSlot.amount += 1;
                if (onInventoryChange != null)
                {
                    onInventoryChange();
                }
                return;
            }
        }

        InventorySlot newSlot = new InventorySlot(item, amount);

        inventory.Add(newSlot);
        if (onInventoryChange != null)
        {
            onInventoryChange();
        }
    }

    public void Remove(InventoryItem item, int amount)
    {

        foreach (InventorySlot currentSlot in inventory)
        {
            if (currentSlot.item.id == item.id)
            {
                currentSlot.amount -= amount;

                if (currentSlot.amount <= 0)
                {
                    inventory.Remove(currentSlot);
                }
                if (onInventoryChange != null)
                {
                    onInventoryChange();
                }
                return;
            }
        }
    }

    public bool Has(InventoryItem item, int amount)
    {
        foreach (InventorySlot currentSlot in inventory)
        {
            if (currentSlot.item.id == item.id && currentSlot.amount >= amount)
            {
                return true;
            }
        }

        return false;
    }

    public bool IfHasRemove(InventoryItem item, int amount)
    {
        if (Has(item, amount))
        {
            Remove(item, amount);
            return true;
        }

        return false;
    }

    public bool IfHasRemoveList(List<InventorySlot> list)
    {
        foreach(InventorySlot currentSlot in list)
        {
            if(!Has(currentSlot.item, currentSlot.amount))
            {
                return false;
            }
        }
        foreach (InventorySlot currentSlot in list)
        {
            Remove(currentSlot.item, currentSlot.amount);
        }
        return true;
    }
}
