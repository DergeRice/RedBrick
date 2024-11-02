using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public Player player;                      // �÷��̾ ���� ����
    public GameObject followerPrefab;          // follower ������

    public List<Follower> followers = new List<Follower>();

    public int maxFollowers = 3;

    public GameObject redMoonState;
    public List<GameObject> wolfParticles;
    public Image redMoonAmount;

    public float redMoonRemainTime, redMoonMaxTime, redMoonLastingTime, redMoonCurLastTime;

    public bool isRedMoonTime;

    public TMP_Text moonText;

    public GameObject rouletteManager;

    [ContextMenu("GetFollower")]
    public void GetFollower(Vector3 pos)
    {
        // follower�� �ν��Ͻ�ȭ�ϰ� ����Ʈ�� �߰�
        var newFollower = Instantiate(followerPrefab, pos, Quaternion.identity).GetComponent<Follower>();

        //newFollower.GetComponent<Collider2D>().isTrigger = true;
        newFollower.charIndex = Random.Range(0, 3);
        newFollower.Init();

        //Utils.DelayCall(2f, () => { newFollower.GetComponent<Collider2D>().isTrigger = false; });

        followers.Add(newFollower);
        SetFollowersAlign();
    }

    internal void GetFollowerExit()
    {
        var target = followers[0];

        if(target.hasRandomBox == true)
        {
            rouletteManager.SetActive(true);
        }
        followers.Remove(target);
        Destroy(target.gameObject);
        SetFollowersAlign();
    }

    public void GetLostFollower(Follower follower)
    {
        followers.Remove(follower);
    }

    public void GetJoinFollower(Follower follower)
    {
        followers.Add(follower);
        SetFollowersAlign();
    }

    private void Start()
    {
        redMoonRemainTime = redMoonMaxTime;
    }


    public void Update()
    {
        if (isRedMoonTime == false)
        { 
            redMoonRemainTime -= Time.deltaTime;
            redMoonCurLastTime = redMoonLastingTime;
        }
        if (isRedMoonTime == true) redMoonCurLastTime -= Time.deltaTime;
        if (redMoonCurLastTime < 0) redMoonCurLastTime = 0;


        if (isRedMoonTime == false) moonText.text = $"���� ���� ���� \n {redMoonRemainTime.ToString("F1")} sec";
        else moonText.text = $"���� ������� \n {redMoonCurLastTime.ToString("F1")} sec";

        // ���� �ð��� 2�ʺ��� �۰� ���� ������ ���� ȣ����� �ʾҴٸ�
        if (redMoonRemainTime < 2f && isRedMoonTime == false)
        {
            StartFadeRedMoon(); // ���ϴ� �Լ� ȣ��
        }

        if (redMoonRemainTime < 0)
        {
            redMoonRemainTime = redMoonMaxTime;
            SetRedMoonState(redMoonLastingTime);
        }

        if(isRedMoonTime == false) redMoonAmount.fillAmount = redMoonRemainTime / redMoonMaxTime;
    }

    private void StartFadeRedMoon()
    {
        redMoonState.SetActive(true);
        redMoonState.GetComponent<Image>().DOFade(0.5f, 2f);
        StartCoroutine(SpawnWolfParticles());

        Utils.DelayCall( 2f, () => { StopCoroutine(SpawnWolfParticles()); });
    }

    IEnumerator SpawnWolfParticles()
    {
        float duration = 2f; // �� ���� �ð�
        float interval = 2f; // ��ȯ ����
        float elapsedTime = 0f; // ��� �ð� ����



        while (elapsedTime < duration)
        {
            // �Ź� ���ο� ���� ����
            var ranVec2 = Random.insideUnitCircle * Random.Range(0f, 3f);
            Vector3 ranVec3 = player.transform.position + new Vector3(ranVec2.x, ranVec2.y, 0);

            // �Ź� ���ο� ��ƼŬ ����
            GameObject particle = Instantiate(wolfParticles[Random.Range(0, wolfParticles.Count)], ranVec3, Quaternion.identity);
            Destroy(particle, 0.5f);

            yield return new WaitForSeconds(interval); // ���� ��ٸ�
            elapsedTime += interval; // ��� �ð� ����
        }
    }

    public void SetFollowersAlign()
    {
        // ��� follower�� �ε����� Ÿ�� ����
        for (int i = 0; i < followers.Count; i++)
        {
            followers[i].myIndex = i;

            if (i == 0)
            {
                // 0��° follower�� �÷��̾ Ÿ������ ����
                followers[i].target = player.transform;
            }
            else
            {
                // 1��°���ʹ� �ٷ� ���� follower�� Ÿ������ ����
                followers[i].target = followers[i - 1].transform;
            }

            followers[i].Init(); // �Ÿ� �ʱ�ȭ
        }
    }


    public void SetRedMoonState(float lastingTime)
    {
        // �����ΰ� ���� ����
        isRedMoonTime = true;
        player.SetWereWolfMask(true);


        Transform target = followers == null ? followers[0].transform : null;

        player.SetWereWolfState(target);


        for (int i = 0; i < followers.Count; i++)
        {
            followers[i].GetRunAway(player.transform);
        }

        Utils.DelayCall(lastingTime, () =>
        {
            redMoonRemainTime = redMoonMaxTime;
            isRedMoonTime = false;
            redMoonState.GetComponent<Image>().DOFade(0, 1f);
            player.SetWereWolfMask(false); // ���� ��

            foreach (var item in followers)
            {
                item.GetIdleState();
            }

        });

        Utils.DelayCall(lastingTime + 1f, () =>
        {
            redMoonState.SetActive(false);

        });
    }

    public void ShowCharMsg(string text)
    {
        player.ShowAlertMsg(text);
    }
}