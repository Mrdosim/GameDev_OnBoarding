using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 100;
    public float maxX = 3f;

    private ObjectPool pool;
    public Rigidbody2D rb;

    void Start()
    {
        pool = GameObject.Find("ArrowPool").GetComponent<ObjectPool>();
    }

    void Update()
    {
        rb.velocity = Vector2.right * speed;

        if (transform.position.x > maxX)
        {
            pool.ReturnObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적과 충돌했을 때
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            pool.ReturnObject(gameObject);
        }
    }
}
