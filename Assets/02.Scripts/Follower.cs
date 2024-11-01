using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target;            // 플레이어의 위치를 따라감
    public float followDistance = 1.5f; // follower 간 거리
    public float moveSpeed = 5f;        // 따라가는 속도
    public int myIndex = 0;             // follower의 인덱스, GameManager에서 할당

    private Transform followTarget;     // 따라갈 대상

    private void Start()
    {
        // 첫 번째 follower는 플레이어(target)를 따라가고, 그 외의 follower는 앞 follower를 따라감
        if (myIndex == 0)
        {
            followTarget = target;
        }
        else
        {
            followTarget = GameManager.Instance.followers[myIndex - 1].transform;
        }
    }

    private void FixedUpdate()
    {
        // 목표 위치 계산
        Vector3 targetPosition = followTarget.position - followTarget.forward * followDistance;

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
