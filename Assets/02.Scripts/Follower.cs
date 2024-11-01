using UnityEngine;


public class Follower : MonoBehaviour
{
    public Transform target;               // 플레이어 위치
    public float baseDistance = 2f;        // 기본 거리
    public float moveSpeed = 5f;           // 따라가는 속도
    public int myIndex = 0;                // follower의 인덱스, GameManager에서 할당

    private Vector3 targetPosition;
    private float followDistance;

    public void Init()
    {
        // 인덱스에 따라 거리 계산
        followDistance = baseDistance * (myIndex + 1);
    }

    private void FixedUpdate()
    {
        // 플레이어의 위치에서 목표 위치 계산
        Vector3 targetDirection = (target.position - transform.position).normalized; // 플레이어 방향 계산
        Vector3 behindTargetPosition = target.position - targetDirection ; // 목표 위치는 플레이어의 뒤쪽

        // 부드럽게 목표 위치로 이동
        transform.position = Vector3.Lerp(transform.position, behindTargetPosition, moveSpeed * Time.deltaTime);
    }
}