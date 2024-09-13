using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public bool isOre = true;
    public InventoryItem item;

    void Start()
    {
        if (isOre)
        {

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.sprite = item.icon;
        }
    }

    public void TriggerPickup()
    {
        Destroy(gameObject.GetComponent<Collider2D>());
        Animator animator = GetComponent<Animator>();

        animator.enabled = true;
        animator.Play("pickup");

        Invoke("OnPickupEnd", 0.8f);
    }

    public void OnPickupEnd()
    {
        Destroy(gameObject);
    }
}
