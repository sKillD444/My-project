using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public int dmgBase = 1;
    public int moneyReward = 10;

    public float moveSpeed = 2f;
    public Transform[] waypoints;

    public Transform healthBarfill;
    public bool hideHealBar=false;
    private Vector3 healthBarScale;

    private int currentWaypointIndex = 0;
    private bool isDead = false;

    [Header("DMG Lửa")]
    private float burnTime = 0f;
    private float burnDmg = 0f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) originalColor = spriteRenderer.color;
        if (healthBarfill != null)
        {
            healthBarScale = healthBarfill.localScale;
            UpdateHealthBar();
        }
    }

    private void Update()
    {
        MoveAlongWaypoints();
        BurnEffect();
    }

    private void MoveAlongWaypoints()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector2 targetPos = target.position;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPos) < 0.001f)
        {
            transform.position = targetPos;
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.TakeDamage(dmgBase);
                }
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if(isDead) return;

        currentHealth -= amount;
        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;

        if (GameManager.instance != null)
        {
            GameManager.instance.AddMoney(moneyReward);
        }

        Destroy(gameObject);
    }

    public void UpdateHealthBar()
    {
        if(healthBarfill == null) return;

        float healthPercentage = maxHealth>0?currentHealth/maxHealth:0;
        if(healthBarfill.parent != null &&hideHealBar)
        {
           healthBarfill.parent.gameObject.SetActive(healthPercentage>0f &&healthPercentage<1f);
        }

        healthBarfill.localScale = new Vector3(healthBarScale.x * healthPercentage, healthBarScale.y, healthBarScale.z);
    }

    private void BurnEffect()
    {
        burnTime -= Time.deltaTime;
        TakeDamage(burnDmg * Time.deltaTime);
        
        if(spriteRenderer != null)
        {
            if (burnTime > 0f)
            {
                spriteRenderer.color = Color.red;
            }
            else
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    public void ApplyBurn(float duration, float dps)
    {
        burnTime = duration;
        burnDmg = dps;
    }
}