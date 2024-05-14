using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObject : MonoBehaviour
{

    [SerializeField] AudioClip[] collisionSounds;    
    [SerializeField] AudioClip[] breakSounds;
    private bool knockedOver = false;
    private bool broken = false;
    private bool behindDoor = false;
    private bool followPlayer = false;
    private float moveSpeed = 50f;


    private PointCounter pointCounter;
    private GameObject player;
    private AudioSource audiosource;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pointCounter = GameObject.Find("PointCounter").GetComponent<PointCounter>();
        audiosource = GetComponent<AudioSource>();

        //remove this as soon as we have unique sounds for break vs knock over
        if (breakSounds == null || breakSounds.Length == 0) collisionSounds = breakSounds;

        //set particle color to ave color ? 
        ParticleSystem.MainModule ps_mm = GetComponent<ParticleSystem>().main;
        ps_mm.startColor = AverageColor();

    }

    // Update is called once per frame
    void Update()
    {

        //make sure the effector is oriented utward
        PlatformEffector2D effector = GetComponent<PlatformEffector2D>();
        effector.rotationalOffset = -1 * GetComponent<Transform>().rotation.eulerAngles.z‌;


        //ouhh the 'grabbed objects follow player' really isn't working HAHAHA
        if (followPlayer) {
            Vector2 pos = GetComponent<Transform>().position;
            Vector2 playerpos = new Vector2(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y);
            Vector2 direction = (pos - playerpos).normalized; 
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed); //rockets obj away from player :salute:
        }

    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (behindDoor) return;
        
        if (collision.gameObject.CompareTag("Player") 
        && collision.relativeVelocity.magnitude > 14.75f && !knockedOver) {            
            //if obj have multiple breaking sounds, play any
            int randNum = (int) UnityEngine.Random.value * collisionSounds.Length;
            if (collisionSounds.Length > 0) {
                audiosource.clip = collisionSounds[randNum];
                audiosource.Play();
            }

            StartCoroutine(breakObject());
            pointCounter.UpdatePoints(10, GetComponent<Transform>().position);  

            knockedOver = true;
        }
        /**
        //distinguish "knocking over" state from "breaking" state
        if (collision.gameObject.CompareTag("Player") && !broken && !collision.gameObject.canDash) {
            
            int randNum = (int) UnityEngine.Random.value * breakSounds.Length;
            audiosource.clip = breakSounds[randNum];
            audiosource.Play();

            pointCounter.UpdatePoints(50);    
            broken = true;
            StartCoroutine(breakObject());
        }*/

    }

    public void toggleObject(bool isOpen) {
        behindDoor = !isOpen;
    }


    //okay this DEFINITELY isn't working
    public void Grab() {

        //disable colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D coll in colliders) {
            coll.forceReceiveLayers =  LayerMask.GetMask("Nothing");
            coll.forceSendLayers = LayerMask.GetMask("Nothing");
        }

        GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, 
                            GetComponent<Transform>().position.y, -9); //set obj to directly behind player
        followPlayer = true;

        //this method currently ROCKETS objects away from player so .... may as well give them points for that
        pointCounter.UpdatePoints(5, GetComponent<Transform>().position);  
    }
    public void Ungrab() {
        //enable colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D coll in colliders) {
            coll.forceReceiveLayers = LayerMask.NameToLayer("Everything");
            coll.forceSendLayers = LayerMask.NameToLayer("Everything");
            
        }

        GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, 
                            GetComponent<Transform>().position.y, 0); //set obj to normal z

        followPlayer = false;
    }

    private IEnumerator breakObject() {
        Debug.Log("Breaking "+gameObject+". Behind door is "+behindDoor);
        GetComponent<ParticleSystem>().Play();
        
        //remove visually before actuallly deleting object
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            sprite.enabled = false;
        
        //freeze object
        foreach (Collider2D coll in GetComponents<Collider2D>()) {
            coll.enabled = false;
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().angularVelocity = 0;

        //actually delete gameObject
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    
    //ArgumentException: Texture2D.GetPixels: texture data is either not readable, corrupted or does not exist
    private Color AverageColor() {
        Color[] allcolors = GetComponent<SpriteRenderer>().sprite.texture.GetPixels();
        int total =0; float r =0f, g=0f, b=0f;
        foreach (Color c in allcolors) {
            if (c.a < 0.9) continue;
            total += 1;
            r += c.r * c.r;
            g += c.g * c.g;
            b += c.b * c.b;
            Debug.Log(c.a);
        }
        Debug.Log("" + gameObject + " " + (r/(float) total) + " "+ (g/(float) total) + " "+ (b/(float) total));
        return new Color((float) System.Math.Sqrt(r/(float) total), 
                        (float) System.Math.Sqrt(g/(float) total), 
                        (float) System.Math.Sqrt(b/(float) total));
    }
}
