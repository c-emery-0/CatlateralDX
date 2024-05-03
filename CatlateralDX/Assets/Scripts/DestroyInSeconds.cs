using UnityEngine;
using System.Collections;

public class DestroyInSeconds : MonoBehaviour
{
    [SerializeField] private float secondsToDestroy = .5f;

    void Start() {
        Destroy(gameObject, secondsToDestroy);
    }
}