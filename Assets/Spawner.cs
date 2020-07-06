using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    public float timer;
    public float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= timer) {
            Instantiate(spawnObject, transform);
            time = 0;
        }
    }
}
