using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{

    private Vector2 direction; // ��Ʈ�� �̵� ����
    private static readonly float speed = 0.1f; // �̵� �ӵ�

    private void OnEnable()
    {
        // ������ ���� ��ġ ����
        transform.position = new Vector2(Random.Range(10f, 0f), Random.Range(0f, 10f)); // ���� ���� ����

        // ù ��° ��Ʈ�� ������ ����, �� ��° ��Ʈ���� ���� ����
        if (BatPoolManager.Instance.BatCount == 0)
        {
            direction = new Vector2(1f, 0f); // ���� ���� (��: ������)
        }
        else
        {
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // ���� ����
        }

        BatPoolManager.Instance.BatCount++; // �ν��Ͻ� ī��Ʈ ����
        StartCoroutine(ReturnToPoolAfterDelay());
    }

    private void FixedUpdate()
    {
        // ������ �������� �̵�
        transform.position += (Vector3)direction * speed; // �ӵ� ����
    }

    IEnumerator ReturnToPoolAfterDelay()
    {
        yield return new WaitForSeconds(5.0f); // 5�� ���
        BatPoolManager.Instance.ReturnBat(gameObject); // Ǯ�� ��ȯ
    }
}
