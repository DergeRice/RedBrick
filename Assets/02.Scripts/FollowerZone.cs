using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FollowerZone : MonoBehaviour
{
    public TMP_Text zoneText;
    public Image zoneFillUI;
    public float zoneTime;
    public int count;
    private float startTime;
    private float barTime;
    private bool isTrigger = false;

    private void Start()
    {
        zoneText.text = zoneTime.ToString();
    }
    void Update()
    {
        if (isTrigger && startTime > 0f)
        {
            startTime -= Time.deltaTime;
            barTime += Time.deltaTime;

            zoneText.text = startTime.ToString("F1");

            zoneFillUI.fillAmount = barTime / zoneTime;

            if (startTime <= 0)
            {
                count++;
                Debug.Log(count);
                GameManager.Instance.GetFollower(transform.position);
                startTime = zoneTime;
                barTime = 0f;

            }  
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player")&& !isTrigger)
        {
            isTrigger = true;
            startTime = zoneTime;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if(other.CompareTag("Player"))
        {
            isTrigger = false;
            barTime = 0f;
            zoneText.text = zoneTime.ToString("F1");
            zoneFillUI.fillAmount = barTime;

        }
    }
   
}

