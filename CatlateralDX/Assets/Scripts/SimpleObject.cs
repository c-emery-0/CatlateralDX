using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObject : MonoBehaviour
{

    private bool knockedOver = false;
    private bool broken = false;
    public PointCounter pointCounter;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        effector = GetComponent<PlatformEffector2D>();
        GetComponent<Transform>().rotation.z
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            pointCounter.UpdatePoints(10);    
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
