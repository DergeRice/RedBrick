using System;
using System.Collections;
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
    public GameObject randomBoxObj, keydownButton;

    public Animator aChar, bChar, cChar;
    private Animator activeAnimator;

    public bool isRunningAway, isLostState, isNearChar;
    private Transform runAwayTarget;

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

        if (isRunningAway == true) return;

        transform.position = Vector3.Lerp(transform.position, behindTargetPosition, moveSpeed * Time.deltaTime);

        if(isNearChar && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.GetJoinFollower(this);
        }
    }

    public void GetRunAway(Transform target)
    {
        if (!isRunningAway && GameManager.Instance.player.isBinding == false)
        {
            isRunningAway = true;
            runAwayTarget = target; // ������ ��� ����

            GameManager.Instance.GetLostFollower(this);
            StartCoroutine(RunAwayCoroutine());
        }
    }

    private IEnumerator RunAwayCoroutine()
    {
        while (isRunningAway)
        {
            // ��� ��ġ���� ���� ��ġ�� ���� ���� ��� (�ݴ� ����)
            Vector3 directionAwayFromTarget = (transform.position - runAwayTarget.position + (Vector3.up * Random.Range(-1,1))).normalized;

            // �ݴ� �������� �̵�
            transform.position += directionAwayFromTarget * (moveSpeed*0.5f) * Time.deltaTime;

            if (directionAwayFromTarget.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (directionAwayFromTarget.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            yield return null; // ���� �����ӱ��� ���
        }
    }

    public void GetIdleState()
    {
        isRunningAway = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") == true)
        {
            keydownButton.SetActive(true);
            Debug.Log("tree");
            isNearChar = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            keydownButton.SetActive(false);
            
            isNearChar = false;
        }
    }
}