using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ravena : MonoBehaviour
{

    public RuntimeAnimatorController idle, chargeStart, charge, slam, slash, death;

    public float timer;
    private Animator animator;

    public GameObject slambox;

    private Rigidbody2D rig;

    public float idleToCharge, timeToCharge, timeSlam;
    public enum variant {idle, charge, slam, stop};
    public variant myVariant;

    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    void DoChase() {
        Vector2 dir = GameObject.Find("Player").transform.position- transform.position;
        dir.Normalize();
        rig.velocity = (new Vector2(moveSpeed * dir.x, moveSpeed * dir.y));
    }

    public GameObject spawners;
    bool dead = false;

    public void Kill() {
        dead = true;
        GameObject.Find("Player").GetComponent<Controller>().enabled = false;
        GameObject.Find("Player").GetComponent<PlayerHealthManager>().enabled = false;
        rig.velocity = Vector2.zero;
        Destroy(spawners);
        StartCoroutine(KillBoss());
    }

    IEnumerator KillBoss() {
        yield return new WaitForSeconds(2.3f);
        animator.runtimeAnimatorController = slash;
        yield return new WaitForSeconds(1f);
        animator.runtimeAnimatorController = idle;
        yield return new WaitForSeconds(6f);
        animator.runtimeAnimatorController = death;
        yield return new WaitForSeconds(2.5f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Win");
    }

    // Update is called once per frame
    void Update()
    {
        if(dead)
            return;
        if(myVariant == variant.idle) {
            DoChase();
        }
        timer += Time.deltaTime;
        switch(myVariant) {
            case variant.idle:
                if(timer > idleToCharge) {
                    timer = 0;
                    animator.runtimeAnimatorController = charge;
                    myVariant = variant.charge;
                    rig.velocity = Vector2.zero;
                }
                break;
            case variant.charge:
                if(timer > timeToCharge) {
                    timer = 0;
                    animator.runtimeAnimatorController = slam;
                    myVariant = variant.slam;
                    slambox.SetActive(true);
                }
                break;
            case variant.slam:
                if(timer > timeSlam) {
                    timer = 0;
                    animator.runtimeAnimatorController = idle;
                    myVariant = variant.idle;
                    slambox.SetActive(false);
                }
                break;
        }
    }
}
