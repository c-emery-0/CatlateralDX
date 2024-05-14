using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMenus : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenu, introMenu, finishMenu;

    [SerializeField] private TextMeshProUGUI pointstext, completiontext;

    void Start() {
        if (pauseMenu) Resume();
        if (finishMenu) finishMenu.SetActive(false);
        if (introMenu) {
            introMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true; 
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) Resume();
            else Pause();
        }
    }
    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void StartGame() {
        introMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void EndGame() {
        finishMenu.SetActive(true);
        
        PointCounter ptcounter = GameObject.Find("PointCounter").GetComponent<PointCounter>();

        //show text if player found all objects
        if (ptcounter.IsAllObjDestroyed()) {
            completiontext.text = "...You found all objects in the level!";
        } else {
            completiontext.text = "...You broke "+ ptcounter.GetNumObjDestroyed()
                                +" out of "+ptcounter.propscountInitial+" total objects!";
        }
        //scroll numbers
        StartCoroutine(CountToNum(ptcounter.points));
        pointstext.text = "" + ptcounter.points + " pts";
    }

    //end of level menu scroll numbers
    private IEnumerator CountToNum(int target) {
        float countDuration = 1.5f;
        int current = 0;
        
        //WHY DOES THIS JITTER
        while (current < target) {
            int num = (int) (target / countDuration * Time.deltaTime);
            current += (num == 0) ? 1 : num;
            current = Mathf.Clamp(current, 0, target);
            pointstext.text = "" + current + " pts";
            yield return null;
        }
    }
}
