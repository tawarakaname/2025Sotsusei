using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    //RigidbodyをtheRBと定義
    public Rigidbody theRB;
    public Animator animator;
    Vector3 movement;

    // Update is called once per frame
    void Update()
    {
        //x軸の移動はHorizontal(左右方向キー等)から取得する
        movement.x = Input.GetAxisRaw("Horizontal");
        //z軸の移動はVertical(上下方向キー等)から取得する
        movement.z = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        //移動速度決定フロー。sqrMagnitudeは距離計算に使用されるらしい
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }
    void FixedUpdate()
    {
        //theRBの移動後の位置を決定するための式
        theRB.MovePosition(theRB.position + movement * moveSpeed * Time.fixedDeltaTime);

    }
}