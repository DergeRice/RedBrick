using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RouletteManager : MonoBehaviour
{
    public ScrollRect scrollRect;             // ScrollRect 참조
    public Transform content;                 // ScrollRect의 Content
    public GameObject itemPrefab;             // 룰렛 아이템 프리팹
    public List<RouletteData> rouletteDatas;             // 룰렛 아이템 프리팹
    public List<RouletteUI> rouletteUis;             // 룰렛 아이템 프리팹
    public int itemCount = 10;                // 아이템 개수
    public float scrollSpeed = 5f;            // 초기 스크롤 속도
    public float slowDownDuration = 2f;       // 감속 시간
    public float offset = 20f;       // 감속 시간

    private bool isSpinning = false;

    public Button spinButton;

    public GameObject selectedTarget;

    public Button autoScrollButton; // Button to toggle auto-scroll
    public float autoScrollSpeed = 50f; // Speed of auto-scroll
    private bool isAutoScrolling = false;

    void Start()
    {
        GameManager.Instance.rouletteManager = gameObject;
        SetupRouletteItems();
        spinButton.onClick.AddListener(StartRoulette);
    }

    // 아이템을 ScrollRect Content에 추가
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


    private IEnumerator SpinRoulette(float time)
    {
        isSpinning = true;

        float elapsedTime = 0f;
        slowDownDuration = time;
        float currentSpeed = scrollSpeed;

        // 스크롤링 시작
        while (elapsedTime < slowDownDuration)
        {
            scrollRect.verticalNormalizedPosition -= currentSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            currentSpeed = Mathf.Lerp(scrollSpeed, 0, elapsedTime / slowDownDuration); // 점차 속도 줄이기


            // 스크롤 위치가 0.1 이하로 내려가면 0.9로 재설정 (자연스러운 순환)
            if (scrollRect.verticalNormalizedPosition <= 0.1f)
            {
                scrollRect.verticalNormalizedPosition = 0.9f;

            }

            yield return null;
        }


        float targetY = Mathf.Round(content.localPosition.y / 105f) * 105f;
        Debug.Log(targetY);

        // 최종 위치 정렬
        // DOTween을 사용하여 목표 위치로 이동하고, OnComplete로 위치를 보정
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
        // Viewport의 중앙 World Position
        Vector3 centerPosition = scrollRect.viewport.position;
        centerPosition.y -= scrollRect.viewport.rect.height / 2 + offset; // 원하는 오프셋 추가

        float minDistance = float.MaxValue;
        GameObject closestItem = null;

        foreach (var item in rouletteUis)
        {
            // 각 항목의 World Position 구하기
            Vector3 itemWorldPosition = item.transform.position;

            // Viewport의 중앙 위치와 항목 위치의 거리 계산
            float distance = Vector3.Distance(centerPosition, itemWorldPosition);

            // 중앙에 가장 가까운 항목 찾기
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
