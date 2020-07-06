using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timespawn : MonoBehaviour
{
    
    public GameObject Ravena;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 6) {
            Ravena.SetActive(true);
            enabled = false;
        }
    }
}
