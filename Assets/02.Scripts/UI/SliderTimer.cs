using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderTimer : MonoBehaviour
{
    public TMP_Text timeText;
    public float maxTime;

    public Slider sliderTimer;

    void Start()
    {
        sliderTimer = GetComponent<Slider>();
        sliderTimer.maxValue = maxTime;
        sliderTimer.value = maxTime;

    }


    void Update()
    {
        if (sliderTimer.value > 0f)
        {
            sliderTimer.value -= Time.deltaTime;
            timeText.text = sliderTimer.value.ToString("F1");
        }
    }
}
