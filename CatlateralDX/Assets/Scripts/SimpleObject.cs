using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObject : MonoBehaviour
{

    [SerializeField] AudioClip[] collisionSounds;    
    [SerializeField] AudioClip collisionSound;

    [SerializeField] AudioClip[] breakSounds;

    private bool knockedOver = false;
    private bool broken = false;
    private PointCounter pointCounter;
    private GameObject player;
    private AudioSource audiosource;
    private bool followPlayer = false;
    private float moveSpeed = 100f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pointCounter = GameObject.Find("PointCounter").GetComponent<PointCounter>();
        audiosource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        //make sure the effector is oriented utward
        PlatformEffector2D effector = GetComponent<PlatformEffector2D>();
        effector.rotationalOffset = -1 * GetComponent<Transform>().rotation.eulerAngles.z‌;

        if (followPlayer) {
            Vector2 pos = GetComponent<Transform>().position;
            Vector2 playerpos = new Vector2(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y);
            Vector2 direction = (pos - playerpos).normalized;
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !knockedOver) {
            int randNum = (int) UnityEngine.Random.value * collisionSounds.Length;

            if (collisionSounds.Length > 0) {
            audiosource.clip = collisionSounds[randNum];
            audiosource.Play();}
            StartCoroutine(breakObject());
            pointCounter.UpdatePoints(10);    
            knockedOver = true;
        }
        /*
        if (collision.gameObject.CompareTag("Player") && !broken && !collision.gameObject.canDash) {
            
            int randNum = (int) UnityEngine.Random.value * breakSounds.Length;
            audiosource.clip = breakSounds[randNum];
            audiosource.Play();

            pointCounter.UpdatePoints(50);    
            broken = true;
            StartCoroutine(breakObject());
        }**/

    }

    public void toggleObjectBehindDoor(bool isOpen) {
        Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D coll in colliders) {
                //coll.forceReceiveLayers = (isOpen) ? LayerMask.NameToLayer("Everything") : LayerMask.GetMask("Nothing");
                //coll.forceSendLayers = (isOpen) ? LayerMask.NameToLayer("Everything") : LayerMask.GetMask("Nothing");
                coll.excludeLayers = (isOpen) ? LayerMask.NameToLayer("Nothing") : LayerMask.GetMask("Player");
            }

            try {

                GetComponent<Rigidbody2D>().isKinematic = !isOpen;

                if (!isOpen) {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    GetComponent<Rigidbody2D>().angularVelocity = 0;
                }
            } catch {}
    }


    public void Grab() {

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D coll in colliders) {
            coll.forceReceiveLayers =  LayerMask.GetMask("Nothing");
            coll.forceSendLayers = LayerMask.GetMask("Nothing");
            
        }

        GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, 
                            GetComponent<Transform>().position.y, -9); //set obj to directly behind player

        followPlayer = true;
    }
    public void Ungrab() {

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D coll in colliders) {
            coll.forceReceiveLayers = LayerMask.NameToLayer("Everything");
            coll.forceSendLayers = LayerMask.NameToLayer("Everything");
            
        }

        GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, 
                            GetComponent<Transform>().position.y, 0); //set obj to normal :)

        followPlayer = false;
    }

    private IEnumerator breakObject() {
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
