using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    public float damage = 25f;

    [Header("Hiệu ứng Nổ")]
    public float explosionRadius = 0f;
    public LayerMask enemyLayer;
    public GameObject explosionEffectPrefab;

    [Header("Hiệu ứng Lửa")]
    public float burnDPS = 0f;
    public float burnDuration = 0f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        if(explosionEffectPrefab != null)
        {
            GameObject effect =Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            DamageEnemy(target.GetComponent<Enemy>());
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                DamageEnemy(enemy);
            }
        }
    }

    private void DamageEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            if (burnDPS > 0f && burnDuration > 0f)
            {
                enemy.ApplyBurn(burnDPS, burnDuration);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}