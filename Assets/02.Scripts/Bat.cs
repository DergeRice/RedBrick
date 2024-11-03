using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{

    private Vector2 direction; // 배트의 이동 방향
    private static readonly float speed = 0.1f; // 이동 속도

    private void OnEnable()
    {
        // 랜덤한 생성 위치 설정
        transform.position = new Vector2(Random.Range(10f, 0f), Random.Range(0f, 10f)); // 생성 범위 설정

        // 첫 번째 배트는 고정된 방향, 두 번째 배트부터 랜덤 방향
        if (BatPoolManager.Instance.BatCount == 0)
        {
            direction = new Vector2(1f, 0f); // 고정 방향 (예: 오른쪽)
        }
        else
        {
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // 랜덤 방향
        }

        BatPoolManager.Instance.BatCount++; // 인스턴스 카운트 증가
        StartCoroutine(ReturnToPoolAfterDelay());
    }

    private void FixedUpdate()
    {
        // 지정된 방향으로 이동
        transform.position += (Vector3)direction * speed; // 속도 적용
    }

    IEnumerator ReturnToPoolAfterDelay()
    {
        yield return new WaitForSeconds(5.0f); // 5초 대기
        BatPoolManager.Instance.ReturnBat(gameObject); // 풀에 반환
    }
}
