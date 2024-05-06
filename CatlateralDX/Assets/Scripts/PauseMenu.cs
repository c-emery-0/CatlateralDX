using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject introMenu;

    void Start() {
        toggleMenu(introMenu, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) toggleMenu(pauseMenu, false);
            else toggleMenu(pauseMenu, true);
        }
    }


    public void toggleMenu(GameObject menu, bool toggleOn = !isPaused) {
        menu.SetActive(toggleOn);
        Time.timeScale = (toggleOn) ? 0f : 1f;
        isPaused = toggleOn;
    }
    
}
