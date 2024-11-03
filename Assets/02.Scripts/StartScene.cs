using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    void Update()
    {
        // 아무 키나 눌리면 게임 시작
        if (Input.anyKeyDown)
        {
            // "GameScene" 이름의 씬으로 전환
            SceneManager.LoadScene("01.firstScene");
        }
    }
}
