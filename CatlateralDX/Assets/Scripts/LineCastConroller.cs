using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCastController : MonoBehaviour
{
    
    LineRenderer lineRenderer;
    public LineCastController() {
        lineRenderer = GetComponent<LineRenderer>();
        width(0.1);
        setpts(Vector2(0, 0));
    }
    public width(param) {
        lineRenderer.widthMultiplier(param);
    }
    public setpts(start, end) {
        lineRenderer.SetPositions()
    }
}