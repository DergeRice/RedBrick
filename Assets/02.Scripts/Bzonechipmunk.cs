using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bzonechipmunk : MonoBehaviour
{
    public float moveSpeed = 2f; // 몬스터의 이동 속도
    public float minX = -40f; // x축 최소값
    public float maxX = 0f; // x축 최대값
    public float minY = -10f; // y축 최소값
    public float maxY = 5f; // y축 최대값
    private Vector3 targetPosition;

   /// public Animator chpiMunk;


    void Start()
    {
        SetRandomTargetPosition();
    }

    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }
    }

    void SetRandomTargetPosition()
    {
        // x와 y축의 랜덤한 위치 설정
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        targetPosition = new Vector3(randomX, randomY, transform.position.z); // z값은 그대로 유지
    }
}
