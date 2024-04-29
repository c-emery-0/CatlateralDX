using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsBehindDoor;
    [SerializeField] Sprite doorClose, doorOpen;

    private SpriteRenderer spriteRenderer;
    
    private bool isOpen = true;

    private int fuck;

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
                coll.forceReceiveLayers = (isOpen) ? LayerMask.NameToLayer("Everything") : LayerMask.GetMask("Nothing");
                coll.forceSendLayers = (isOpen) ? LayerMask.NameToLayer("Everything") : LayerMask.GetMask("Nothing");
                
            }

            obj.GetComponent<Transform>().position = new Vector3(obj.GetComponent<Transform>().position.x, 
                                obj.GetComponent<Transform>().position.y, (isOpen) ? -2 : 0); //if z=-2, then objects are in front of the door

            try {

                obj.GetComponent<Rigidbody2D>().isKinematic = !isOpen;
            } catch {}
        }

    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("Player")) return;

        Collider2D obj = collision.gameObject.GetComponent<Collider2D>();
        Collider2D doorCollider = GetComponent<Collider2D>();
        
        if (doorCollider.bounds.Contains(obj.bounds.min) 
                && ! objectsBehindDoor.Contains(collision.gameObject)) {
            objectsBehindDoor.Add(collision.gameObject);
        }
        else if ((!doorCollider.bounds.Contains(obj.bounds.min) 
                || !doorCollider.bounds.Contains(obj.bounds.max))
                && objectsBehindDoor.Contains(collision.gameObject)) {
            objectsBehindDoor.Remove(collision.gameObject);
        } 
    }
}
