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
 
    [SerializeField] private float fallLongMult = 0.85f;
    [SerializeField] private float fallShortMult = 1.55f;
    
    // Movement state machine:  0 is still, -1 is left, 1 is right
    float moveDirection = 0;

    // Object component references
    Rigidbody2D r2d;
    BoxCollider2D mainCollider;
    SpriteRenderer spriteRenderer;
    Animator animator;

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
        mainCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        {
            moveDirection = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = 1;            

        }
        else if (isGrounded() || r2d.velocity.magnitude < 0.01f)
        {
            moveDirection = 0;
        }

        // Jumping
        jump = (isGrounded() && Input.GetKeyDown(KeyCode.C)); //getKeyDown is true on first frame if button is depressed
        jumpHeld = (!isGrounded() && Input.GetKey(KeyCode.C)); //getKey is true for every frame if button is depressed


        if (jump)
        {
            float jumpvel = 2f;
            r2d.velocity = Vector2.up * jumpvel * jumpHeight;
            jump = false;
        }
    
        // jumpheight dependent on holding down the jump
        if(r2d.velocity.y > 0)
            r2d.velocity += (jumpHeld)
            ? Vector2.up * Physics2D.gravity.y * jumpHeight * (fallLongMult - 1) * Time.fixedDeltaTime 
            : Vector2.up * Physics2D.gravity.y * jumpHeight * (fallShortMult - 1) * Time.fixedDeltaTime;
        
        
        
        
        
    }
    // Called at fixed intervals regardless of frame rate, unlike the Update method.
    void FixedUpdate()
    {
        //check if player collides with top, bottom, front (direction movement), behind
        float magnitudey =  (mainCollider.size.y) * 0.55f ;
        float magnitudex = mainCollider.size.x * 0.55f ;
        RaycastHit2D top = Physics2D.Raycast(transform.position, Vector2.up, magnitudey);
        RaycastHit2D front = Physics2D.Raycast(transform.position, Vector2.right * moveDirection, magnitudex);
        RaycastHit2D behind = Physics2D.Raycast(transform.position, Vector2.right * - moveDirection, magnitudex);
        Color color1 = (top.collider) ? Color.green : Color.blue;
        Color color2 = (front.collider) ? Color.green : Color.blue;
        Color color3 = (behind.collider) ? Color.green : Color.blue;
        Debug.DrawRay(transform.position, Vector2.up * magnitudey, color1);
        Debug.DrawRay(transform.position, Vector2.right * moveDirection * magnitudex, color2);
        Debug.DrawRay(transform.position, Vector2.right * -moveDirection * magnitudex, color3);

        /*
        if (front) r2d.velocity = new Vector2(0, r2d.velocity.y);
        */
        if (isGrounded()) Debug.Log("isgrounded");

        updateCharState();
        // Apply movement velocity in the x direction
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);

    }
    private bool isGrounded()
    {
        float magnitudey =  (mainCollider.size.y) * 0.55f ;
        RaycastHit2D bot = Physics2D.Raycast(transform.position, Vector2.up * -1, magnitudey);
        Color color4 = (bot.collider) ? Color.green : Color.blue;
        Debug.DrawRay(transform.position, Vector2.up * magnitudey * -1, color4);
        
        return bot;


        /*
        // Get information from Player's collider
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);

        // Position to check for if grounded
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        
        //Access all overlapping colliders at groundCheckPos
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);

        isGrounded = false;
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)

            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    Debug.Log("Landed on: " + colliders[i]);
                    break;
                }
            }
        }
        */
    }
    private void updateCharState() {
        if (moveDirection < 0) spriteRenderer.flipX = true;
        if (moveDirection > 0) spriteRenderer.flipX = false;

        
        if (r2d.velocity.y != 0) {
            animator.SetInteger("CharState", (int) CharStates.jump);
            return;
        }
        if (moveDirection == 0) {
            animator.SetInteger("CharState", (int) CharStates.idle);
            return;
        }
            animator.SetInteger("CharState", (int) CharStates.walk);
        return;
    }
}