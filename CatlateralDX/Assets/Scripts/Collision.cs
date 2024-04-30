using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer ;
    public LayerMask platformLayer  ;
    public LayerMask propLayer  ;

    [Space]

    public bool onGround;
    public bool onProp;
    public Collider2D onPlatformColl;
    public Collider2D[] nearProps;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;

    [Space]

    [Header("Collision")]
    
    [Range(0.2f, 0.8f)]
    public float collisionRadius;
    [Range(0.2f, 0.8f)]
    public float grabRadius;
    [Range(0.2f, 5f)]
    public float botScale, sideScale;
    private Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
        groundLayer =  LayerMask.GetMask("Ground");
        platformLayer  =  LayerMask.GetMask("Platform");
        propLayer  =  LayerMask.GetMask("Props");
        
    }

    // Update is called once per frame
    void Update()
    {  
        bottomOffset = botScale * Vector2.down;
        rightOffset = sideScale * Vector2.right;
        leftOffset = sideScale * Vector2.left;

        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onProp = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, propLayer);
        onPlatformColl = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, platformLayer);
        
        nearProps = Physics2D.OverlapCircleAll((Vector2)transform.position, grabRadius, propLayer);

        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) 
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = (onGround) ? Color.red : Color.green;

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position  + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position  + leftOffset, collisionRadius);
    }
}