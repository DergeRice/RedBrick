using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatPooling : MonoBehaviour
{

    private static Vector2 direction; // 정적 필드로 방향 설정 (모든 인스턴스가 공유)
    private static readonly float speed = 0.1f; // 이동 속도
    private static int batCount = 0; // 배트 인스턴스 카운트

    private void OnEnable()
    {
        // 랜덤한 생성 위치 설정
        transform.position = new Vector2(Random.Range(10f, 0f), Random.Range(0f, 10f)); // 생성 범위 설정

        // 첫 번째 배트는 고정된 방향, 두 번째 배트부터 랜덤 방향
        if (batCount == 0)
        {
            direction = new Vector2(1f, 0f); // 고정 방향 (예: 오른쪽)
        }
        else
        {
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // 랜덤 방향
        }

        batCount++; // 인스턴스 카운트 증가
        StartCoroutine("DisableObject");
    }

    private void FixedUpdate()
    {
        // 지정된 방향으로 이동
        transform.position += (Vector3)direction * speed; // 속도 적용
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(5.2f);
       
        gameObject.SetActive(false);
        Destroy(gameObject);

    }
    /*private Vector3 direction;
    private void OnEnable()
    {
        transform.position = new Vector2(20, 10);
        StartCoroutine("DisableObject");
    }

    private void Start()
    {
        direction = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        StartCoroutine("DisableObject");
    }

    private void FixedUpdate()
    {
        transform.position += direction;
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(6.0f);
        gameObject.SetActive(false);
    }*/
}
