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
        //// ���� �ȿ� MakeNoodle�̶�� �Լ��� ��ڴ�. // ����  subscribe
        //OnTestAction -= MakeNoodle;
        // ���� �ȿ� MakeNoodle�̶�� �Լ��� ���ڴ�. // ���� ���  desubscribe

        //OnTestAction = null;

        TestSingleton.Instance.testAction += MakeNoodle;
    }

    public void MakeNoodle()
    {
        Debug.Log("����� ������");
    }


    // ����Ƽ���� Action ��� Event, UnityEvent, delegate
}
