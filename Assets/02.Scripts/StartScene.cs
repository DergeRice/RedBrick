using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    void Update()
    {
        // �ƹ� Ű�� ������ ���� ����
        if (Input.anyKeyDown)
        {
            // "GameScene" �̸��� ������ ��ȯ
            SceneManager.LoadScene("01.firstScene");
        }
    }
}
