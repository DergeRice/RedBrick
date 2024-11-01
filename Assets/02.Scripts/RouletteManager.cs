using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteManager : MonoBehaviour
{

    public List<RouletteUI> rouletteUIs = new List<RouletteUI>(); 
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rouletteUIs.Count; i++)
        {
            int index = i;
            rouletteUIs[index].text.text = $" {index} 번째 항목 ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
