using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float attackInterval = 1.0f;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10.0f;

    private float moveDuration = 2.0f;

    public float attackRange = 2.0f;
    public LayerMask monsterLayer;

    private Animator animator;
    private ObjectPool arrowPool;

    void Start()
    {
        animator = GetComponent<Animator>();
        arrowPool = FindObjectOfType<ObjectPool>();

        Move();
    }

    private void Update()
    {
        DetectAndAttackMonsters();
    }

    IEnumerator IdleToAttackRoutine()
    {
        while (true)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackInterval);
        }
    }

    void DetectAndAttackMonsters()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, monsterLayer);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Monster"))
            {
                animator.SetTrigger("Attack");
                StartCoroutine(IdleToAttackRoutine());
                break;
            }
        }
    }

    void Move()
    {
        transform.DOMove(new Vector3(-1, -3.35f, 0), moveDuration, false)
            .OnComplete(() =>
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
            });
        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
    }

    void Attack()
    {
        GameObject arrow = arrowPool.GetObject("Arrow");
        arrow.transform.position = arrowSpawnPoint.position;

        animator.SetBool("Idle", true);
    }
}