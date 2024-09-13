using System.Collections;
using UnityEngine;

public class EnemyDeathSystem : MonoBehaviour
{
    [Header("Settings")]
    public int lifes = 3;
    public ExplosionTrigger explosionTrigger;

    [Space]
    [Header("Hitstop")]
    public bool triggerHitstopOnDeath = false;
    public float hitstopDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Collider2D objectCollider2D;
    private EnemyAI enemyAi;
    private Rigidbody2D rb;

    private DamageFlash damageFlash;
    [SerializeField]
    GameObject heal;

    // Start is called before the first frame update
    void Start()
    {
        damageFlash = GetComponent<DamageFlash>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectCollider2D = GetComponent<Collider2D>();
        enemyAi = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();

        explosionTrigger.onEnd += () =>
        {
            Destroy(this.gameObject);
        };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            return;
        }

        damageFlash.TriggerDamageFlash();
        CameraShake.Instance.ShakeCamera();

        lifes--;

        if (lifes <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Destroy(spriteRenderer);
        Destroy(objectCollider2D);
        Destroy(enemyAi);
        Destroy(rb);
        if (Random.Range(0,100) > 95)
        {
            Instantiate(heal).transform.position = this.transform.position;
        }

        explosionTrigger.TriggerExplosion();
        WaveManager.Instance.OnEnemyDespawn();
        if (triggerHitstopOnDeath)
        {
            HitStop.Instance.TriggerHitstop(hitstopDuration);
        }
    }

    public void TriggerDeath()
    {
        OnDeath();
    }
}
