using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;

public class PlayerNoUse : MonoBehaviour
{
    public Vector3 move;
    public float forceAmount, moveSpeed;

    private void FixedUpdate()
    {
        move.x = SimpleInput.GetAxis("Horizontal");

        transform.position = transform.position + (move * Time.deltaTime * moveSpeed);

        if (SimpleInput.GetButton("Jump"))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * forceAmount, ForceMode2D.Force);
        }
    }
}
