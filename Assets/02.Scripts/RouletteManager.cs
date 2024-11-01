using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public RectTransform content; // The content object of the ScrollView
    public GameObject itemPrefab; // Prefab for the items
    public List<RouletteUI> rouletteUIs = new List<RouletteUI>();
    public int itemCount = 100; // Total number of items to display
    public float itemHeight = 30f; // Height of each item
    private float scrollPosition = 0f;

    void Start()
    {
        // Create the initial items
        for (int i = 0; i < itemCount; i++)
        {
            GameObject item = Instantiate(itemPrefab, content);
            rouletteUIs[i].text.text = $"{i} 번째 항목";
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * itemHeight);
            rouletteUIs.Add(item.GetComponent<RouletteUI>());
        }
    }

    void Update()
    {
       
        float newScrollPosition = content.anchoredPosition.y;

      
        if (newScrollPosition != scrollPosition)
        {
            float delta = newScrollPosition - scrollPosition;

            
            if (delta < 0)
            {
             
                if (content.anchoredPosition.y < -itemHeight)
                {
                  
                    RouletteUI firstItem = rouletteUIs[0];
                    rouletteUIs.RemoveAt(0);
                    firstItem.transform.SetAsLastSibling();
                    rouletteUIs.Add(firstItem);
                    firstItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -((itemCount - 1) * itemHeight));
                }
            }
            
            else if (delta > 0)
            {
               
                if (content.anchoredPosition.y > -itemCount * itemHeight + itemHeight)
                {
                   
                    RouletteUI lastItem = rouletteUIs[rouletteUIs.Count - 1];
                    rouletteUIs.RemoveAt(rouletteUIs.Count - 1);
                    lastItem.transform.SetAsFirstSibling();
                    rouletteUIs.Insert(0, lastItem);
                    lastItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                }
            }

            scrollPosition = newScrollPosition;
        }
    }
}
