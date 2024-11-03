using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{

    private void Start()
    {
        GameObject.Find("SettingBtn").SetActive(true);
    }
    void Update()
    {
        // Ű���� �Է��� ���� �� �Լ� ȣ��
        if (Input.anyKeyDown && !IsMouseInput())
        {
            SceneManager.LoadScene("01.firstScene");
        }
    }

    // ���콺 �Է��� �����ϱ� ���� �Լ�
    private bool IsMouseInput()
    {
        // ���콺 ��ư(����, ������, �߰�) Ȯ��
        return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
    }

}
