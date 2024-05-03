using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement mechanics variables
    public float maxSpeed = 10f;
    public float jumpHeight = 6f;
 
    private float jumpStartTime = .05f;
    private float jumpTime;
    private bool isJumping;
    private bool canJump = true;

    private GameObject grabbedObject;

    // Movement state machine:  0 is still, -1 is left, 1 is right
    float moveDirection = 0;

    // Object component references
    private Rigidbody2D r2d;
    private CapsuleCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Collision collision;
    private GameObject currentOneWayPlatform;

    private int inputx, inputy;

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
        collider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        collision = GetComponent<Collision>();
        // If freezeRotation is enabled, the rotation in Z is not modified by the physics simulation.
        //      Good for 2D!
        r2d.freezeRotation = true;

        // Ensures that all collisions are detected when a Rigidbody2D moves.
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;


    }

    // Update is called once per frame (e.g. runs 24 times at 24 frames per second).
    //      Use FixedUpdate if you require consistent time between method calls.
    void Update()
    {
        // Movement controls (left and right)
        inputx = (int) Input.GetAxisRaw("Horizontal");
        inputy = (int) Input.GetAxisRaw("Vertical");

        moveDirection = inputx;
        
        
        if (inputy < 0 && currentOneWayPlatform != null) 
            StartCoroutine(DisableCollision());
            
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyUp(KeyCode.X)) 
            toggleGrab();
        

    }
    // Called at fixed intervals regardless of frame rate, unlike the Update method.
    void FixedUpdate()
    {
        updateCharState();
        
        //xvelo
        float xvelo = r2d.velocity.x;
        if (System.Math.Abs(r2d.velocity.x) < maxSpeed 
        || System.Math.Abs(moveDirection + r2d.velocity.x) < System.Math.Abs(r2d.velocity.x))
            xvelo += moveDirection * maxSpeed * 0.1f;
        if (moveDirection == 0) 
            xvelo = 0;
        
        //yvelo
        float yvelo = r2d.velocity.y;
        if (!( collision.onGround && currentOneWayPlatform != null))
            //falling
            yvelo += Physics2D.gravity.y * Time.deltaTime;
        else
            //onGround
            yvelo = 0;
        
        r2d.velocity = new Vector2(xvelo, yvelo);
        
        Jump();
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = coll.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        //for disabling fallthrough platforms
        BoxCollider2D platColl = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(collider, platColl);
        yield return new WaitForSeconds(0.75f);
        Physics2D.IgnoreCollision(collider, platColl, false);
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(0.75f);
        canJump = true;
    }
 
    void Jump() {
        if (canJump && (collision.onGround || collision.onProp || currentOneWayPlatform != null) && inputy > 0) {
            isJumping = true;
            StartCoroutine(JumpCooldown());
            jumpTime = jumpStartTime;
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }
        if (isJumping && inputy > 0) {
            if (jumpTime <= 0) 
                isJumping = false;
            else {
                r2d.velocity = new Vector2(r2d.velocity.x, r2d.velocity.y + jumpHeight);
                jumpTime -= Time.deltaTime;
            }
        }
        if (inputy <= 0) {
            isJumping = false;
        }
    }

    void toggleGrab() {
        if (grabbedObject) {
            try {grabbedObject.GetComponent<SimpleObject>().Ungrab();}
            catch {grabbedObject.GetComponent<Door>().Ungrab();}

            grabbedObject = null;
            return;
        }

        //look for closest collider nearProps
        float oldDistance = float.PositiveInfinity;
        Collider2D closestObj = null;
        foreach (Collider2D obj in collision.nearProps)////////what
        {
            float dist = Vector2.Distance(this.gameObject.transform.position, obj.gameObject.transform.position);
            if (dist < oldDistance && (obj.gameObject.CompareTag("Prop") || obj.gameObject.CompareTag("Lever")))
            {
                closestObj = obj;
                oldDistance = dist;
            }
        }

        if (closestObj == null) return;
        Debug.Log(closestObj);
        grabbedObject = closestObj.gameObject;

        try {grabbedObject.GetComponent<SimpleObject>().Grab();}
        catch {grabbedObject.GetComponent<Door>().Grab();}
    }

    private void updateCharState() {
        if (moveDirection < 0) spriteRenderer.flipX = true;
        if (moveDirection > 0) spriteRenderer.flipX = false;


        if (System.Math.Abs(r2d.velocity.y) > 0.25f || !canJump) {
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