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
        // 키보드 입력이 있을 때 함수 호출
        if (Input.anyKeyDown && !IsMouseInput())
        {
            SceneManager.LoadScene("01.firstScene");
        }
    }

    // 마우스 입력을 제외하기 위한 함수
    private bool IsMouseInput()
    {
        // 마우스 버튼(왼쪽, 오른쪽, 중간) 확인
        return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
    }

}
