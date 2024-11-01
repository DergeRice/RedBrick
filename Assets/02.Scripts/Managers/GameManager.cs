using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;                      // �÷��̾ ���� ����
    public GameObject followerPrefab;          // follower ������

    public List<Follower> followers = new List<Follower>();

    public GameObject redMoonState;

    public float redMoonRemainTime, redMoonMaxTime, redMoonLastingTime;

    public bool isRedMoonTime;

    [ContextMenu("GetFollower")]
    public void GetFollower()
    {
        // follower�� �ν��Ͻ�ȭ�ϰ� ����Ʈ�� �߰�
        var newFollower = Instantiate(followerPrefab, player.transform.position, Quaternion.identity).GetComponent<Follower>();
        followers.Add(newFollower);
        SetFollowersAlign();
    }

    private void Start()
    {
        redMoonRemainTime = redMoonMaxTime;
    }

    public void Update()
    {
        if(isRedMoonTime == false) redMoonRemainTime -= Time.deltaTime;

        if(redMoonRemainTime < 0)
        {
            redMoonRemainTime = redMoonMaxTime;
            SetRedMoonState(redMoonLastingTime);
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
        redMoonState.SetActive(true);
        isRedMoonTime = true;
        Utils.DelayCall(lastingTime, () => 
        { 
            redMoonState.SetActive(false);
            redMoonRemainTime = redMoonMaxTime;
            isRedMoonTime = false;
        });
    }
}