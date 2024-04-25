using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject[] objectsBehindDoor;
    [SerializeField] Sprite doorClose, doorOpen;

    private SpriteRenderer spriteRenderer;
    
    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        toggleObjectsBehindDoor(isOpen);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("C")) {
            isOpen = !isOpen;
            toggleObjectsBehindDoor(!isOpen);
        }
    }

    void toggleObjectsBehindDoor(bool state) {
        
        spriteRenderer.sprite = (state) ? doorOpen : doorClose;

        foreach (GameObject obj in objectsBehindDoor) {
            obj.GetComponent<Collider2D>().enabled=state;
            try {
                obj.GetComponent<Rigidbody2D>().excludeLayers = "Player";
            } catch {}
        }

    }
}
