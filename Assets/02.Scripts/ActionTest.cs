using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : MonoBehaviour
{

    [ContextMenu("�����̱�")]
    public void �������()
    {
        TestSingleton.Instance.testAction.Invoke();
    }
}
