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
            float newYRotation = move.x < 0 ? 180f : 0f; // y 축 값 설정
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
            // 타겟이 없을 경우 임의의 벡터로 0~2초마다 방향 변경하며 이동
            coroutine = StartCoroutine(MoveRandomly());
        }
        else
        {
            // 타겟이 있을 경우 해당 타겟을 따라가고, 일정 거리 안에 들어오면 액션 발생
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

            // 0~2초의 대기 시간 후 랜덤한 방향 설정
            float waitTime = Random.Range(0f, 2f);
            yield return new WaitForSeconds(waitTime);

            // 임의의 방향과 속도를 설정하여 이동
            Vector3 randomDirection = Random.insideUnitCircle.normalized * Random.Range(5f, 10f);
            Vector3 targetPosition = transform.position + randomDirection;

            // 이동
            
            while ((transform.position - targetPosition).sqrMagnitude > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, wereWolfMoveSpeed * Time.deltaTime);

                if (wereWolfAnimator != null)
                {
                    wereWolfAnimator.SetBool("isMoving", true);
                }

                // 이동 방향에 따른 회전 설정 (왼쪽이면 y = 180, 오른쪽이면 y = 0)
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
        float closeDistance = 0.1f; // 목표물에 다가설 거리
        followTarget = target;

        while (followTarget != null && (transform.position - target.position).sqrMagnitude > closeDistance * closeDistance)
        {
            // 타겟을 향해 이동
            float wereWolfMoveSpeed = isBinding ? 0 : 8f;
            transform.position = Vector3.MoveTowards(transform.position, target.position, wereWolfMoveSpeed * Time.deltaTime);

            if (wereWolfAnimator != null)
            {
                wereWolfAnimator.SetBool("isMoving", true);
            }

            // 이동 방향에 따른 회전 설정 (왼쪽이면 y = 180, 오른쪽이면 y = 0)
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

        // 타겟과 가까워지면 특정 액션 발생
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
