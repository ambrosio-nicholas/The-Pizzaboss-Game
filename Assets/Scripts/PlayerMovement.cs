using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private bool hasControl = true;
    private float dirX = 0f;

    private bool isWallSliding;
    private float wallSlidingSpeed = 3f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingDuration = 0.5f;
    private float timeAfterWallJump;
    private Vector2 wallJumpingPower = new Vector2(8f, 11f);
    private float offWallCounter;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float jumpTime = 0.2f;
    private float jumpStrength = 10f;
    private float MoveSpeed = 8f;
    private float WalkSpeed = 8f;
    private float RunSpeed = 11f;
    private float SprintSpeed = 12f;
    private float fallGravity = 4.75f;
    private float normalGravity = 3.5f;

    private float jumpTimeCounter;
    private float coyoteTimeCounter;
    private float coyoteTime = 0.1f;
    private bool isJumping;

    private float sprintCounter;
    private float holdDownTime = 2f;
    private bool bigTurnAround = false;

    private enum MovementState { idle, running, jumping, falling , wallSlide , sliding }

    [SerializeField] private AudioSource jumpSoundEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (hasControl && !PauseMenu.GameIsPaused && !isWallJumping && IsGrounded()) //Movement on the ground
        {
            dirX = Input.GetAxisRaw("Horizontal"); //Adds "drift" when turning back quickly
            if (((dirX > 0 && rb.velocity.x < 0) || (dirX < 0 && rb.velocity.x > 0)) && MoveSpeed > WalkSpeed)
            {
                rb.velocity += new Vector2(dirX * 2 * MoveSpeed * Time.deltaTime, 0);
                bigTurnAround = true;
            }
            else
            {
                dirX = Input.GetAxis("Horizontal");
                rb.velocity = new Vector2(dirX * MoveSpeed, rb.velocity.y);
                bigTurnAround = false;
            }
        }
        else if (hasControl && !PauseMenu.GameIsPaused && !IsGrounded()) //Movement in the air
        {
            dirX = Input.GetAxis("Horizontal");
            if (Mathf.Abs((rb.velocity.x)) <= MoveSpeed)
            {
                rb.velocity += new Vector2(dirX * 4 * MoveSpeed * Time.deltaTime, 0);
            }
            else
            {
                dirX = rb.velocity.x / Mathf.Abs(rb.velocity.x); //Returns the direction player is currently traveling as -1 or 1
                rb.velocity = new Vector2(MoveSpeed * dirX, rb.velocity.y);
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
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f && rb.bodyType == RigidbodyType2D.Dynamic && !isWallSliding)
        {
            jumpSoundEffect.Play();
            rb.velocity = rb.velocity + new Vector2(rb.velocity.x, jumpStrength);
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
        if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(rb.velocity.x) > WalkSpeed - 0.1f && IsGrounded())
        {
            sprintCounter += Time.deltaTime;
            MoveSpeed = RunSpeed;
        }
        else if(((Input.GetKeyUp(KeyCode.LeftShift) && IsGrounded()) || isWallSliding || Mathf.Abs(rb.velocity.x) < 5f || bigTurnAround) && !isWallJumping)
        {
            sprintCounter = 0;
            MoveSpeed = WalkSpeed;
        }
        if (Input.GetKey(KeyCode.LeftShift) && sprintCounter > holdDownTime && IsGrounded())
        {
            MoveSpeed = SprintSpeed;
        }
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && !isWallJumping && dirX !=0f && (rb.velocity.x <= 1f && rb.velocity.x >= -1f)) 
        {
            timeAfterWallJump = 1;
            wallJumpingDirection = -dirX;
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            offWallCounter = .4f;
        }
        else
        {
            isWallSliding = false;
  
            offWallCounter -= Time.deltaTime;
        }
    }

    private void WallJump()
    {
        if (Input.GetButtonDown("Jump") && timeAfterWallJump > wallJumpingDuration && offWallCounter > 0 && !IsGrounded() && !isWallJumping) 
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x + (wallJumpingDirection * wallJumpingPower.x), wallJumpingPower.y);
            timeAfterWallJump = 0;
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
        if (dirX > 0)
        {
            return Physics2D.OverlapCircle(wallCheckRight.position, 0.2f, wallLayer);
        } 
        else
        {
            return Physics2D.OverlapCircle(wallCheckLeft.position, 0.2f, wallLayer);
        }
    }

    public bool IsGrounded() // Checking groiund before jumping
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, jumpableGround);
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
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
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

        if (isWallSliding && rb.velocity.y < 1.5f)
        {
            state = MovementState.wallSlide;
        }

        if (bigTurnAround && dirX > 0 && IsGrounded())
        {
            state = MovementState.sliding;
            sprite.flipX = true;
        }

        if (bigTurnAround && dirX < 0 && IsGrounded())
        {
            state = MovementState.sliding;
            sprite.flipX = false;
        }

        anim.SetInteger("state", (int)state);
    }
}