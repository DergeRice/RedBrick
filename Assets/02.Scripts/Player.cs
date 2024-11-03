using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;
using TMPro;

public class Player : MonoBehaviour
{
    public Vector3 move;
    public float forceAmount, moveSpeed, maxSpeed, originSpeed;

    public GameObject wereWolfState, humanState, alertMsgBox,dustObject;

    public Animator wereWolfAnimator, humanAnimator;
    //private SpriteRenderer sp;

    public bool wereWolfmode = false;


    public bool isBinding = false;
    public bool isMovable = true;

    Coroutine coroutine;

    Transform followTarget;

    bool isKillable;

    private void Start()
    {
        moveSpeed = maxSpeed;
        originSpeed = maxSpeed;
    }

    private void Update()
    {
        if (isBinding) moveSpeed = 0;
        else moveSpeed = maxSpeed;

        if (maxSpeed > originSpeed) dustObject.SetActive(true);
        if (maxSpeed <= originSpeed) dustObject.SetActive(false);
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

        wereWolfmode = wear;

        if (wear == false)
        {
            StopAllCoroutines();
        }
    }

    public void SetWereWolfState(Transform target = null)
    {
        isKillable = true;

        if (target == null)
        {
            // Ÿ���� ���� ��� ������ ���ͷ� 0~2�ʸ��� ���� �����ϸ� �̵�
            coroutine = StartCoroutine(MoveRandomly());
        }
        else
        {
            // Ÿ���� ���� ��� �ش� Ÿ���� ���󰡰�, ���� �Ÿ� �ȿ� ������ �׼� �߻�
            Utils.DelayCall(2f, () =>
            {
                coroutine = StartCoroutine(FollowTarget(target));
            }
            );
        }
    }

    private IEnumerator MoveRandomly()
    {
        while (true)
        {
            float wereWolfMoveSpeed = isBinding ?  0: 8f;

            // 0~2���� ��� �ð� �� ������ ���� ����
            float waitTime = Random.Range(0f, 2f);
            yield return new WaitForSeconds(waitTime);

            // ������ ����� �ӵ��� �����Ͽ� �̵�
            Vector3 randomDirection = Random.insideUnitCircle.normalized * Random.Range(5f, 10f);
            Vector3 targetPosition = transform.position + randomDirection;

            // �̵�
            
            while ((transform.position - targetPosition).sqrMagnitude > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, wereWolfMoveSpeed * Time.deltaTime);

                if (wereWolfAnimator != null)
                {
                    wereWolfAnimator.SetBool("isMoving", true);
                }

                // �̵� ���⿡ ���� ȸ�� ���� (�����̸� y = 180, �������̸� y = 0)
                if (targetPosition.x - transform.position.x < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (targetPosition.x - transform.position.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                yield return null;
            }
        }
    }

    private IEnumerator FollowTarget(Transform target)
    {
        float closeDistance = 0.1f; // ��ǥ���� �ٰ��� �Ÿ�
        followTarget = target;

        while (followTarget != null && (transform.position - target.position).sqrMagnitude > closeDistance * closeDistance)
        {
            // Ÿ���� ���� �̵�
            float wereWolfMoveSpeed = isBinding ? 0 : 8f;
            transform.position = Vector3.MoveTowards(transform.position, target.position, wereWolfMoveSpeed * Time.deltaTime);

            if (wereWolfAnimator != null)
            {
                wereWolfAnimator.SetBool("isMoving", true);
            }

            // �̵� ���⿡ ���� ȸ�� ���� (�����̸� y = 180, �������̸� y = 0)
            if (target.position.x - transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (target.position.x - transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            yield return null;
        }

        // Ÿ�ٰ� ��������� Ư�� �׼� �߻�
        if (isKillable == true)
        {
            PerformActionNearTarget();
        }
    }

    private void PerformActionNearTarget()
    {
        isKillable = false;
        if(followTarget != null) followTarget.GetComponent<Follower>().GetDead();

        coroutine =  StartCoroutine(MoveRandomly());
    }

    public void ShowAlertMsg(string _text)
    {
        var msg = Instantiate(alertMsgBox);
        msg.transform.position = alertMsgBox.transform.position;
        msg.SetActive(true);
        msg.transform.localScale = Vector3.one * 0.003f;
        msg.transform.GetChild(0).GetComponent<TMP_Text>().text = _text;

        msg.transform.DOLocalMoveY(msg.transform.position.y + 2f, 2f).SetEase(Ease.OutCirc);
        Destroy(msg, 2f);
    }
}
