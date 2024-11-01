using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;

public class PlayerTopView : MonoBehaviour
{
    public Vector3 move;
    public float forceAmount, moveSpeed;

    private void FixedUpdate()
    {
        move.x = SimpleInput.GetAxis("Horizontal");
        move.y = SimpleInput.GetAxis("Vertical");

        transform.position = transform.position + (move * Time.deltaTime * moveSpeed);


    }
}
