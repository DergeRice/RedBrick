using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestSingleton : Singleton<TestSingleton>
{
    public Action testAction;

}
