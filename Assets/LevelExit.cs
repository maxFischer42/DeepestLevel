using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    public string levelToLoad;

    void OnTriggerStay2D(Collider2D c) {
        if (c.gameObject.tag == "Player" && Input.GetAxis("Vertical") > 0) {
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void Update() {
        if(Input.GetKey(KeyCode.Numlock)) {
            SceneManager.LoadScene(levelToLoad);
        }
    } 
}
