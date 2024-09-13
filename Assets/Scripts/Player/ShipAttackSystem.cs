using UnityEngine;

public class ShipAttackSystem : MonoBehaviour
{
    public GameObject shipBullet;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.1f;
    private float nextFireTime = 0f;
    // Start is called before the first frame update
    private void Start()
    {
        UpgradeManager.Instance.fireRateUpgrade.onUpgrade += (float newValue) =>
        {
            fireRate = newValue;
        };
        WaveManager.Instance.onWaveSuccess += () =>
        {
            GetComponent<HealthBar>().fullHeal();
        };
    }

    void Shoot()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            GameObject bulletInstance = Instantiate(shipBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
            bulletRb.velocity = bulletSpawnPoint.up * bulletSpeed;

            AudioManager.Instance.Play(ClipType.PlayerShoot);
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }
}
