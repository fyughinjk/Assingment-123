using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class WallJump : MonoBehaviour
{
    public int jumpForce = 6;
    public float wallSlideSpeed = 0.5f;
    public float jumpCooldown = 0.2f;
    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    public LayerMask isWallLayer;
    public float wallCheckRadius = 0.2f;
    public AudioClip jumpClip;

    private bool isOnWall;
    private bool wallOnRight;
    private bool wallOnLeft;
    private bool isSliding;
    private bool canJump = true;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    private PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();

        if (jumpForce <= 0)
        {
            jumpForce = 6;
            Debug.Log("Jump Force set to default Value.");
        }

        if (wallCheckRadius <= 0)
        {
            wallCheckRadius = 0.2f;
            Debug.Log("Wall Check Radius set to default Value.");
        }
    }

    void Update()
    {
        wallOnRight = Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, isWallLayer);
        wallOnLeft = Physics2D.OverlapCircle(wallCheckLeft.position, wallCheckRadius, isWallLayer);
        isOnWall = wallOnRight || wallOnLeft;

        playerController.SetisOnWall(isOnWall);

        Debug.Log($"wallOnRight: {wallOnRight}, wallOnLeft: {wallOnLeft}, isOnWall: {isOnWall}");

        if (isOnWall && rb.velocity.y < 0)
        {
            isSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            Debug.Log("Sliding on Wall");
        }
        else
        {
            isSliding = false;
        }

        playerController.SetisSliding(isSliding);

        if (Input.GetButtonDown("Jump") && canJump)
        {
            if (isOnWall)
            {
                float jumpDirection = wallOnRight ? -1 : 1;
                rb.velocity = new Vector2(rb.velocity.x, 0); // Reset y-velocity
                rb.AddForce(new Vector2(jumpDirection * jumpForce, jumpForce), ForceMode2D.Impulse);
                audioSource.PlayOneShot(jumpClip);
                sr.flipX = wallOnRight;
                isSliding = false;
                StartCoroutine(JumpCooldown()); // Initiate jump cooldown
                Debug.Log($"Wall Jump: direction={jumpDirection}, jumpForce={jumpForce}");
            }
        }
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckRadius);
    }
}
