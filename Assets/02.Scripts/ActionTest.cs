using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : MonoBehaviour
{

    [ContextMenu("라면끓이기")]
    public void 배고프다()
    {
        TestSingleton.Instance.testAction.Invoke();
    }
}
