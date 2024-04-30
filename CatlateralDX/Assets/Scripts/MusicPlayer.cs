using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    static MusicPlayer player = null;

    void Awake()
    {
        if (player != null)
          Destroy(gameObject);
        else
          player = this;

     GameObject.DontDestroyOnLoad(gameObject);
    }
}
