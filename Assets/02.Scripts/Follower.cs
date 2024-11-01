using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public int myIndex;
    public Transform target;

    public float moveSpeed;

    public void Update()
    {
        transform.position = target.position + (Vector3.one * myIndex) * Time.deltaTime * moveSpeed;
    }

}
