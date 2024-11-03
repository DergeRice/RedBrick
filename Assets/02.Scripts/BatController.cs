using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public GameObject batObject;
    public GameObject[] gameObjects;
    private int pivot = 0;

    private void Start()
    {
        gameObjects = new GameObject[5];
        for(int i= 0; i<5; i++)
        {
            GameObject gameObject = Instantiate(batObject);
            gameObjects[i] = gameObject;
            gameObject.SetActive(false);

        }
        StartCoroutine("EnableBat");

    }

    IEnumerator EnableBat()
    {
        yield return new WaitForSeconds(5f);
        gameObjects[pivot++].SetActive(true);
        if (pivot == 5) pivot = 0;
        StartCoroutine("EnableBat");
    }
}
