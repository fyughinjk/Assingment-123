using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public bool TestMode;

    [SerializeField] private int speed;
    [SerializeField] private int jumpForce = 3;


    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private float groundCheckRadius; 

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody2D>(); 
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
        if (speed <= 0)
        {
            speed = 5;
            if (TestMode) Debug.Log("Speed set to default Value.");
        }

        if (jumpForce <= 0)
        {
            jumpForce = 3;
            if (TestMode) Debug.Log("Jump Force set to default Value.");
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.2f;
            if (TestMode) Debug.Log("Ground Check Radius set to default Value.");
        }

        if (groundCheck == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("GroundCheck");
            if (obj != null)
            {
                groundCheck = obj.transform;
                return;
            }
            GameObject newObj = new GameObject();
            newObj.transform.SetParent(transform);
            newObj.transform.localPosition = Vector3.zero;
            newObj.name = "GroundCheck";
            newObj.tag = newObj.name;
            groundCheck = newObj.transform;
            if (TestMode) Debug.Log("Ground Check Tranform created via code.");
        }

       
    }

    // Update is called once per frame
    void Update()
    {

        float xInput = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        Vector2 moveDirection = new Vector2(xInput * speed, rb.velocity.y);

        rb.velocity = moveDirection;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }

        if (xInput != 0) sr.flipX = (xInput < 0);

        anim.SetFloat("Speed", Mathf.Abs(xInput));
        anim.SetBool("isGrounded", isGrounded);
        
    }
}
