using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{
    public AudioSource aud;

    // Update is called once per frame
    void Update()
    {
        aud.volume = PlayerPrefs.GetFloat("y23s4A21");
    }
}
