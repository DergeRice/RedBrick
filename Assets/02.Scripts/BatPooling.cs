using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatPooling : MonoBehaviour
{

    private static Vector2 direction; // ���� �ʵ�� ���� ���� (��� �ν��Ͻ��� ����)
    private static readonly float speed = 0.1f; // �̵� �ӵ�
    private static int batCount = 0; // ��Ʈ �ν��Ͻ� ī��Ʈ

    private void OnEnable()
    {
        // ������ ���� ��ġ ����
        transform.position = new Vector2(Random.Range(10f, 0f), Random.Range(0f, 10f)); // ���� ���� ����

        // ù ��° ��Ʈ�� ������ ����, �� ��° ��Ʈ���� ���� ����
        if (batCount == 0)
        {
            direction = new Vector2(1f, 0f); // ���� ���� (��: ������)
        }
        else
        {
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // ���� ����
        }

        batCount++; // �ν��Ͻ� ī��Ʈ ����
        StartCoroutine("DisableObject");
    }

    private void FixedUpdate()
    {
        // ������ �������� �̵�
        transform.position += (Vector3)direction * speed; // �ӵ� ����
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
