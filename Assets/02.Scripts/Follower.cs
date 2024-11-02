using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Follower : MonoBehaviour
{
    public int charIndex;
    public Transform target;               // �÷��̾� ��ġ
    public float baseDistance = 2f;        // �⺻ �Ÿ�
    public float moveSpeed = 5f;           // ���󰡴� �ӵ�
    public int myIndex = 0;                // follower�� �ε���, GameManager���� �Ҵ�

    private Vector3 targetPosition;
    private float followDistance;
    public bool hasRandomBox = false;
    public GameObject randomBoxObj;

    public Animator aChar, bChar, cChar;
    private Animator activeAnimator;

    public void Init()
    {
        // �ε����� ���� �Ÿ� ���
        followDistance = baseDistance * (myIndex + 1);
        activeAnimator = GetActiveAnimator();

        transform.GetChild(charIndex).gameObject.SetActive(true);

        if (Random.value < 0.1f)
        {
            hasRandomBox = true;
            randomBoxObj.SetActive(true);
        }
    }

    private Animator GetActiveAnimator()
    {
        // charIndex�� ���� Ȱ��ȭ�� Animator ����
        return charIndex switch
        {
            0 => aChar,
            1 => bChar,
            2 => cChar,
            _ => null,
        };
    }

    private void FixedUpdate()
    {
        // �÷��̾��� ��ġ���� ��ǥ ��ġ ���
        Vector3 targetDirection = (target.position - transform.position).normalized;
        Vector3 behindTargetPosition = target.position - targetDirection;

        // ��ǥ ��ġ�� �̵�
        Vector3 previousPosition = transform.position;
        transform.position = Vector3.Lerp(transform.position, behindTargetPosition, moveSpeed * Time.deltaTime);

        // ������ Ȯ�� �� �ִϸ��̼� ���� ������Ʈ
        bool isMoving = (transform.position - previousPosition).sqrMagnitude > 0.0001f;
        if (activeAnimator != null)
        {
            activeAnimator.SetBool("isMoving", isMoving);
        }

        // �̵� ���⿡ ���� ȸ�� ���� (�����̸� y = 180, �������̸� y = 0)
        if (targetDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (targetDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void GetRunAway()
    {
        
    }
}