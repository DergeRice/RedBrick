using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
    public GameObject randomBoxObj, keydownButton, deadImg;

    public Animator aChar, bChar, cChar;
    private Animator activeAnimator;

    public bool isRunningAway, isLostState, isNearChar;
    private Transform runAwayTarget;


    public List<GameObject> particles = new List<GameObject>();
    private Vector3 previousPosition;
    private bool InitCompleted = false;

    public void Init()
    {
        // �ε����� ���� �Ÿ� ���
        followDistance = baseDistance * (myIndex + 1);
        activeAnimator = GetActiveAnimator();

        transform.GetChild(charIndex).gameObject.SetActive(true);

        if (Random.value < 0.1f && InitCompleted == false)
        {
            hasRandomBox = true;
            randomBoxObj.SetActive(true);
        }
        InitCompleted = true;
    }

    private void Update()
    {


        if (isNearChar && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.GetJoinFollower(this);
            isLostState = false;
            isNearChar = false;
            moveSpeed = 5f;
            keydownButton.SetActive(false);
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
        bool isMoving = (transform.position - previousPosition).sqrMagnitude > 0.001f;

        if (activeAnimator != null)
        {
            activeAnimator.SetBool("isMoving", isMoving);
        }

        // ���� ��ġ�� previousPosition�� ������Ʈ
        previousPosition = transform.position;


        if (target == null) return;
        // �÷��̾��� ��ġ���� ��ǥ ��ġ ���
        Vector3 targetDirection = (target.position - transform.position).normalized;
        Vector3 behindTargetPosition = target.position - targetDirection;

        // ��ǥ ��ġ�� �̵�
        

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


    }

    public void GetRunAway(Transform target)
    {
        if (!isRunningAway && GameManager.Instance.player.isBinding == false)
        {
            isRunningAway = true;
            runAwayTarget = target; // ������ ��� ����

            StartCoroutine(RunAwayCoroutine());
        }
    }

    private IEnumerator RunAwayCoroutine()
    {
        while (isRunningAway)
        {
            // ��� ��ġ���� ���� ��ġ�� ���� ���� ��� (�ݴ� ����)
            Vector3 directionAwayFromTarget = (transform.position - runAwayTarget.position + (Vector3.up * Random.Range(-1, 1))).normalized;

            // �ݴ� �������� �̵�
            transform.position += directionAwayFromTarget * (moveSpeed * 0.5f) * Time.deltaTime;

            Vector3 previousPosition = transform.position;


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
        if (GameManager.Instance.player.isBinding == true) return;

        isRunningAway = false;
        moveSpeed = 0;
        isLostState = true;
        GameManager.Instance.GetLostFollower(this);
        StopCoroutine(RunAwayCoroutine());

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") == true && isLostState)
        {
            keydownButton.SetActive(true);
            isNearChar = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true && isLostState)
        {
            keydownButton.SetActive(false);

            isNearChar = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") == true && GameManager.Instance.player.isBinding == false && GameManager.Instance.player.wereWolfmode == true)
        {
            GetDead();
        }
    }

    public void GetDead()
    {
        isRunningAway = false;
        moveSpeed = 0;
        Debug.Log("Dead");

        transform.GetChild(charIndex).gameObject.SetActive(false);

        GameManager.Instance.followers.Remove(this);

        deadImg.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, particles.Count);

            var part = Instantiate(particles[randomIndex], transform.position + (Random.Range(-2f, 2f) * Vector3.one), Quaternion.identity);

            part.SetActive(true);
            part.transform.localScale = Vector3.one;
            part.transform.DOScale(2f, 1f).SetEase(Ease.InOutBounce);

            Destroy(part, 1f);
        }

        Utils.DelayCall(0.1f,()=>
        {
            GameManager.Instance.canvasManager.totalKilledCount++;

            Destroy(gameObject,2f);
        });
    }
}