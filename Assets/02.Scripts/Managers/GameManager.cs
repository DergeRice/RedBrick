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
    public Player player;                      // 플레이어에 대한 참조
    public GameObject followerPrefab;          // follower 프리팹

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

    private int lastFollowerCount = 0; // 이전 follower의 개수 저장

    [ContextMenu("GetFollower")]
    public void GetFollower(Vector3 pos)
    {
        // follower를 인스턴스화하고 리스트에 추가
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
        GameObject.Find("SettingBtn").SetActive(false);
    }


    public void Update()
    {
        if (followers.Count != lastFollowerCount)
        {
            player.maxSpeed = player.originSpeed - (followerSpeedDecreaseAmount * followers.Count);
            lastFollowerCount = followers.Count; // follower 개수를 업데이트
        }

        if (isRedMoonTime == false)
        { 
            redMoonRemainTime -= Time.deltaTime;
            redMoonCurLastTime = redMoonLastingTime;
        }
        if (isRedMoonTime == true) redMoonCurLastTime -= Time.deltaTime;
        if (redMoonCurLastTime < 0) redMoonCurLastTime = 0;


        if (isRedMoonTime == false) moonText.text = $"다음 월식 까지 \n {redMoonRemainTime.ToString("F1")} sec";
        else moonText.text = $"월식 종료까지 \n {redMoonCurLastTime.ToString("F1")} sec";

        // 남은 시간이 2초보다 작고 상태 변경이 아직 호출되지 않았다면
        if (redMoonRemainTime < 2f && isRedMoonTime == false)
        {
            StartFadeRedMoon(); // 원하는 함수 호출
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
        float duration = 2f; // 총 지속 시간
        float interval = 2f; // 소환 간격
        float elapsedTime = 0f; // 경과 시간 추적



        while (elapsedTime < duration)
        {
            // 매번 새로운 난수 생성
            var ranVec2 = Random.insideUnitCircle * Random.Range(0f, 3f);
            Vector3 ranVec3 = player.transform.position + new Vector3(ranVec2.x, ranVec2.y, 0);

            // 매번 새로운 파티클 생성
            GameObject particle = Instantiate(wolfParticles[Random.Range(0, wolfParticles.Count)], ranVec3, Quaternion.identity);
            Destroy(particle, 0.5f);

            yield return new WaitForSeconds(interval); // 간격 기다림
            elapsedTime += interval; // 경과 시간 증가
        }
    }

    public void SetFollowersAlign()
    {
        
        // 모든 follower의 인덱스와 타겟 설정
        for (int i = 0; i < followers.Count; i++)
        {
            int index = i;
            followers[index].myIndex = index;

            if (index == 0)
            {
                // 0번째 follower는 플레이어를 타겟으로 설정
                followers[index].target = player.transform;
            }
            else
            {
                // 1번째부터는 바로 앞의 follower를 타겟으로 설정
                followers[index].target = followers[index - 1].transform;
            }

            followers[index].Init(); // 거리 초기화
        }
    }


    public void SetRedMoonState(float lastingTime)
    {
        // 늑대인간 상태 시작
        isRedMoonTime = true;
        player.SetWereWolfMask(true);


        Transform target = null;

         if(followers.Count > 0) target =  followers[0].transform;

        player.SetWereWolfState(target);

        List<Follower> tmpList = new List<Follower>();

        tmpList = followers;

        SoundManager.Instance.PlaySound("howling",SoundType.SFX);

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
            player.SetWereWolfMask(false); // 늑인 끝



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