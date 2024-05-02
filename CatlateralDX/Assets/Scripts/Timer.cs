using UnityEngine;
using System.Collections;
using TMPro;

public class Timer: MonoBehaviour {

    public float targetTime = 6.0f;
    [SerializeField] private TextMeshProUGUI timer_text;

    void Update(){

        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            timerEnded();
        }

        timer_text.text = ""+ (int) targetTime;

    }

    void timerEnded()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadScene("Start");

    }


}
