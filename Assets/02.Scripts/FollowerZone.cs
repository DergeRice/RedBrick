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

    private float startTime;
    private float barTime;

    private bool isTrigger = false;

    void Update()
    {
        if (isTrigger && startTime > 0f)
        {
            startTime -= Time.deltaTime;
            barTime += Time.deltaTime;

            zoneText.text = startTime.ToString("F1");

            zoneFillUI.fillAmount = barTime / zoneTime;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player")&& !isTrigger)
        {
            Debug.Log("found Player");
            isTrigger = true;
            startTime = zoneTime;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("exited Player");
            isTrigger = false;
            barTime = 0f;
            zoneText.text = zoneTime.ToString("F1");
            zoneFillUI.fillAmount = barTime;

        }
    }
   
}

