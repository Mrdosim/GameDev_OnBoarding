using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 100;
    public float maxX = 3f;

    private ObjectPool pool; // 오브젝트 풀 참조

    void Start()
    {
        pool = GameObject.Find("ArrowPool").GetComponent<ObjectPool>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > maxX)
        {
            pool.ReturnObject(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 적과 충돌했을 때
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage); // 몬스터에게 데미지를 입힘
            }

            // 화살이 적에게 충돌한 후 풀로 반환
            pool.ReturnObject(gameObject);
        }

        // 다른 오브젝트와 충돌해도 화살을 풀로 반환
        pool.ReturnObject(gameObject);
    }
}
