using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float attackInterval = 1.0f; // 공격 간격
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10.0f;
    private Vector3 moveDirection = Vector3.right * 2;
    private float moveDuration = 2.0f;
    private float nextAttackTime = 0f;

    private Animator animator;
    private ObjectPool arrowPool; // 화살을 위한 오브젝트 풀

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        arrowPool = FindObjectOfType<ObjectPool>(); // 화살용 ObjectPool 참조

        Move();
    }

    IEnumerator IdleToAttackRoutine()
    {
        while (true)
        {
            // 일정 시간 대기 후 Attack 상태로 전환
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackInterval);
            StartCoroutine(IdleToAttackRoutine()); // Idle로 돌아가기
        }
    }

    void Move()
    {
        // 플레이어를 지정된 위치로 이동시키고, 이동 완료 후 Idle 상태로 전환
        transform.DOMove(new Vector3(-1, -3.35f, 0), moveDuration, false)
            .OnComplete(() =>
            {
                // 이동이 끝난 후 Idle 상태로 전환
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
                StartCoroutine(IdleToAttackRoutine());
            });
        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
    }

    void Attack()
    {        
        GameObject arrow = arrowPool.GetObject(); // 오브젝트 풀에서 화살 가져오기
        arrow.transform.position = arrowSpawnPoint.position;

        // 다음 공격까지 대기 시간 설정
        nextAttackTime = Time.time + attackInterval;

        animator.SetBool("Idle", true);
    }
}
