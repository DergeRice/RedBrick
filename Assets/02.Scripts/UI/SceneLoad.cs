using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{

    public void OnClick()
    {
        SceneManager.LoadScene("01.firstScene");
    }
}
