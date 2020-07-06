using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{

    public Slider soundSlider;
    public Slider volumeSlider;

    public InputField UpB;
    public InputField DownB;
    public InputField LeftB;
    public InputField RightB;
    public InputField JumpB;
    public InputField RunB;
    public InputField ActionB;

    public bool useCustom;

    public Toggle toggle;


    public bool isMain;
    public Text highscore;

    public void Start() {
        if(PlayerPrefs.GetInt("c") == 0) {
            PlayerPrefs.SetFloat("y23s4A21", 0.8f);
            PlayerPrefs.SetFloat("g3hdpi^&", 0.3f);
            PlayerPrefs.SetInt("c",1);
        } 

        if(soundSlider) {
            soundSlider.value = PlayerPrefs.GetFloat("y23s4A21");
            volumeSlider.value = PlayerPrefs.GetFloat("g3hdpi^&");
            switch(PlayerPrefs.GetInt("cust") > 0) {
                case true:
                    toggle.isOn = true;
                    break;
                case false:
                    toggle.isOn = false;
                    break;
            }
        }
    }

    public void toggleCustom() {
        useCustom = !useCustom;
        switch(useCustom) {
            case true:
                PlayerPrefs.SetInt("cust", 1);
                break;
            case false:
                PlayerPrefs.SetInt("cust", 0);
                break;
        }
    }

    public void SetControls() {
        string upKey = UpB.text;
        PlayerPrefs.SetString("Up_", upKey);
        string downKey = DownB.text;
        PlayerPrefs.SetString("Down_", downKey);
        string leftKey = LeftB.text;
        PlayerPrefs.SetString("Left_", leftKey);
        string rightKey = RightB.text;
        PlayerPrefs.SetString("Right_", rightKey);
        string jumpKey = JumpB.text;
        PlayerPrefs.SetString("Jump_", jumpKey);
        string runKey = RunB.text;
        PlayerPrefs.SetString("Run_", runKey);
        string actionKey = ActionB.text;
        PlayerPrefs.SetString("Action_", actionKey);
    }

    public void Update() {
        if(isMain) {
            highscore.text = "Highscore: " + PlayerPrefs.GetInt("HS");
        }
    }

    public void ChangeSoundVolume() {
        PlayerPrefs.SetFloat("y23s4A21", soundSlider.value);
    }

    public void ChangeMusicVolume() {
        PlayerPrefs.SetFloat("g3hdpi^&", volumeSlider.value);
    }

    public void LoadMain() {
        SceneManager.LoadScene("Main");
    }

    public void LoadOptions() {
        SceneManager.LoadScene("Controls"); 
    }

    public void LoadControls() {
        SceneManager.LoadScene("HowTo"); 
    }

    public void Reset() {
        PlayerPrefs.SetInt("coin", 0);  
        PlayerPrefs.SetInt("gold", 0);  
        PlayerPrefs.SetInt("gem", 0);  
        PlayerPrefs.SetInt("wisp", 0);  
        PlayerPrefs.SetInt("combo", 0);    
        PlayerPrefs.SetInt("HS", 0);
        PlayerPrefs.SetInt("c",0);
        PlayerPrefs.SetString("Up_", "");
        PlayerPrefs.SetString("Down_", "");
        PlayerPrefs.SetString("Left_", "");
        PlayerPrefs.SetString("Right_", "");
        PlayerPrefs.SetString("Jump_", "");
        PlayerPrefs.SetString("Run_", "");
        PlayerPrefs.SetString("Action_", "");
        PlayerPrefs.SetInt("cust", 0);

    }

    public void LoadGame() {
        PlayerPrefs.SetInt("coin", 0);  
        PlayerPrefs.SetInt("gold", 0);  
        PlayerPrefs.SetInt("gem", 0);  
        PlayerPrefs.SetInt("wisp", 0);  
        PlayerPrefs.SetInt("combo", 0);    
        SceneManager.LoadScene("SampleScene"); 
    }

    public void Exit() {
        Application.Quit();
    }
}
