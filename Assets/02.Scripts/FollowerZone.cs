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


    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("found player tag");
            StartCoroutine(CountDownText());
            StartCoroutine(FillCountUp());
        }
    }

    private IEnumerator CountDownText()
    {
        float startTime = zoneTime;

        while (startTime > 0)
        {  
            zoneText.text = startTime.ToString("F1");
            yield return new WaitForSeconds(1.0f); 
            startTime -= 1.0f; 
        }
 
        zoneText.text = "0";
    }
    private IEnumerator FillCountUp()
    {
        float startbar = 0f;

        while (startbar < zoneTime)
        {       
            zoneFillUI.fillAmount = startbar / zoneTime;

            yield return new WaitForSeconds(1.0f); 
            startbar += 1.0f; 
        }

        zoneFillUI.fillAmount = 1f;
    }
}

