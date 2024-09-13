using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTurret : MonoBehaviour
{
    public float detectionRadius = 10f;  // Raio de detec��o da turret
    public float shootingRange = 8f;     // Raio de tiro da turret
    public LayerMask enemyLayer;         // Layer onde os inimigos est�o
    public float rotationSpeed = 5f;     // Velocidade de rota��o da turret

    public GameObject bulletPrefab;      // Prefab da bala
    public Transform[] firePoint;          // Ponto de onde as balas s�o disparadas
    public float bulletSpeed = 10f;      // Velocidade das balas
    public float fireRate = 1f;          // Taxa de tiro
    private float nextFireTime = 0f;     // Tempo at� o pr�ximo disparo

    private Transform targetEnemy;       // Refer�ncia para o inimigo mais pr�ximo

    void Update()
    {
        // Atualizar o alvo mais pr�ximo a cada frame
        FindClosestEnemy();

        // Se houver um inimigo dentro do raio de detec��o, rotacionar a turret
        if (targetEnemy != null)
        {
            RotateTurretTowardsEnemy();

            // Calcular a dist�ncia at� o inimigo
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.position);

            // Se o inimigo estiver no raio de tiro e for o momento certo, disparar
            if (distanceToEnemy <= shootingRange && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void FindClosestEnemy()
    {
        // Encontrar todos os inimigos dentro do raio de detec��o usando um overlap circle
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        float closestDistance = Mathf.Infinity;
        targetEnemy = null;

        // Percorrer todos os inimigos encontrados e verificar qual est� mais perto
        foreach (Collider2D enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                targetEnemy = enemy.transform;
            }
        }
    }

    void RotateTurretTowardsEnemy()
    {
        // Calcular a dire��o para o inimigo mais pr�ximo
        Vector2 direction = targetEnemy.position - transform.position;

        // Calcular o �ngulo que a turret deve rotacionar
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplicar a rota��o suavemente na dire��o do inimigo
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        AudioManager.Instance.Play(ClipType.TurretShoot);
        foreach (Transform point in firePoint)
        {
            // Instanciar a bala no ponto de disparo
            GameObject bulletInstance = Instantiate(bulletPrefab, point.position, point.rotation);

            // Adicionar velocidade � bala
            Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
            bulletRb.velocity = point.up * bulletSpeed;
        }
    }

    // Fun��o opcional para visualizar o raio de detec��o e o raio de tiro no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
