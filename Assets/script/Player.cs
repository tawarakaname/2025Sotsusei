using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    //Rigidbody??theRB?????`
    public Rigidbody theRB;
    public Animator animator;
    Vector3 movement;

    // Update is called once per frame
    void Update()
    {
        
        movement.x = Input.GetAxisRaw("Horizontal Stick-L");
        movement.z = Input.GetAxisRaw("Vertical Stick-L");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }
    void FixedUpdate()
    {
        
        theRB.MovePosition(theRB.position + movement * moveSpeed * Time.fixedDeltaTime);

    }
}