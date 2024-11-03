using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SpawnBats());
    }

    private IEnumerator SpawnBats()
    {
        while (true)
        {
            GameObject bat = BatPoolManager.Instance.GetBat(); // 풀에서 배트 가져오기
            if (bat != null)
            {
                bat.SetActive(true); // 활성화
            }
            yield return new WaitForSeconds(5.0f); // 배트를 5초마다 스폰
        }
    }
}
