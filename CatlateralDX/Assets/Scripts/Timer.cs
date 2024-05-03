using UnityEngine;
using System.Collections;
using TMPro;

public class Timer: MonoBehaviour {

    public bool timed;
    public float targetTime = 6.0f;
    [SerializeField] private TextMeshProUGUI timer_text;
    [SerializeField] private GameObject warning;

    void Start() {
        warning.SetActive(false);
    }

    void Update(){
        if (!timed) {
            timer_text.text = "∞";
            return;
        }

        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            timerEnded();
        }
        if (targetTime <= 5f) {
            warning.SetActive(true);
        }

        timer_text.text = ""+ (int) targetTime;

    }

    void timerEnded()
    {
        GameObject.Find("Canvas").GetComponent<GameMenus>().EndGame();

    }


}
