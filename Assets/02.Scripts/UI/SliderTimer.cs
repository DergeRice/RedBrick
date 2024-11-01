using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{

    Slider sliderTimer;

    void Start()
    {
       sliderTimer = GetComponent<Slider>();

    }

    
    void Update()
    {
        if (sliderTimer.value > 0f)
        {
            sliderTimer.value -= Time.deltaTime;
           
            //Debug.Log(sliderTimer.value);
        }
        else
        {
            Debug.Log("error");
        }
    }
}
