using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RouletteManager : MonoBehaviour
{
    public ScrollRect scrollRect;             // ScrollRect ����
    public Transform content;                 // ScrollRect�� Content
    public GameObject itemPrefab;             // �귿 ������ ������
    public List<RouletteData> rouletteDatas;             // �귿 ������ ������
    public List<RouletteUI> rouletteUis;             // �귿 ������ ������
    public int itemCount = 10;                // ������ ����
    public float scrollSpeed = 5f;            // �ʱ� ��ũ�� �ӵ�
    public float slowDownDuration = 2f;       // ���� �ð�
    public float offset = 20f;       // ���� �ð�

    private bool isSpinning = false;

    public Button spinButton;

    public GameObject selectedTarget;

    public Button autoScrollButton; // Button to toggle auto-scroll
    public float autoScrollSpeed = 50f; // Speed of auto-scroll
    private bool isAutoScrolling = false;

    void Start()
    {
        SetupRouletteItems();
        spinButton.onClick.AddListener(StartRoulette);
    }

    // �������� ScrollRect Content�� �߰�
    void SetupRouletteItems()
    {
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < rouletteDatas.Count; i++)
            {
                int index = i;
                GameObject item = Instantiate(itemPrefab, content);
                item.GetComponent<RouletteUI>().text.text = $"{rouletteDatas[index].dataName}";
                rouletteUis.Add(item.GetComponent<RouletteUI>());
            }

        }

    }

    [ContextMenu("StartRoulette")]
    public void StartRoulette()
    {
        if (isSpinning == false)
        {
            StartCoroutine(SpinRoulette(Random.Range(8f,12f)));
        }

        // Set up the auto-scroll button
        autoScrollButton.onClick.AddListener(ToggleAutoScroll);
    }

    public void SetAlign()
    {
<<<<<<< Updated upstream
        // If auto-scrolling is enabled, update the scroll position
        if (isAutoScrolling)
        {
            content.anchoredPosition += new Vector2(0, autoScrollSpeed * Time.deltaTime);
        }

        // Check the scroll position and reposition items if needed
        float newScrollPosition = content.anchoredPosition.y;

        // If scrolled past a certain point, reposition items
        if (newScrollPosition != scrollPosition)
=======

    }

    private IEnumerator SpinRoulette(float time)
    {
        isSpinning = true;

        float elapsedTime = 0f;
        slowDownDuration = time;
        float currentSpeed = scrollSpeed;

        // ��ũ�Ѹ� ����
        while (elapsedTime < slowDownDuration)
>>>>>>> Stashed changes
        {
            scrollRect.verticalNormalizedPosition -= currentSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            currentSpeed = Mathf.Lerp(scrollSpeed, 0, elapsedTime / slowDownDuration); // ���� �ӵ� ���̱�

<<<<<<< Updated upstream
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
=======
            // ��ũ�� ��ġ�� 0.1 ���Ϸ� �������� 0.9�� �缳�� (�ڿ������� ��ȯ)
            if (scrollRect.verticalNormalizedPosition <= 0.1f)
            {
                scrollRect.verticalNormalizedPosition = 0.9f;
>>>>>>> Stashed changes
            }

            yield return null;
        }


        float targetY = Mathf.Round(content.localPosition.y / 105f) * 105f;
        Debug.Log(targetY);

        // ���� ��ġ ����
        // DOTween�� ����Ͽ� ��ǥ ��ġ�� �̵��ϰ�, OnComplete�� ��ġ�� ����
        content.GetComponent<RectTransform>().DOAnchorPosY(targetY, 0.3f).SetEase(Ease.OutCirc);
        isSpinning = false;

        Utils.DelayCall(0.4f, () => 
        {
            Debug.Log(GetCenterItem(offset).GetComponent<RouletteUI>().text.text);
            selectedTarget = GetCenterItem(offset);
        });
    }
    GameObject GetCenterItem(float offset = 0f)
    {
        // Viewport�� �߾� World Position
        Vector3 centerPosition = scrollRect.viewport.position;
        centerPosition.y -= scrollRect.viewport.rect.height / 2 + offset; // ���ϴ� ������ �߰�

        float minDistance = float.MaxValue;
        GameObject closestItem = null;

        foreach (var item in rouletteUis)
        {
            // �� �׸��� World Position ���ϱ�
            Vector3 itemWorldPosition = item.transform.position;

            // Viewport�� �߾� ��ġ�� �׸� ��ġ�� �Ÿ� ���
            float distance = Vector3.Distance(centerPosition, itemWorldPosition);

            // �߾ӿ� ���� ����� �׸� ã��
            if (distance < minDistance)
            {
                minDistance = distance;
                closestItem = item.gameObject;
            }
        }

        return closestItem;
    }

    // Toggle auto-scroll on or off
    void ToggleAutoScroll()
    {
        isAutoScrolling = !isAutoScrolling;
    }
}
