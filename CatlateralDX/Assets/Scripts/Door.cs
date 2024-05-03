using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsBehindDoor;
    [SerializeField] Sprite doorClose, doorOpen;

    [Space]

    [SerializeField] AudioClip open_audio;
    [SerializeField] AudioClip close_audio;
    
    private SpriteRenderer spriteRenderer;
    private AudioSource audiosource;


    private bool isOpen = true;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audiosource = GetComponent<AudioSource>();
        toggleObjectsBehindDoor(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            toggleObjectsBehindDoor(true);
        }
        
    }

    
    public void toggleObjectsBehindDoor(bool playAudio) {
        isOpen = !isOpen;

        spriteRenderer.sprite = (isOpen) ? doorOpen : doorClose;
        
        //toggle physics
        foreach (GameObject obj in objectsBehindDoor) {

            //disable colliders
            Collider2D[] colliders = obj.GetComponents<Collider2D>();
                foreach (Collider2D coll in colliders) {
                    coll.forceReceiveLayers = (isOpen) ? LayerMask.NameToLayer("Everything") : LayerMask.GetMask("Nothing");
                    coll.forceSendLayers = (isOpen) ? LayerMask.NameToLayer("Everything") : LayerMask.GetMask("Nothing");
                    //coll.excludeLayers = (isOpen) ? LayerMask.NameToLayer("Nothing") : LayerMask.GetMask("Player");
                }

                try { //objects only have rigidbody if SimpleObject, not if Platform
                    obj.GetComponent<SimpleObject>().toggleObject(isOpen);

                    obj.GetComponent<Rigidbody2D>().isKinematic = !isOpen;
                    if (!isOpen) {
                        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                        obj.GetComponent<Rigidbody2D>().angularVelocity = 0;
                    }
                } catch {}
            
            obj.GetComponent<Transform>().position = new Vector3(obj.GetComponent<Transform>().position.x, 
                                obj.GetComponent<Transform>().position.y, (isOpen) ? -2 : 0); 
            //if z=-2, then objects are in front of the door
        }

    
        //sound
        if (isOpen && playAudio) {
            audiosource.clip = open_audio;
        } else if (playAudio) {
            audiosource.clip = close_audio;
        }
        audiosource.Play();
    }
    
    public static Bounds Get2DBounds(Bounds aBounds)
    {
        var ext = aBounds.extents;
        ext.z = float.PositiveInfinity;
        aBounds.extents = ext;
        return aBounds;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform") || collision.gameObject.CompareTag("Player")) return;

        Collider2D obj = collision.gameObject.GetComponent<Collider2D>();
        Collider2D doorCollider = GetComponent<Collider2D>();
        

        if (Get2DBounds(doorCollider.bounds).Contains(obj.bounds.min)
                && Get2DBounds(doorCollider.bounds).Contains(obj.bounds.max)
                && ! objectsBehindDoor.Contains(collision.gameObject)) {
            objectsBehindDoor.Add(collision.gameObject);
        }
        else if ((!Get2DBounds(doorCollider.bounds).Contains(obj.bounds.min)
                || !Get2DBounds(doorCollider.bounds).Contains(obj.bounds.max)) 
                && objectsBehindDoor.Contains(collision.gameObject)) {
            objectsBehindDoor.Remove(collision.gameObject);
        } 
    }

    public void Grab() {
        toggleObjectsBehindDoor(true);
    }
    public void Ungrab() {
        toggleObjectsBehindDoor(true);
    }
}
