using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TestClass : MonoBehaviour
{
    //public UnityEvent OnTest;
    //public Action OnTestAction;

    //public delegate void OnDelegate();

    private void Awake()
    {
        //Utils.DelayCall(5f, ()=> { MakeNoodle();});

        //OnTestAction += MakeNoodle; ///????????????????????????????????????????????????????????
        //// 상자 안에 MakeNoodle이라는 함수를 담겠다. // 구독  subscribe
        //OnTestAction -= MakeNoodle;
        // 상자 안에 MakeNoodle이라는 함수를 빼겠다. // 구독 취소  desubscribe

        //OnTestAction = null;

        TestSingleton.Instance.testAction += MakeNoodle;
    }

    public void MakeNoodle()
    {
        Debug.Log("라면을 끓여요");
    }


    // 유니티에서 Action 기능 Event, UnityEvent, delegate
}
