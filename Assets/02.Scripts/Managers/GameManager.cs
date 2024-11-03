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

    public CanvasManager canvasManager;

    public float followerSpeedDecreaseAmount = 0.2f;

    private int lastFollowerCount = 0; // ���� follower�� ���� ����

    [ContextMenu("GetFollower")]
    public void GetFollower(Vector3 pos)
    {
        // follower�� �ν��Ͻ�ȭ�ϰ� ����Ʈ�� �߰�
        var newFollower = Instantiate(followerPrefab, pos, Quaternion.identity).GetComponent<Follower>();

        //newFollower.GetComponent<Collider2D>().isTrigger = true;
        newFollower.charIndex = Random.Range(0, 3);
        newFollower.Init();

        //Utils.DelayCall(2f, () => { newFollower.GetComponent<Collider2D>().isTrigger = false; });

         if(followers.Contains(newFollower) == false )followers.Add(newFollower);

        SetFollowersAlign();
    }

    internal void GetFollowerExit()
    {
        var target = followers[0];

        if(target.hasRandomBox == true)
        {
            rouletteManager.GetComponent<RouletteManager>().SetActive();
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
        if (followers.Contains(follower) == false)  followers.Add(follower);
        
        SetFollowersAlign();
        Debug.Log("Added Success");
    }

    private void Start()
    {
        redMoonRemainTime = redMoonMaxTime;
        PauseGame();
    }


    public void Update()
    {
        if (followers.Count != lastFollowerCount)
        {
            player.maxSpeed = player.originSpeed - (followerSpeedDecreaseAmount * followers.Count);
            lastFollowerCount = followers.Count; // follower ������ ������Ʈ
        }

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
            int index = i;
            followers[index].myIndex = index;

            if (index == 0)
            {
                // 0��° follower�� �÷��̾ Ÿ������ ����
                followers[index].target = player.transform;
            }
            else
            {
                // 1��°���ʹ� �ٷ� ���� follower�� Ÿ������ ����
                followers[index].target = followers[index - 1].transform;
            }

            followers[index].Init(); // �Ÿ� �ʱ�ȭ
        }
    }


    public void SetRedMoonState(float lastingTime)
    {
        // �����ΰ� ���� ����
        isRedMoonTime = true;
        player.SetWereWolfMask(true);


        Transform target = null;

         if(followers.Count > 0) target =  followers[0].transform;

        player.SetWereWolfState(target);

        List<Follower> tmpList = new List<Follower>();

        tmpList = followers;

        for (int i = 0; i < followers.Count; i++)
        {
            int index = i;
            tmpList[index].GetRunAway(player.transform);

            Debug.Log(index+"is Run Away");
        }

        Utils.DelayCall(lastingTime, () =>
        {
            for (int i = followers.Count -1 ; i >= 0; i--)
            {
                int index = i;
                if(tmpList[index] != null) tmpList[index].GetIdleState();
                Debug.Log(index + "is Get Idle");
            }

            redMoonRemainTime = redMoonMaxTime;
            isRedMoonTime = false;
            redMoonState.GetComponent<Image>().DOFade(0, 1f);
            player.SetWereWolfMask(false); // ���� ��



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

    [ContextMenu("PauseGame")]
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    [ContextMenu("ResumeGame")]
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}