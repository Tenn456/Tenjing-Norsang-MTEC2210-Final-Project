using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 5;
    public float jumpForce = 10;
    public float xInput;
    public float castDist = 0.6f;
    public LayerMask groundLayer;
    public bool firstJump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);

        xInput = Input.GetAxis("Horizontal");

        transform.Translate(speed * Time.deltaTime * xInput, 0, 0);

        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            jump();
            firstJump = true;
        }

        if (Input.GetButtonDown("Jump") && firstJump && !isGrounded())
        {
            jump();
            firstJump = false;
        }
    }

    public bool isGrounded()
    {
        if(Physics2D.Raycast(transform.position, Vector2.down, castDist, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
