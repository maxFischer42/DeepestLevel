using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public GameObject pauseMenu;

    public void Update() {
        if(Input.GetButtonDown("Pause")) {
            if(Time.timeScale == 0) {
                Resume();
            } else {                
                DoPause();
            }
        }
    }

    public void DoPause() {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Resume() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Exit() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }


}
