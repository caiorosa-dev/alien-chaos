using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float normalMovementSpeed = 10f;
    [SerializeField] private float turboMovementSpeed = 15f;

    private float desacelerationSpeed = 30f;

    [Space]

    [Header("Turbo Settings")]
    [SerializeField] private float turboDuration = 7.5f;
    [SerializeField] private float turboRecuperationRate = 1.5f;
    [SerializeField] private KeyCode activationKey;
    [SerializeField] private GameObject BoostBarObject;
    private float movementSpeed;
    private float currentTurboTime;
    private bool isTurboActive;
    private Image BoostBarImage;

    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private HealthBar healthBar;

    void Start()
    {
        healthBar = GetComponent<HealthBar>();
        healthBar.onDeath += () =>
        {
            AudioManager.Instance.HandleGameStateChange(GameState.Explore);
            AudioManager.Instance.Play(ClipType.GameOver);
            MenuScripts.goToDeathScreen();
        };
        UpgradeManager.Instance.healthUpgrade.onUpgrade += (float newValue) =>
        {
            healthBar.maxHealth = (int)newValue;
            healthBar.fullHeal();
        };
        BoostBarImage = BoostBarObject.GetComponent<Image>();
        rb = GetComponent<Rigidbody2D>();
        movementSpeed = normalMovementSpeed;
        currentTurboTime = turboDuration;
        UpgradeManager upgradeManager = UpgradeManager.Instance;
        upgradeManager.speedUpgrade.onUpgrade += (float newValue) =>
        {
            movementSpeed = newValue;
        };
        upgradeManager.turboDurationUpgrade.onUpgrade += (float newValue) =>
        {
            currentTurboTime = newValue;
        };
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            healthBar.TakeDamage(1);
            AudioManager.Instance.Play(ClipType.PlayerHit);
            Destroy(collision.gameObject);
        }
    }

    void PointToMouse()
    {
        Vector3 mousePosition = Input.mousePosition;

        if (mousePosition.x < 0 || mousePosition.y < 0 || mousePosition.x > Screen.width || mousePosition.y > Screen.height)
        {
            return;
        }

        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = (worldMousePosition - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void HandleMovement()
    {
        float XMove = Input.GetAxis("Horizontal");
        float YMove = Input.GetAxis("Vertical");

        Vector2 targetVelocity = new Vector2(XMove, YMove) * movementSpeed;

        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, desacelerationSpeed * Time.fixedDeltaTime);
    }

    void HandleTurbo()
    {
        if (Input.GetKey(activationKey) && currentTurboTime > 0)
        {
            ActivateTurbo();
        }
        else
        {
            DeactivateTurbo();
        }

        if (!isTurboActive && currentTurboTime < turboDuration)
        {
            currentTurboTime += turboRecuperationRate * Time.deltaTime;
        }

        currentTurboTime = Mathf.Clamp(currentTurboTime, 0, turboDuration);
    }

    void Update()
    {
        BoostBarImage.fillAmount = currentTurboTime / turboDuration;
        HandleTurbo();
    }

    private void FixedUpdate()
    {
        PointToMouse();
        HandleMovement();
    }

    void ActivateTurbo()
    {
        isTurboActive = true;
        movementSpeed = turboMovementSpeed;

        currentTurboTime -= Time.deltaTime;
    }

    void DeactivateTurbo()
    {
        isTurboActive = false;
        movementSpeed = normalMovementSpeed;
    }
}
