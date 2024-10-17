using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorMove : MonoBehaviour
{
    public float walkSpeed;
    public float jumpSpeed;

    Rigidbody2D myRigidbody2D;

    public SpriteRenderer mySpriteRenderer;
    public Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("right"))
        {
            myRigidbody2D.velocity = new Vector2(walkSpeed, myRigidbody2D.velocity.y);
            mySpriteRenderer.flipX = false;
            myAnimator.SetBool("walk", true);
        }
        else if (Input.GetKey("left"))
        {
            myRigidbody2D.velocity = new Vector2(-walkSpeed, myRigidbody2D.velocity.y);
            mySpriteRenderer.flipX = true;
            myAnimator.SetBool("walk", true);
        }
        else
        {
            myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
            myAnimator.SetBool("walk", false);
        }
        if (Input.GetKey("space") && CheckGround.isGrounded)
        {
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpSpeed);
        }
        if (CheckGround.isGrounded==false)
        {
            myAnimator.SetBool("jump", true);
            myAnimator.SetBool("walk", false);
        }
        if (CheckGround.isGrounded==true)
        {
            myAnimator.SetBool("jump", false);
            
        }
    }
}
