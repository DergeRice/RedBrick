using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ExitZone : MonoBehaviour
{

    public TMP_Text zoneText, zoneMaxText;
    public Image zoneFillUI;
    public float zoneTime;
    public int count, zoneMaxCount;
    private float startTime;
    private float barTime;
    private bool isTrigger = false;

    private void Start()
    {
        zoneText.text = zoneTime.ToString();
        zoneMaxText.text = $"{count}/{zoneMaxCount}";
    }
    void Update()
    {
        if (isTrigger && startTime > 0f && count < zoneMaxCount && GameManager.Instance.followers.Count > 0)
        {
            startTime -= Time.deltaTime;
            barTime += Time.deltaTime;

            zoneText.text = startTime.ToString("F1");

            zoneFillUI.fillAmount = barTime / zoneTime;

            if (startTime <= 0)
            {
                count++;
                Debug.Log(count);
                zoneMaxText.text = $"{count}/{zoneMaxCount}";
                startTime = zoneTime;
                barTime = 0f;
                

                GameManager.Instance.GetFollowerExit();
                if(count >= zoneMaxCount)
                {
                    GetComponent<SpriteRenderer>().color = new Color32(90, 90, 90, 255);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTrigger )
        {
            if(GameManager.Instance.followers.Count <= 0)
            {
                GameManager.Instance.ShowCharMsg("내보낼 대상이 없어요!");
                return;
            }

            isTrigger = true;
            startTime = zoneTime;
        }
    }    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTrigger)
        {
            startTime = zoneTime;
            isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            isTrigger = false;
            barTime = 0f;
            zoneText.text = zoneTime.ToString("F1");
            zoneFillUI.fillAmount = 1f;

        }
    }


}
