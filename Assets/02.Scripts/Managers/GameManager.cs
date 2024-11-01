using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public GameObject follower;

    public List<Follower> followers = new List<Follower>();

    [ContextMenu("GetPlayer")]
    public void GetFollower()
    {
        var newFollower = Instantiate(follower.GetComponent<Follower>());
        followers.Add(newFollower);
        SetFollowersAlign();
        //Instantiate(follower, player.transform);
    }

    public void SetFollowersAlign()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            int index = i;
            followers[index].myIndex = index;
            followers[index].target = player.transform;

        }
    }

}
