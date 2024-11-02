using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bzonechipmunk : MonoBehaviour
{
    public float moveSpeed = 2f; // ������ �̵� �ӵ�
    public float minX = -40f; // x�� �ּҰ�
    public float maxX = 0f; // x�� �ִ밪
    public float minY = -10f; // y�� �ּҰ�
    public float maxY = 5f; // y�� �ִ밪
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
        // x�� y���� ������ ��ġ ����
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        targetPosition = new Vector3(randomX, randomY, transform.position.z); // z���� �״�� ����
    }
}
