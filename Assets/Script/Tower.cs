using UnityEngine;
using UnityEngine.InputSystem;
public class Tower : MonoBehaviour
{
    public float range = 3.5f;
    public float fireRate = 1.5f;
    public int cost = 50;
    public LayerMask enemyLayer;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public bool isBuilt = false;

    private Transform target;
    private float fireCountdown = 0f;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.2f);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame && LastFrameToggled != Time.frameCount)
        {
            LastFrameToggled = Time.frameCount;
        }

        if (!isBuilt || target == null) return;

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private static int LastFrameToggled = -1;

    private void UpdateTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        Debug.Log("Target hits: " + hits.Length);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider2D hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearestEnemy = hit.transform;
            }
        }
        target = nearestEnemy;
    }

    private void Shoot()
    {
        Debug.Log($"Shoot called | isBuilt={isBuilt}, target={target}, proj={projectilePrefab}, firePoint={firePoint}");
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile proj = projGO.GetComponent<Projectile>();
            if (proj != null) proj.Seek(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}