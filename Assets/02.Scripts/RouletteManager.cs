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

    public Button autoScrollButton; // Button to toggle auto-scroll
    public float autoScrollSpeed = 50f; // Speed of auto-scroll
    private bool isAutoScrolling = false;

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

        // Set up the auto-scroll button
        autoScrollButton.onClick.AddListener(ToggleAutoScroll);
    }

    void Update()
    {
        // If auto-scrolling is enabled, update the scroll position
        if (isAutoScrolling)
        {
            content.anchoredPosition += new Vector2(0, autoScrollSpeed * Time.deltaTime);
        }

        // Check the scroll position and reposition items if needed
        float newScrollPosition = content.anchoredPosition.y;

        // If scrolled past a certain point, reposition items
        if (newScrollPosition != scrollPosition)
        {
            float delta = newScrollPosition - scrollPosition;

            // Move items up when scrolling down
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
            // Move items down when scrolling up
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

    // Toggle auto-scroll on or off
    void ToggleAutoScroll()
    {
        isAutoScrolling = !isAutoScrolling;
    }
}
