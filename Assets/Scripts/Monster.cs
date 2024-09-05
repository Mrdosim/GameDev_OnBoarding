using DG.Tweening;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData; // monsterData 변수를 추가
    public int maxHealth;
    public int currentHealth;
    public float moveSpeed;
    private MonsterSpawner spawner;

    private Animator animator;

    public void Initialize(MonsterData data)
    {
        monsterData = data; // monsterData 초기화
        maxHealth = data.Health;
        currentHealth = maxHealth;
        moveSpeed = data.Speed;
        spawner = FindObjectOfType<MonsterSpawner>();
        HealthBarManager.Instance.SetHealth(maxHealth, currentHealth);
    }
    void OnEnable()
    {
        animator = GetComponent<Animator>();

        Move();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HealthBarManager.Instance.UpdateHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        spawner.OnMonsterDie(gameObject);
    }

    void Move()
    {
        transform.DOMove(new Vector3(1, -3.55f, 0), 2f, false)
            .OnComplete(() =>
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
            });
        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
    }

}
