using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement mechanics variables
    public float maxSpeed = 10f;
    public float jumpHeight = 15f;
    public float gravityScale = 1.5f;
    bool jump = false, jumpHeld = false;
 
    private float jumpStartTime = 1f;
    private float jumpTime;
    private bool isJumping;
    
    // Movement state machine:  0 is still, -1 is left, 1 is right
    float moveDirection = 0;

    // Object component references
    private Rigidbody2D r2d;
    private BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Collision collision;
    private GameObject currentOneWayPlatform;
    private int inputx, inputy;


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
        inputx = Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
        inputx = Input.GetKey(KeyCode.RightArrow) ? 1 : inputx;
        inputy = Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        inputy = Input.GetKey(KeyCode.UpArrow) ? 1 : inputy;

        moveDirection = inputx;
        
        
        if (inputy < 0) {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }
    // Called at fixed intervals regardless of frame rate, unlike the Update method.
    void FixedUpdate()
    {
        updateCharState();
        
        // Apply movement velocity in the x direction
        //r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);

        // X movement
        //movedir is 0 if (collision.onGround || r2d.velocity.magnitude < 0.01f)

        float xvelo = r2d.velocity.x;
        //if (System.Math.Abs(r2d.velocity.x) <= .01f)
        //    xvelo = 0;
        //else 
        if (System.Math.Abs(r2d.velocity.x) < maxSpeed || System.Math.Abs(moveDirection + r2d.velocity.x) < System.Math.Abs(r2d.velocity.x  ))
            xvelo += moveDirection * maxSpeed * 0.1f;
        if (moveDirection == 0) 
            xvelo = 0;
        r2d.velocity = new Vector2(xvelo, r2d.velocity.y);
        

        if (!( collision.onGround || collision.onPlatform))
            {r2d.velocity = new Vector2(r2d.velocity.x, r2d.velocity.y + Physics2D.gravity.y * Time.deltaTime);}
        else
            {r2d.velocity = new Vector2(r2d.velocity.x, 0);}
        
        Jump();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Debug.Log("Disabled");
        Physics2D.IgnoreCollision(collider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(collider, platformCollider, false);
    }
 
    void Jump() {
        if (collision.onGround && Input.GetButtonDown("Jump")) {
            isJumping = true;
            jumpTime = jumpStartTime;
            r2d.velocity = Vector2.up * jumpHeight;
        }
        if (isJumping && Input.GetButtonDown("Jump")) {
            if (jumpTime <= 0) 
                isJumping = false;
            else {
                r2d.velocity = Vector2.up * jumpHeight;
                jumpTime -= Time.deltaTime;
            }
        }
        if (Input.GetButtonUp("Jump"))
            isJumping = false;
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