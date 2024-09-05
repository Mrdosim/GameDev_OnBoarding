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
    private bool isDead = false;

    public void Initialize(MonsterData data)
    {
        monsterData = data;
        maxHealth = data.Health;
        currentHealth = maxHealth;
        moveSpeed = data.Speed; // 몬스터 데이터에서 속도 초기화
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
        // 몬스터가 이동할 목표 위치를 설정합니다.
        Vector3 targetPosition = new Vector3(1, -3.55f, 0);

        // 이동 시간을 moveSpeed에 비례하여 설정합니다.
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
