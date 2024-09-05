using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour
{ 
    public MonsterData monsterData;
    public int maxHealth;
    public int currentHealth;
    public float moveSpeed;
    private MonsterSpawner spawner;

    private Animator animator;
    private bool isDead = false;

    public void Initialize(MonsterData data)
    {
        monsterData = data;
        maxHealth = data.Health;
        currentHealth = maxHealth;
        moveSpeed = data.Speed;
        spawner = FindObjectOfType<MonsterSpawner>();
        HealthBarManager.Instance.SetHealth(maxHealth, currentHealth);
    }

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Death", false);
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        HealthBarManager.Instance.UpdateHealth(currentHealth);
        DamageTextManager.Instance.ShowDamageText(transform.position+Vector3.up*3, damage);
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            animator.SetBool("Death", true);
        }
    }

    public void Die()
    {
        spawner.OnMonsterDie(gameObject);
    }

    public void Move()
    {
        Vector3 targetPosition = new Vector3(1, -3.55f, 0);

        float moveDuration = Vector3.Distance(transform.position, targetPosition) / moveSpeed;

        transform.DOMove(targetPosition, moveDuration, false)
            .OnComplete(() =>
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
            });

        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
    }
}
