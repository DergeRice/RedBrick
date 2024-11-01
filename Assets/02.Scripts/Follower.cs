using UnityEngine;


public class Follower : MonoBehaviour
{
    public Transform target;               // �÷��̾� ��ġ
    public float baseDistance = 2f;        // �⺻ �Ÿ�
    public float moveSpeed = 5f;           // ���󰡴� �ӵ�
    public int myIndex = 0;                // follower�� �ε���, GameManager���� �Ҵ�

    private Vector3 targetPosition;
    private float followDistance;

    public void Init()
    {
        // �ε����� ���� �Ÿ� ���
        followDistance = baseDistance * (myIndex + 1);
    }

    private void FixedUpdate()
    {
        // �÷��̾��� ��ġ���� ��ǥ ��ġ ���
        Vector3 targetDirection = (target.position - transform.position).normalized; // �÷��̾� ���� ���
        Vector3 behindTargetPosition = target.position - targetDirection ; // ��ǥ ��ġ�� �÷��̾��� ����

        // �ε巴�� ��ǥ ��ġ�� �̵�
        transform.position = Vector3.Lerp(transform.position, behindTargetPosition, moveSpeed * Time.deltaTime);
    }
}