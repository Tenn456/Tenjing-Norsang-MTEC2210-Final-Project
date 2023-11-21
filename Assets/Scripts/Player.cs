using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
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
    public float drag = 2;

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
            transform.Translate(speed * Time.deltaTime * xInput, 0, 0);

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
        if (isGrounded())
        {
            canMove = true;
            firstJump = true;
            firstDash = true;
        }

        //Jump if press space and is grounded
        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            jump();
        }

        //Double jump if you only jumped once
        if (Input.GetButtonDown("Jump") && firstJump && !isGrounded())
        {
            jump();
            firstJump = false;
        }

        //Wall jump if you are against a wall in the air
        if (Input.GetButtonDown("Jump") && !isGrounded() && (wallRight() || wallLeft()))
        {
            wallJump();
            firstJump = false;
            canMove = false;
            firstDash = true;
        }

        //Dash if you are in the air
        if (Input.GetKeyDown(KeyCode.LeftShift) && firstDash && !isGrounded())
        {
            dash();
            canMove = false;
            firstDash = false;
            
        }

        //Wall Side if you are against a wall in the air
        if (!isGrounded() && (wallRight() || wallLeft()))
        {
            rb.velocity = new Vector2(rb.velocity.x, - drag);
        }
    }

    //Dash code
    public void dash()
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

        Invoke("dashTimer", 0.2f);
    }

    public void dashTimer()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    //Ground Check
    public bool isGrounded()
    {
        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, castDist, groundLayer);
        return isGrounded;
    }

    //Wall Check
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

    //Jump Code
    public void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    //Wall Jump Code
    public void wallJump()
    {
        drag = 0;
        if (wallRight())
        {
            rb.velocity = new Vector2(rb.velocity.x - wallJumpForce, (jumpForce / 2));
            facingRight = false;
            
        }

        if (wallLeft())
        {
            rb.velocity = new Vector2(rb.velocity.x + wallJumpForce, jumpForce / 2);
            facingRight = true;
        }
    }
}
