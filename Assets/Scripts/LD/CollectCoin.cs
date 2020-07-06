using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    public string name;
    public AudioClip audio;

    void OnCollisionEnter2D(Collision2D c) {
        if(c.gameObject.tag == "Player") {
            PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + 1);
            GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(audio);
            Destroy(gameObject);
        }
    }
}
