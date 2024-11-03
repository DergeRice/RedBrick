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
            GameObject bat = BatPoolManager.Instance.GetBat(); // Ǯ���� ��Ʈ ��������
            if (bat != null)
            {
                bat.SetActive(true); // Ȱ��ȭ
            }
            yield return new WaitForSeconds(5.0f); // ��Ʈ�� 5�ʸ��� ����
        }
    }
}
