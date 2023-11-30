using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private bool hasControl = true;
    private float dirX = 0f;

    private bool isWallSliding;
    private float wallSlidingSpeed = 3f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingDuration = 0.4f;
    private float timeAfterWallJump;
    private Vector2 wallJumpingPower = new Vector2(7f, 14f);
    private float offWallCounter;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float jumpStrength = 10f;
    [SerializeField] private float jumpTime;
    [SerializeField] private float MoveSpeed = 6f;
    [SerializeField] private float fallGravity = 4.5f;
    [SerializeField] private float normalGravity = 3.5f;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private LayerMask wallLayer;

    private float jumpTimeCounter;
    private float coyoteTimeCounter;
    private float coyoteTime = 0.1f;
    private bool isJumping;

    private float sprintCounter;
    private float holdDownTime = 2f;

    private enum MovementState { idle, running, jumping, falling , wallSlide }

    [SerializeField] private AudioSource jumpSoundEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (hasControl && PauseMenu.GameIsPaused == false)
        {
            if (!isWallJumping)
            {
                dirX = Input.GetAxisRaw("Horizontal");
                rb.velocity = new Vector2(dirX * MoveSpeed, rb.velocity.y);
            }
        }
    }

    private void Update()
    {
        if (hasControl == true && PauseMenu.GameIsPaused == false)
        {

            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }


            Jump();
            Sprint();
            WallSlide();
            WallJump();

            UpdateAnimationState();
        }
    }

    private void Jump() // What affects the jump
    {
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            isJumping = true;
            jumpTimeCounter = jumpTime;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true) // Allows higher jumps based on how long you press jump
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                jumpTimeCounter -= Time.deltaTime;
                coyoteTimeCounter = 0f;
            }
            else
            {
                isJumping = false;
                coyoteTimeCounter = 0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(rb.velocity.x) > 0.5f)
        {
            sprintCounter += Time.deltaTime;
            MoveSpeed = 8f;
        }
        else
        {
            sprintCounter = 0;
            MoveSpeed = 6f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && sprintCounter > holdDownTime)
        {
            MoveSpeed = 9f;
        }
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && !isWallJumping && dirX !=0f) 
        {
            timeAfterWallJump = 1f;
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            offWallCounter = .3f;

            CancelInvoke("StopWallJumping");
        }
        else
        {
            isWallSliding = false;
            offWallCounter -= Time.deltaTime;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        { 
            CancelInvoke("StopWallJumping");
        }

        if (Input.GetButtonDown("Jump") && timeAfterWallJump > wallJumpingDuration && offWallCounter > 0 && !IsGrounded() && !isWallJumping) 
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x + (wallJumpingDirection * wallJumpingPower.x), wallJumpingPower.y);
            timeAfterWallJump += Time.deltaTime;
            isWallJumping = true;
        }

        Invoke("StopWallJumping", wallJumpingDuration);
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheckRight.position, 0.2f, wallLayer) || Physics2D.OverlapCircle(wallCheckLeft.position, 0.2f, wallLayer);
    }

    public bool IsGrounded() // Checking groiund before jumping
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Finish")
        {
            hasControl = false;
        }
    }
    private void UpdateAnimationState() // Animations transitions
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
            wallJumpingDirection = -transform.localScale.x;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
            wallJumpingDirection = transform.localScale.x;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {

            rb.gravityScale = normalGravity;
            state = MovementState.jumping;

        }
        else if (rb.velocity.y < -.1f)
        {
            rb.gravityScale = fallGravity;
            state = MovementState.falling;
        }

        if (isWallSliding && rb.velocity.y < 1f)
        {
            state = MovementState.wallSlide;
        }

        anim.SetInteger("state", (int)state);
    }
}