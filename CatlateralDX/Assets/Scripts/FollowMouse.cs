using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    private Vector3 mousePosition;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float moveSpeed = 100f;

	
	// Update is called once per frame
	void Update () {
        if (!Input.mousePresent) {
            Debug.Log("where da mouse??");
            return;
        }
        Debug.Log("following mouse!");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) ;
        direction = (mousePosition - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        
    }
}