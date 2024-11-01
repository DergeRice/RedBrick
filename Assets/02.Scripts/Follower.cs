using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target;            // �÷��̾��� ��ġ�� ����
    public float followDistance = 1.5f; // follower �� �Ÿ�
    public float moveSpeed = 5f;        // ���󰡴� �ӵ�
    public int myIndex = 0;             // follower�� �ε���, GameManager���� �Ҵ�

    private Transform followTarget;     // ���� ���

    private void Start()
    {
        // ù ��° follower�� �÷��̾�(target)�� ���󰡰�, �� ���� follower�� �� follower�� ����
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
        // ��ǥ ��ġ ���
        Vector3 targetPosition = followTarget.position - followTarget.forward * followDistance;

        // �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
