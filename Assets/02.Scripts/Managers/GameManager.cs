using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;                      // 플레이어에 대한 참조
    public GameObject followerPrefab;          // follower 프리팹

    public List<Follower> followers = new List<Follower>();

    [ContextMenu("GetFollower")]
    public void GetFollower()
    {
        // follower를 인스턴스화하고 리스트에 추가
        var newFollower = Instantiate(followerPrefab, player.transform.position, Quaternion.identity).GetComponent<Follower>();
        followers.Add(newFollower);
        SetFollowersAlign();
    }

    public void SetFollowersAlign()
    {
        // 모든 follower의 인덱스와 타겟 설정
        for (int i = 0; i < followers.Count; i++)
        {
            followers[i].myIndex = i;

            if (i == 0)
            {
                // 0번째 follower는 플레이어를 타겟으로 설정
                followers[i].target = player.transform;
            }
            else
            {
                // 1번째부터는 바로 앞의 follower를 타겟으로 설정
                followers[i].target = followers[i - 1].transform;
            }

            followers[i].Init(); // 거리 초기화
        }
    }

}