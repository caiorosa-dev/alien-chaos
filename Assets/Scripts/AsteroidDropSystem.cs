using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AsteroidDropSystem : MonoBehaviour
{
    [SerializeField]
    HealthBar healthBar;
    [SerializeField]
    public GameObject dropedObject = null;
    [SerializeField]
    public int dropQuantity = 1;
    [SerializeField]
    float dropChance = 100;
    [SerializeField]
    int MaxHealth;
    public ExplosionTrigger explosionTrigger;
    public float throwForce = 300f;

    private bool startedToDestroy;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D objectCollider2D;
    private DamageFlash damageFlash;
    public delegate void onDestroyCallbackType();
    public onDestroyCallbackType onDestroyCallback;
    
    public float spawnedTime;
    public float lastSeen = 0;
    private void OnDestroy()
    {
        if (onDestroyCallback != null)
        {
            onDestroyCallback();
        }
    }
    void Start()
    {
        healthBar.maxHealth = MaxHealth;
        healthBar.fullHeal();
        spawnedTime = Time.time;
        healthBar.onDeath += onDeath;
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectCollider2D = GetComponent<CircleCollider2D>();
        damageFlash = GetComponent<DamageFlash>();

        explosionTrigger.onEnd += () =>
        {
            Destroy(this.gameObject);
        };
    }

    private void onDeath()
    {
        if (startedToDestroy)
        {
            return;
        }

        AudioManager.Instance.Play(ClipType.AsteroidBreak);
        startedToDestroy = true;
        spriteRenderer.enabled = false;
        objectCollider2D.enabled = false;
        explosionTrigger.TriggerExplosion();
        HitStop.Instance.TriggerHitstop();
        AudioManager.Instance.Play(ClipType.AsteroidBreak);

        SpawnDrops();
    }

    private void SpawnDrops()
    {
        if (dropedObject != null)
        {
            for (int i = 0; i < dropQuantity; i++)
            {
                if (Random.Range(0, 100) <= dropChance)
                {
                    GameObject newInstance = Instantiate(dropedObject);
                    newInstance.transform.position = this.transform.position;
                    Rigidbody2D rb = newInstance.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.AddForce(new Vector2(Random.Range(rb.mass * -throwForce, rb.mass * throwForce), Random.Range(rb.mass * -throwForce, rb.mass * throwForce)));
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            CameraShake.Instance.ShakeCamera();
            healthBar.TakeDamage((int) UpgradeManager.Instance.damageUpgrade.currentValue);
            damageFlash.TriggerDamageFlash();
            Destroy(collision.gameObject);
        }
    }

    private void Update()
    {
        if (spriteRenderer.isVisible)
        {
            lastSeen = Time.time;
        }
        if (lastSeen == 0 && Time.time - spawnedTime < 5)
        {
            return;
        }

        if (Time.time - lastSeen > 10 && !spriteRenderer.isVisible)
        {
            Destroy(gameObject);
        };
    }
}
