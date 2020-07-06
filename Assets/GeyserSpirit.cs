using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserSpirit : MonoBehaviour
{
    public RuntimeAnimatorController idle;
    public RuntimeAnimatorController attack;

    private Animator animator;

    public GameObject particles;

    public bool AMode = false;

    public float time;
    private float timer;

    public float roll;
    public GameObject rollForObject;

    void Start() {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        timer+= Time.deltaTime;
        if(timer >= time) {
            AMode = !AMode;
            timer = 0;
            if(AMode) {
                animator.runtimeAnimatorController = attack;
                if(Random.Range(0,100) > roll) {
                    Instantiate(rollForObject, particles.transform);
                }
                particles.GetComponent<ParticleSystem>().Play();
            } else {
                animator.runtimeAnimatorController = idle;
                particles.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}
