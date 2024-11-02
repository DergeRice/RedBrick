using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public GameObject keydownButton;
    public Transform bindingPos;

    public bool isNearChar;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&& isNearChar)
        {
            GameManager.Instance.player.transform.position = bindingPos.transform.position;

            bool reverse = GameManager.Instance.player.isBinding;
            GameManager.Instance.player.isBinding = !reverse;
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            keydownButton.SetActive(true);
            Debug.Log("tree");
            isNearChar = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            keydownButton.SetActive(false);
            isNearChar = false;
        }
    }
}
