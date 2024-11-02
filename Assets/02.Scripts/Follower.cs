using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Follower : MonoBehaviour
{
    public int charIndex;
    public Transform target;               // 플레이어 위치
    public float baseDistance = 2f;        // 기본 거리
    public float moveSpeed = 5f;           // 따라가는 속도
    public int myIndex = 0;                // follower의 인덱스, GameManager에서 할당

    private Vector3 targetPosition;
    private float followDistance;
    public bool hasRandomBox = false;
    public GameObject randomBoxObj, keydownButton;

    public Animator aChar, bChar, cChar;
    private Animator activeAnimator;

    public bool isRunningAway, isLostState, isNearChar;
    private Transform runAwayTarget;

    public void Init()
    {
        // 인덱스에 따라 거리 계산
        followDistance = baseDistance * (myIndex + 1);
        activeAnimator = GetActiveAnimator();

        transform.GetChild(charIndex).gameObject.SetActive(true);

        if (Random.value < 0.1f)
        {
            hasRandomBox = true;
            randomBoxObj.SetActive(true);
        }
    }

    private Animator GetActiveAnimator()
    {
        // charIndex에 따라 활성화할 Animator 선택
        return charIndex switch
        {
            0 => aChar,
            1 => bChar,
            2 => cChar,
            _ => null,
        };
    }

    private void FixedUpdate()
    {

        // 플레이어의 위치에서 목표 위치 계산
        Vector3 targetDirection = (target.position - transform.position).normalized;
        Vector3 behindTargetPosition = target.position - targetDirection;

        // 목표 위치로 이동
        Vector3 previousPosition = transform.position;

        // 움직임 확인 및 애니메이션 상태 업데이트
        bool isMoving = (transform.position - previousPosition).sqrMagnitude > 0.0001f;
        if (activeAnimator != null)
        {
            activeAnimator.SetBool("isMoving", isMoving);
        }

        // 이동 방향에 따른 회전 설정 (왼쪽이면 y = 180, 오른쪽이면 y = 0)
        if (targetDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (targetDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (isRunningAway == true) return;

        transform.position = Vector3.Lerp(transform.position, behindTargetPosition, moveSpeed * Time.deltaTime);

        if(isNearChar && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.GetJoinFollower(this);
        }
    }

    public void GetRunAway(Transform target)
    {
        if (!isRunningAway && GameManager.Instance.player.isBinding == false)
        {
            isRunningAway = true;
            runAwayTarget = target; // 도망갈 대상 설정

            GameManager.Instance.GetLostFollower(this);
            StartCoroutine(RunAwayCoroutine());
        }
    }

    private IEnumerator RunAwayCoroutine()
    {
        while (isRunningAway)
        {
            // 대상 위치에서 현재 위치로 가는 방향 계산 (반대 방향)
            Vector3 directionAwayFromTarget = (transform.position - runAwayTarget.position + (Vector3.up * Random.Range(-1,1))).normalized;

            // 반대 방향으로 이동
            transform.position += directionAwayFromTarget * (moveSpeed*0.5f) * Time.deltaTime;

            if (directionAwayFromTarget.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (directionAwayFromTarget.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            yield return null; // 다음 프레임까지 대기
        }
    }

    public void GetIdleState()
    {
        isRunningAway = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") == true)
        {
            keydownButton.SetActive(true);
            Debug.Log("tree");
            isNearChar = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            keydownButton.SetActive(false);
            
            isNearChar = false;
        }
    }
}