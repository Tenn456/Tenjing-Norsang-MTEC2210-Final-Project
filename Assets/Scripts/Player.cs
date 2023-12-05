using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed;
    public float jumpForce = 10;
    public bool firstJump;
    public float wallJumpForce = 5;
    public float xInput;
    public float castDist = 0.6f;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public bool canMove = true;
    public bool facingRight = true;
    public bool firstDash = true;
    public float dashSpeed = 10;
    public float drag = 4;
    public bool wallJumping;
    public bool wallJumped;
    float wjtimer = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //For visualizing Raycasts
        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * castDist, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * castDist, Color.red);

        //Movement
        xInput = Input.GetAxis("Horizontal");
        if (canMove)
        {
            //Checks which way you are facing
            if (xInput > 0)
            {
                facingRight = true;
            }
            else if (xInput < 0)
            {
                facingRight = false;
            }
        }

        
        //Movement refresher
        if (IsGrounded())
        {
            canMove = true;
            firstJump = true;
            firstDash = true;
            wallJumping = false;
        }

        //Jump if press space and is grounded
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        //Double jump if you only jumped once
        if (Input.GetButtonDown("Jump") && firstJump && !IsGrounded())
        {
            Jump();
            firstJump = false;
        }

        //Wall jump if you are against a wall in the air
        if (Input.GetButtonDown("Jump") && !IsGrounded() && WallRight() && xInput > 0)
        {
            wallJumping = true;
            WallJump();
            firstJump = false;
            canMove = false;
            firstDash = true;
        }

        if (Input.GetButtonDown("Jump") && !IsGrounded() && WallLeft() && xInput < 0)
        {
            wallJumping = true;
            WallJump();
            firstJump = false;
            canMove = false;
            firstDash = true;
        }

        //Dash if you are in the air
        if (Input.GetKeyDown(KeyCode.LeftShift) && firstDash && !IsGrounded())
        {
            Dash();
            canMove = false;
            firstDash = false;
            
        }

        //Wall Slide if you are against a wall in the air
        if (!IsGrounded() && (WallLeft() || WallRight()) && !wallJumping)
        {
            WallSlide();
        }

        //To Fix Wall Slide dragging Wall Jump down
        if (wallJumping)
        {
            wjtimer -= Time.deltaTime;
        }

        if(wjtimer < 0)
        {
            wallJumping = false;
            wjtimer = 0.1f;
        }
    }

    public void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
        }
        
    }

    //WallSlide Code
    public void WallSlide()
    {
        if (WallLeft() && xInput < 0)
        {
            canMove = false;
            rb.velocity = new Vector2(rb.velocity.x, -drag);
        }

        if (WallRight() && xInput > 0)
        {
            canMove = false;
            rb.velocity = new Vector2(rb.velocity.x, -drag);
        }
    }

    //Dash code
    public void Dash()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;

        if (facingRight)
        {
            rb.velocity = new Vector2(dashSpeed, rb.velocity.y); 
        }

        if (!facingRight)
        {
            rb.velocity = new Vector2(-dashSpeed, rb.velocity.y);
        }


        Invoke("DashTimer", 0.2f);
    }

    public void DashTimer()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    //Ground Check
    public bool IsGrounded()
    {
        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, castDist, groundLayer);
        return isGrounded;
    }

    //Wall Check
    public bool WallRight()
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

    public bool WallLeft()
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

    //Jump Code
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    //Wall Jump Code
    public void WallJump()
    {

        if (WallRight())
        {
            rb.velocity = new Vector2(rb.velocity.x - wallJumpForce, jumpForce);
            facingRight = false;
            
        }

        if (WallLeft())
        {
            rb.velocity = new Vector2(rb.velocity.x + wallJumpForce, jumpForce);
            facingRight = true;
        }

    }
}
