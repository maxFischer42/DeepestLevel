using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreCalculator : MonoBehaviour
{
    public Text coinCount;
    public Text goldCount;
    public Text gemCount;
    public Text comboCount;
    public Text wispCount;

    public int coin, gold, gem, combo, wisp;

    public Text coinTotal;
    public Text goldTotal;
    public Text gemTotal;
    public Text comboTotal;
    public Text wispTotal;

    public int coinScore, goldScore, gemScore, comboScore, wispScore;

    public Text myScore;
    public Text highScore;
    public Text prompt;

    public void Start() {
        coin = PlayerPrefs.GetInt("coin");
        gold = PlayerPrefs.GetInt("gold");
        gem = PlayerPrefs.GetInt("gem");
        combo = PlayerPrefs.GetInt("combo");
        wisp = PlayerPrefs.GetInt("wisp");

        coinCount.text = "x " + coin;
        goldCount.text = "x " + gold;
        gemCount.text = "x " + gem;
        comboCount.text = "x " + combo;
        wispCount.text = "x " + wisp;

        coinScore = coin * 10;
        goldScore = gold * 20;
        gemScore = gem * 50;
        comboScore = Mathf.RoundToInt(combo * 0.25f);
        wispScore = wisp;

        int s = ((coinScore + goldScore + gemScore) * comboScore) * wispScore;
        int t = PlayerPrefs.GetInt("HS");
        if(s > t) {
            PlayerPrefs.SetInt("HS", s);
            t = s;
        }
 
        StartCoroutine(delay(s, t));
    }

    
    public void Update() {
        if(Input.GetButtonDown("Jump") ||
            Input.GetButtonDown("Action") ||
            Input.GetButtonDown("Run") ||
            Input.GetButtonDown("Menu") ||
            Input.GetAxis("Horizontal") != 0||
            Input.GetAxis("Vertical") != 0
        ) {
            SceneManager.LoadScene("Main");
        }
    }

    IEnumerator delay(int s, int t) {
        yield return new WaitForSeconds(1f);
        coinTotal.text = "" + coinScore;
        yield return new WaitForSeconds(1f);
        goldTotal.text = "" + goldScore;
        yield return new WaitForSeconds(1f);
        gemTotal.text = "" + gemScore;
        yield return new WaitForSeconds(1f);
        comboTotal.text = "" + comboScore;
        yield return new WaitForSeconds(1f);
        wispTotal.text = "" + wispScore;
        yield return new WaitForSeconds(2.5f);
        myScore.text = "Score: " + s;
        yield return new WaitForSeconds(3f);
        highScore.text = "High Score: " + t;
        yield return new WaitForSeconds(1f);
        prompt.text = "Press Any Button to Return";
    }
}
