using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CherryManager : MonoBehaviour
{
    private void Start()
    {
        SpawnCherry();
    }

    private void SpawnCherry()
    {

        transform.GetChild(0).gameObject.SetActive(true);

        // z���� -20������ 20�� ������ ���������� ����
        float randomZRotation = Random.Range(-40f, 40f);
        transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, randomZRotation);
    }

    public void AteCherry()
    {
        Utils.DelayCall(10f,() => { SpawnCherry(); });
    }

}
