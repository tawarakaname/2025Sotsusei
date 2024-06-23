using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    //Rigidbody��theRB�ƒ�`
    public Rigidbody theRB;
    public Animator animator;
    Vector3 movement;

    // Update is called once per frame
    void Update()
    {
        //x���̈ړ���Horizontal(���E�����L�[��)����擾����
        movement.x = Input.GetAxisRaw("Horizontal");
        //z���̈ړ���Vertical(�㉺�����L�[��)����擾����
        movement.z = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        //�ړ����x����t���[�BsqrMagnitude�͋����v�Z�Ɏg�p�����炵��
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }
    void FixedUpdate()
    {
        //theRB�̈ړ���̈ʒu�����肷�邽�߂̎�
        theRB.MovePosition(theRB.position + movement * moveSpeed * Time.fixedDeltaTime);

    }
}