using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject[] objectsBehindDoor;
    [SerializeField] Sprite doorClose, doorOpen;

    private SpriteRenderer spriteRenderer;
    
    private bool isOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        toggleObjectsBehindDoor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            toggleObjectsBehindDoor();
        }
    }

    void toggleObjectsBehindDoor() {
        isOpen = !isOpen;

        spriteRenderer.sprite = (isOpen) ? doorOpen : doorClose;

        foreach (GameObject obj in objectsBehindDoor) {
            Collider2D[] colliders = obj.GetComponents<Collider2D>();
            foreach (Collider2D coll in colliders) {
                coll.enabled = isOpen;
            }

            obj.GetComponent<Transform>().position = new Vector3(obj.GetComponent<Transform>().position.x, 
                                obj.GetComponent<Transform>().position.y, (isOpen) ? -2 : 0); //if z=-2, then objects are in front of the door

            try {
                obj.GetComponent<Rigidbody2D>().isKinematic = !isOpen;
            } catch {}
        }

    }
}
