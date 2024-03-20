using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement mechanics variables
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;

    // Movement state machine:  0 is still, -1 is left, 1 is right
    string moveDirection = "moveDirection";
    string isGrounded = "isGrounded";
    string charState = "CharState";

    // Object component references
    Rigidbody2D r2d;
    BoxCollider2D mainCollider;
    Animator animator;
    SpriteRenderer spriterenderer;

    const int walk = 1;
    const int idle = 0;
    const int jump = 2;

    // To get camera to follow Player: 
    //      1. Add/install Cinemachine from Unity package manager
    //      2. Add a Cinemachine 2D Camera object to scene
    //      3. Drag and drop Player object into the 2D Camera's 'follow' field

    // A Cinemachine virtual camera is like a cameraman controlling the position and settings 
    //      of the Main Camera, but not actually a camera itself.


    // Use this for initialization
    void Start()
    {
        // initialize object component variables
        spriterenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();


        animator.SetInteger(moveDirection, 0);
        animator.SetBool(isGrounded, false);

        // If freezeRotation is enabled, the rotation in Z is not modified by the physics simulation.
        //      Good for 2D!
        r2d.freezeRotation = true;

        // Ensures that all collisions are detected when a Rigidbody2D moves.
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Apply gravity scale to Player's Rigidbody2D
        r2d.gravityScale = gravityScale;

    }

    // Update is called once per frame (e.g. runs 24 times at 24 frames per second).
    //      Use FixedUpdate if you require consistent time between method calls.
    void Update()
    {
        // Movement controls (left and right)
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetInteger(moveDirection, -1);
            spriterenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {            
            animator.SetInteger(moveDirection, 1);
            spriterenderer.flipX = false;
        }
        else if (animator.GetBool(isGrounded) || r2d.velocity.magnitude < 0.01f)
        {            
            animator.SetInteger(moveDirection, 0);
        }

        // Jumping
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && animator.GetBool(isGrounded))
        {
            // Apply movement velocity in the y direction
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }


        if ((animator.GetInteger(moveDirection)) != 0 ) {
            animator.SetInteger(charState, walk);
        }
        else if ((animator.GetInteger(moveDirection)) == 0 ) {
            animator.SetInteger(charState, idle);
        }
        else if ( !animator.GetBool(isGrounded) ) {
            animator.SetInteger(charState, jump);
        }
    }
    // Called at fixed intervals regardless of frame rate, unlike the Update method.
    void FixedUpdate()
    {
        // Get information from Player's collider
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);

        // Position to check for if grounded
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        
        //Access all overlapping colliders at groundCheckPos
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);

        animator.SetBool(isGrounded, false);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    animator.SetBool(isGrounded, true);
                    Debug.Log("Landed on: " + colliders[i]);
                    break;
                }
            }
        }

        // Apply movement velocity in the x direction
        r2d.velocity = new Vector2((animator.GetInteger(moveDirection)) * maxSpeed, r2d.velocity.y);

    }

}