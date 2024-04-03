using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement mechanics variables
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    bool jump = false, jumpHeld = false;
 
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    
    // Movement state machine:  0 is still, -1 is left, 1 is right
    float moveDirection = 0;

    // Object component references
    Rigidbody2D r2d;
    BoxCollider2D collider;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Collision collision;

    // To get camera to follow Player: 
    //      1. Add/install Cinemachine from Unity package manager
    //      2. Add a Cinemachine 2D Camera object to scene
    //      3. Drag and drop Player object into the 2D Camera's 'follow' field

    // A Cinemachine virtual camera is like a cameraman controlling the position and settings 
    //      of the Main Camera, but not actually a camera itself.

    private enum CharStates {
        idle = 1,
        walk = 2,
        jump = 3,
    }

    // Use this for initialization
    void Start()
    {
        // initialize object component variables
        r2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        collision = GetComponent<Collision>();
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
        if (Input.GetKey(KeyCode.LeftArrow))
            moveDirection = -1;
        else if (Input.GetKey(KeyCode.RightArrow))
            moveDirection = 1;
        else if (collision.onGround || r2d.velocity.magnitude < 0.01f)
            moveDirection = 0;


        if (Input.GetButtonDown("Jump"))
        {
            if (collision.onGround)
                r2d.velocity = new Vector2(r2d.velocity.x, 0); //kill momentum. only a set jump dist
                r2d.velocity += Vector2.up * 5; //the 5 is variable
            //if (coll.onWall && !coll.onGround)
            //    WallJump();
        }
        /* //"better jumping"
        if(r2d.velocity.y < 0)
        {
            r2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(r2d.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            r2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        */
    }
    // Called at fixed intervals regardless of frame rate, unlike the Update method.
    void FixedUpdate()
    {


        updateCharState();

        // Apply movement velocity in the x direction
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);

    }
    private void updateCharState() {
        if (moveDirection < 0) spriteRenderer.flipX = true;
        if (moveDirection > 0) spriteRenderer.flipX = false;


        if (r2d.velocity.y != 0) {
            anim.SetInteger("CharState", (int) CharStates.jump);
            return;
        }
        if (moveDirection == 0) {
            anim.SetInteger("CharState", (int) CharStates.idle);
            return;
        }
            anim.SetInteger("CharState", (int) CharStates.walk);
        return;
    }
}