using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Security;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    private GameManager gameManager;
    private SpriteRenderer sr;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;
    public AudioSource audioSource5;
    public AudioSource audioSource6;
    private AudioClip clip1;
    private AudioClip clip2;
    private AudioClip clip3;
    private AudioClip clip4;
    private AudioClip clip5;
    private AudioClip clip6;

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
    public bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        sr = GetComponent<SpriteRenderer>();
        clip1 = audioSource1.clip;
        clip2 = audioSource2.clip;
        clip3 = audioSource3.clip;
        clip4 = audioSource4.clip;
        clip5 = audioSource5.clip;
        clip6 = audioSource6.clip;
        transform.position = gameManager.lastPos;
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

        if (alive)
        {
            //Flips sprite depending on direction facing
            if (!facingRight)
            {
                sr.flipY = true;
            }

            if (facingRight)
            {
                sr.flipY = false;
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
        //Wallside
        if (WallLeft() && xInput < 0)
        {
            canMove = false;
            rb.velocity = new Vector2(rb.velocity.x, -drag);
            sr.flipY = false;
        }

        if (WallRight() && xInput > 0)
        {
            canMove = false;
            rb.velocity = new Vector2(rb.velocity.x, -drag);
            sr.flipY = true;
        }


        //Unstick to wall if input is opposite to wall
        if (WallLeft() && xInput > 0)
        {
            canMove = true;
        }

        if (WallRight() && xInput < 0)
        {
            canMove = true;
        }
    }

    //Dash code
    public void Dash()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;

        audioSource3.PlayOneShot(clip3);

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
        audioSource6.PlayOneShot(clip6);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    //Wall Jump Code
    public void WallJump()
    {
        audioSource6.PlayOneShot(clip6);

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

    //Collector Code
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.gameObject.CompareTag("Collect"))
        {
            gameManager.score += 100;
            gameManager.scoreSince += 100;
            audioSource1.PlayOneShot(clip1);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("BigCollect"))
        {
            gameManager.score += 500;
            gameManager.scoreSince += 500;
            audioSource2.PlayOneShot(clip2);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            alive = false;
            audioSource4.PlayOneShot(clip4);
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            Invoke("Die", 1);
        }

        if (collision.gameObject.CompareTag("Goal"))
        {
            audioSource5.PlayOneShot(clip5);
            Destroy(collision.gameObject);
        }
    }

    //Reloads the scene
    private void Die()
    {
        gameManager.resetScore();
        gameManager.resetTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
