using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TMP_Text resquedCount;
    public TMP_Text killedCount;

    public int totalRescuedCount = 0;
    public int totalKilledCount = 0;

    public Transform followerIndicatorParent;
    public List<Image> followerIndicator = new List<Image>();

    public GameManager gameManager;

    public Color disabledColor;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void Update()
    {
        for (int i = 0; i < gameManager.maxFollowers; i++)
        {
            int index = i;
            followerIndicatorParent.GetChild(index).gameObject.SetActive(true);
        }

        for (int i = 0; i < followerIndicator.Count; i++)
        {
            int index = i;
            if (index >= gameManager.followers.Count)
            {
                followerIndicator[index].color = disabledColor;
            }
            else
            {
                followerIndicator[index].color = Color.white;
            }

            
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        resquedCount.text = $"x {totalRescuedCount.ToString()}";
        killedCount.text = $"x {totalKilledCount.ToString()}";


    }
}
