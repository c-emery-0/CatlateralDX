using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObject : MonoBehaviour
{

    private bool knockedOver = false;
    private bool broken = false;
    private PointCounter pointCounter;

    // Start is called before the first frame update
    void Start()
    {
        pointCounter = GetComponent<PointCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        pointCounter.UpdatePoints(10);    
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
