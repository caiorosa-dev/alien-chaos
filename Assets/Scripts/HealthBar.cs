using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image healthBarImage;
    [SerializeField]
    private Canvas displayCanvas;
    [SerializeField]
    bool onGameHealthBar = true;

    public delegate void OnDeath();
    public OnDeath onDeath;
    public int maxHealth = 50;
    private float health;
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        this.health -= amount;
        if (this.health <= 0)
        {
            if (onDeath != null)
            {
                onDeath();
            }
        }

        healthBarImage.fillAmount = (health / maxHealth);
    }

    public void Heal(int ammout)
    {
        this.health += ammout;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBarImage.fillAmount = (health / maxHealth);
    }

    public void fullHeal()
    {
        this.health = maxHealth;
        healthBarImage.fillAmount = (health / maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (onGameHealthBar)
        {

            transform.rotation = Camera.main.transform.rotation;

            if (health == maxHealth || health <= 0)
            {
                displayCanvas.enabled = false;
            }
            else
            {
                displayCanvas.enabled = true;
            }
        }
    }
}
