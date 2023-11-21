using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 9;
    public float jumpForce = 10;
    public float xInput;
    public float castDist = 0.6f;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public bool firstJump;
    public float wallJumpForce = 5;
    public bool canMove = true;
    public bool firstDash = true;
    public float dashSpeed = 5;
    public bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * castDist, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * castDist, Color.red);

        Debug.Log(wallRight());

        xInput = Input.GetAxis("Horizontal");
        if (canMove)
        {
            transform.Translate(speed * Time.deltaTime * xInput, 0, 0);
        }

        if (xInput > 0)
        {
            facingRight = true;
        }
        else if (xInput < 0)
        {
            facingRight = false;
        }
        
        if (isGrounded())
        {
            canMove = true;
            firstJump = true;
            firstDash = true;
        }

        /*if (xInput > 0)
        {   
            Invoke("RunSpeed", 0.5f);
        }
        else
        {
            speed = 9;
        }*/


        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            jump();
        }

        if (Input.GetButtonDown("Jump") && firstJump && !isGrounded())
        {
            jump();
            firstJump = false;
        }

        if (Input.GetButtonDown("Jump") && !isGrounded() && (wallRight() || wallLeft()))
        {
            wallJump();
            firstJump = false;
            canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && firstDash && !isGrounded())
        {
            dash();
            canMove = false;
        }
    }

    public void dash()
    {
        if (facingRight)
        {
            rb.velocity = new Vector2(rb.velocity.x + dashSpeed, rb.velocity.y);
            
        }

        if (!facingRight)
        {
            rb.velocity = new Vector2(rb.velocity.x - dashSpeed, rb.velocity.y);
        }
    }

    public bool isGrounded()
    {
        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, castDist, groundLayer);
        return isGrounded;
    }

    public bool wallRight()
    {
        if(Physics2D.Raycast(transform.position, Vector2.right, castDist, wallLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool wallLeft()
    {
        if (Physics2D.Raycast(transform.position, Vector2.left, castDist, wallLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void RunSpeed()
    {
        speed = 7;
    }

    public void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void wallJump()
    {
        if (wallRight())
        {
            rb.velocity = new Vector2(rb.velocity.x - wallJumpForce, jumpForce);
            facingRight = false;
            
        }

        if (wallLeft())
        {
            rb.velocity = new Vector2(rb.velocity.x + wallJumpForce, jumpForce);
            facingRight = true;
        }
    }
}
