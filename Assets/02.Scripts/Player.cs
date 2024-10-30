using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;

public class Player : MonoBehaviour
{
    public Vector3 move;
    public float forceAmount;

    private void FixedUpdate()
    {
        move.x = SimpleInput.GetAxis("Horizontal");

        transform.position += move;

        if (SimpleInput.GetButton("Jump"))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * forceAmount, ForceMode2D.Force);
        }
    }
}
