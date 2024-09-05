using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 100;
    public float maxX = 3f;

    public Rigidbody2D rb;


    void Update()
    {
        rb.velocity = Vector2.right * speed;

        if (transform.position.x > maxX)
        {
            ObjectPool.Instance.ReturnObject(gameObject, "Arrow"); // 화살을 풀에 반환
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            ObjectPool.Instance.ReturnObject(gameObject, "Arrow"); // 충돌 후 화살 반환
        }
    }
}
