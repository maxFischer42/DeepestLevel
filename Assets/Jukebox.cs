using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public AudioSource aud;

    // Update is called once per frame
    void Update()
    {
        aud.volume = PlayerPrefs.GetFloat("g3hdpi^&");
    }
}
