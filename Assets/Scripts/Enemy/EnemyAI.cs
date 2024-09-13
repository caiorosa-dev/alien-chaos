using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float shootingRange = 5f;
    public float stopDistance = 3f;
    public float fireRate = 1f;
    public float avoidanceForce = 5f;
    public float avoidanceDistance = 2f;
    public float bulletSpeed = 20f;
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public int rayCount = 9;

    private Transform player;
    private float nextFireTime = 0f;

    void Start()
    {
        // Encontra o jogador pela tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            MoveTowardsPlayer();
        }

        if (distanceToPlayer <= shootingRange && Time.time >= nextFireTime)
        {
            foreach (Transform point in firePoints)
            {
                Shoot(point);
            }
            nextFireTime = Time.time + fireRate;
        }

        RotateTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        Vector2 avoidanceDirection = Vector2.zero;

        Vector2 finalDirection = (directionToPlayer + avoidanceDirection).normalized;

        transform.Translate(finalDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    // Função para o inimigo atirar
    void Shoot(Transform point)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, point.position, point.rotation);

        Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRb.velocity = point.up * bulletSpeed;
    }

    Vector2 AvoidObstacles()
    {
        Vector2 avoidanceDirection = Vector2.zero;

        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = i * angleStep;
            Vector2 rayDirection = DegreeToVector2(currentAngle);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, avoidanceDistance);
            if (hit.collider != null && hit.collider.gameObject.tag != "Enemy" && hit.collider.gameObject.tag != "EnemyBullet")
            {
                avoidanceDirection -= rayDirection * avoidanceForce;
            }
        }

        return avoidanceDirection;
    }

    Vector2 DegreeToVector2(float degree)
    {
        float radian = degree * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = player.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * moveSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = i * angleStep;
            Vector2 rayDirection = DegreeToVector2(currentAngle);
            //Gizmos.DrawRay(transform.position, rayDirection * avoidanceDistance);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootingRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, avoidanceDistance);
    }
}
