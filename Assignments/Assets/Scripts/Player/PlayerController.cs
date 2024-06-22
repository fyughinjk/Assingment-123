using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public bool TestMode;
    [SerializeField] private int speed;
    [SerializeField] private int jumpForce = 3;

    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private AudioClip jumpClip;

    [SerializeField] private bool isOnWall;
    [SerializeField] private bool wallOnRight;
    [SerializeField] private bool wallOnLeft;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private LayerMask isWallLayer;
    [SerializeField] private bool isSliding;
    [SerializeField] private float wallSlideSpeed = 0.5f;
    [SerializeField] private float wallCheckRadius = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource audioSource;

    private Coroutine jumpForceChange = null;

    public void PowerupValueChange(PickUp.PickupType type)
    {
        if (type == PickUp.PickupType.PowerupJump)
            FillSpecificCoroutineVar(ref jumpForceChange, ref jumpForce, type);
    }

    void FillSpecificCoroutineVar(ref Coroutine inVar, ref int varToChange, PickUp.PickupType type)
    {
        if (inVar != null)
        {
            StopCoroutine(inVar);
            inVar = null;
            varToChange /= 2;
            inVar = StartCoroutine(ValueChangeCoroutine(type));
            return;
        }

        inVar = StartCoroutine(ValueChangeCoroutine(type));
    }

    IEnumerator ValueChangeCoroutine(PickUp.PickupType type)
    {
        if (type == PickUp.PickupType.PowerupJump)
            jumpForce *= 2;

        yield return new WaitForSeconds(2.0f);

        if (type == PickUp.PickupType.PowerupJump)
        {
            jumpForce /= 2;
            jumpForceChange = null;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (speed <= 0)
        {
            speed = 7;
            if (TestMode) Debug.Log("Speed set to default Value.");
        }

        if (jumpForce <= 0)
        {
            jumpForce = 6;
            if (TestMode) Debug.Log("Jump Force set to default Value.");
        }

        if (wallCheckRadius <= 0)
        {
            wallCheckRadius = 1f;
            if (TestMode) Debug.Log("Wall Check Radius set to default Value.");
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

        if (wallCheckRight == null)
        {
            GameObject newObj = new GameObject();
            newObj.transform.SetParent(transform);
            newObj.transform.localPosition = new Vector3(0.5f, 0.8f, 0);
            newObj.name = "WallCheckRight";
            wallCheckRight = newObj.transform;
            if (TestMode) Debug.Log("Wall Check Right Tranform created via code.");
        }

        if (wallCheckLeft == null)
        {
            GameObject newObj = new GameObject();
            newObj.transform.SetParent(transform);
            newObj.transform.localPosition = new Vector3(-0.5f, 0.8f, 0);
            newObj.name = "WallCheckLeft";
            wallCheckLeft = newObj.transform;
            if (TestMode) Debug.Log("Wall Check Left Tranform created via code.");
        }
    }

    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        float xInput = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        wallOnRight = Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, isWallLayer);
        wallOnLeft = Physics2D.OverlapCircle(wallCheckLeft.position, wallCheckRadius, isWallLayer);
        isOnWall = wallOnRight || wallOnLeft;

        Debug.Log($"isGrounded: {isGrounded}, wallOnRight: {wallOnRight}, wallOnLeft: {wallOnLeft}");

        if (curPlayingClips.Length > 0)
        {
            if (curPlayingClips[0].clip.name == "Attack")
                rb.velocity = Vector2.zero;
            else
            {
                Vector2 moveDirection = new Vector2(xInput * speed, rb.velocity.y);
                rb.velocity = moveDirection;
                if (Input.GetButtonDown("Fire1"))
                {
                    anim.SetTrigger("Attack");
                    audioSource.Play();
                }
            }
        }

        if (isOnWall && !isGrounded)
        {
            isSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isSliding = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                audioSource.PlayOneShot(jumpClip);
            }
            else if (isSliding)
            {
                if (wallOnRight)
                {

                }
                float jumpDirection = wallOnRight ? -1 : 1;
                rb.velocity = new Vector2(jumpDirection * speed, jumpForce);
                audioSource.PlayOneShot(jumpClip);


                sr.flipX = wallOnRight;


                isSliding = false;
            }
            else
            {
                anim.SetTrigger("JumpAttack");
            }
        }

        if (xInput != 0) sr.flipX = (xInput < 0);

        anim.SetFloat("Speed", Mathf.Abs(xInput));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("isOnWall", isOnWall);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckRadius);
    }

    private void GameOver()
    {
        Debug.Log("Game Over goes here");
    }

    private void Respawn()
    {
        Debug.Log("Respawn goes here");
    }
}
