using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData; // monsterData 변수를 추가
    public int maxHealth;
    public int currentHealth;
    public float moveSpeed;
    private MonsterSpawner spawner;
    private MonsterHealthBar healthBar;

    public void Initialize(MonsterData data)
    {
        monsterData = data; // monsterData 초기화
        maxHealth = data.Health;
        currentHealth = maxHealth;
        moveSpeed = data.Speed;
        spawner = FindObjectOfType<MonsterSpawner>();
        healthBar = GetComponentInChildren<MonsterHealthBar>();
        healthBar.SetHealth(maxHealth); // 초기 체력 설정
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth); // 체력바 업데이트
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        spawner.OnMonsterDie(gameObject); // 스폰 제어로 몬스터 죽음 알림
    }

    void Update()
    {
        // 몬스터 이동
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }
}
