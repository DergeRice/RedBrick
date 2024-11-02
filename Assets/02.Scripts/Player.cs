using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;

public class Player : MonoBehaviour
{
    public Vector3 move;
    public float forceAmount, moveSpeed, maxSpeed;

    public GameObject wereWolfState, humanState;

    public Animator wereWolfAnimator, humanAnimator;
    //private SpriteRenderer sp;

    private bool isLeft;

    public bool isBinding = false;

    private void Start()
    {
        moveSpeed = maxSpeed;
    }

    private void Update()
    {
        if (isBinding) moveSpeed = 0;
        else moveSpeed = maxSpeed;
    }

    private void FixedUpdate()
    {
        move.x = SimpleInput.GetAxis("Horizontal");
        move.y = SimpleInput.GetAxis("Vertical");

        isLeft = move.x < 0 ? true : false;

        if (Mathf.Abs(move.x) > 0)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            float newYRotation = move.x < 0 ? 180f : 0f; // y 축 값 설정
            transform.rotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z);
        }

        if(move.magnitude > 0)
        {
            wereWolfAnimator.SetBool("isMoving",true);
            humanAnimator.SetBool("isMoving", true);
        }
        else
        {
            wereWolfAnimator.SetBool("isMoving", false);
            humanAnimator.SetBool("isMoving", false);
        }

        transform.position = transform.position + (move * Time.deltaTime * moveSpeed);
    }

    public void SetWereWolfMask(bool wear) 
    {
        wereWolfState.SetActive(wear);
        humanState.SetActive(!wear);
    }

}
