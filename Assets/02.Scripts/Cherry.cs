using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.player.wereWolfmode == false )
        {
            other.GetComponent<Player>().maxSpeed += 2;

            Utils.DelayCall(3f,() =>
            {
                other.GetComponent<Player>().maxSpeed -= 2;
            });
            transform.parent.GetComponent<CherryManager>().AteCherry();
            gameObject.SetActive(false);
        }


    }
}
