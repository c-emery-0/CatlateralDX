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
    
    [SerializeField] public LineCast linerenderer;
    // Movement state machine:  0 is still, -1 is left, 1 is right
    float moveDirection = 0;

    // Object component references
    Rigidbody2D r2d;
    BoxCollider2D mainCollider;
    SpriteRenderer spriteRenderer;
    LineRenderer lineRenderer;

    [SerializeField] private LayerMask groundLayer;

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
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = linerender.GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.25f;
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
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = 1;            
            spriteRenderer.flipX = false;

        }
        else if (isGrounded() || r2d.velocity.magnitude < 0.01f)
        {
            moveDirection = 0;
        }

        // Jumping
        if (isGrounded() && Input.GetButtonDown(KeyCode.C)) jump = true; //toggle, not "live"
        jumpHeld = (!isOnGround() && Input.GetButton(KeyCode.C)); 


        if (jump)
        {
            float jumpvel = 2f;
            r2d.velocity = Vector2.up * jumpvel;
            jump = false;
        }
    
        // jumpheight dependent on holding down the jump
        if(r2d.velocity.y > 0)
            r2d.velocity += (jumpHeld)
            ? Vector2.up * Physics2D.gravity.y * (fallLongMult - 1) * Time.fixedDeltaTime 
            : Vector2.up * Physics2D.gravity.y * (fallShortMult - 1) * Time.fixedDeltaTime;

    }
    // Called at fixed intervals regardless of frame rate, unlike the Update method.
    void FixedUpdate()
    {
        //check if player collides with top, bottom, front (direction movement), behind
        float magnitudey =  (mainCollider.size.y) * 0.5f * Mathf.Abs(transform.localScale.y) + 1;
        float magnitudex = mainCollider.size.x * 0.5f * Mathf.Abs(transform.localScale.y);
        bool top = Physics2D.Raycast(transform.position, Vector2.up, magnitudey);
        bool bot = Physics2D.Raycast(transform.position, Vector2.down, magnitudey);
        if (bot) Debug.Log("bot"); else Debug.Log("no bot");
        if (top) Debug.Log("top"); else Debug.Log("no top");
        
        bool front = Physics2D.Raycast(transform.position, Vector2.right * moveDirection, magnitudex);
        if (front) Debug.Log("front");
        bool behind = Physics2D.Raycast(transform.position, Vector2.right * - moveDirection, magnitudex);
        if (behind) Debug.Log("behind");
        Debug.Log("-");

        /*
        if (front) r2d.velocity = new Vector2(0, r2d.velocity.y);
        */
        if (isGrounded()) Debug.Log("isgrounded");

        // Apply movement velocity in the x direction
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);

    }
    private bool isGrounded()
    {
        float magnitudey =  (mainCollider.size.y) * 0.6f ;
        bool bot = Physics2D.Raycast(transform.position, Vector2.down, magnitudey);
        Debug.DrawRay(position, direction, Color.green);
        return bot;
        RaycastHit2D hit = Physics2D.CircleCast(mainCollider.bounds.center, mainCollider.size.x / 2, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;


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
}