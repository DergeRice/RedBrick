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
            StartCoroutine(CountDown());
        }
    }

    private IEnumerator CountDown()
    {
        float time = zoneTime;

        while (time > 0)
        {
            Debug.Log(time.ToString("F1"));
            zoneFillUI.fillAmount = time / zoneTime;
            yield return new WaitForSeconds(1f); 
            time -= 1f;
        }
    }
}

