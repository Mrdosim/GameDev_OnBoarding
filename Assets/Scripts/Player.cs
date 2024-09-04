using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float attackInterval = 1.0f; // ���� ����
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10.0f;
    private Vector3 moveDirection = Vector3.right * 2;
    private float moveDuration = 2.0f;
    private float nextAttackTime = 0f;

    private Animator animator;
    private ObjectPool arrowPool; // ȭ���� ���� ������Ʈ Ǯ

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        arrowPool = FindObjectOfType<ObjectPool>(); // ȭ��� ObjectPool ����

        Move();
    }

    IEnumerator IdleToAttackRoutine()
    {
        while (true)
        {
            // ���� �ð� ��� �� Attack ���·� ��ȯ
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackInterval);
            StartCoroutine(IdleToAttackRoutine()); // Idle�� ���ư���
        }
    }

    void Move()
    {
        // �÷��̾ ������ ��ġ�� �̵���Ű��, �̵� �Ϸ� �� Idle ���·� ��ȯ
        transform.DOMove(new Vector3(-1, -3.35f, 0), moveDuration, false)
            .OnComplete(() =>
            {
                // �̵��� ���� �� Idle ���·� ��ȯ
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
                StartCoroutine(IdleToAttackRoutine());
            });
        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
    }

    void Attack()
    {        
        GameObject arrow = arrowPool.GetObject(); // ������Ʈ Ǯ���� ȭ�� ��������
        arrow.transform.position = arrowSpawnPoint.position;

        // ���� ���ݱ��� ��� �ð� ����
        nextAttackTime = Time.time + attackInterval;

        animator.SetBool("Idle", true);
    }
}
