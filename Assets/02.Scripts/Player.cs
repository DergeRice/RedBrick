using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;
using System;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public Vector3 move;
    public float forceAmount, moveSpeed, maxSpeed;

    public GameObject wereWolfState, humanState;

    public Animator wereWolfAnimator, humanAnimator;
    //private SpriteRenderer sp;

    private bool wereWolfmode = false;

    public bool isBinding = false;
    public bool isMovable = true;

    Coroutine coroutine;

    private void Start()
    {
        moveSpeed = maxSpeed;
    }

    private void Update()
    {
        if (isBinding) moveSpeed = 0;
        else moveSpeed = maxSpeed;
    }

    private void FixedUpdate()
    {
        if (move.magnitude > 0)
        {
            wereWolfAnimator.SetBool("isMoving", true);
            humanAnimator.SetBool("isMoving", true);
        }
        else
        {
            wereWolfAnimator.SetBool("isMoving", false);
            humanAnimator.SetBool("isMoving", false);
        }

        if (isMovable == false) return;
        move.x = SimpleInput.GetAxis("Horizontal");
        move.y = SimpleInput.GetAxis("Vertical");


        if (Mathf.Abs(move.x) > 0)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            float newYRotation = move.x < 0 ? 180f : 0f; // y �� �� ����
            transform.rotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z);
        }

        

        transform.position = transform.position + (move * Time.deltaTime * moveSpeed);
    }

    public void SetWereWolfMask(bool wear) 
    {
        wereWolfState.SetActive(wear);
        humanState.SetActive(!wear);


        isMovable = !wear;

        if (wear == false)
        {
            StopCoroutine(coroutine);
        }
    }

    public void SetWereWolfState(Transform target = null)
    {
        if (target == null)
        {
            // Ÿ���� ���� ��� ������ ���ͷ� 0~2�ʸ��� ���� �����ϸ� �̵�
            coroutine = StartCoroutine(MoveRandomly());
        }
        else
        {
            // Ÿ���� ���� ��� �ش� Ÿ���� ���󰡰�, ���� �Ÿ� �ȿ� ������ �׼� �߻�
            coroutine = StartCoroutine(FollowTarget(target));
        }
    }

    private IEnumerator MoveRandomly()
    {
        while (true)
        {
            // 0~2���� ��� �ð� �� ������ ���� ����
            float waitTime = Random.Range(0f, 2f);
            yield return new WaitForSeconds(waitTime);

            // ������ ����� �ӵ��� �����Ͽ� �̵�
            Vector3 randomDirection = Random.insideUnitCircle.normalized * Random.Range(5f, 10f);
            Vector3 targetPosition = transform.position + randomDirection;

            // �̵�
            float wereWolfMoveSpeed = moveSpeed * 1.5f;
            while ((transform.position - targetPosition).sqrMagnitude > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, wereWolfMoveSpeed * Time.deltaTime);

                if (wereWolfAnimator != null)
                {
                    wereWolfAnimator.SetBool("isMoving", true);
                }

                // �̵� ���⿡ ���� ȸ�� ���� (�����̸� y = 180, �������̸� y = 0)
                if (targetPosition.x < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (targetPosition.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                yield return null;
            }
        }
    }

    private IEnumerator FollowTarget(Transform target)
    {
        float closeDistance = 2f; // ��ǥ���� �ٰ��� �Ÿ�

        while ((transform.position - target.position).sqrMagnitude > closeDistance * closeDistance)
        {
            // Ÿ���� ���� �̵�
            float moveSpeed = 4f;
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Ÿ�ٰ� ��������� Ư�� �׼� �߻�
        PerformActionNearTarget();
    }

    private void PerformActionNearTarget()
    {
        // ���� �Ÿ� �ȿ� �������� �� ������ ����
        Debug.Log("Target reached! Performing action.");
        // ��: ���� �ִϸ��̼�, ���� ���� ��
    }
}
