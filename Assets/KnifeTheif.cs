using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeTheif : MonoBehaviour
{
    
    public enum state {Idle, Chase, Attack, Hurt};
    public state currentState;
    private Transform player;

    public RuntimeAnimatorController aIdle;
    public RuntimeAnimatorController aChase;
    public RuntimeAnimatorController aAttack;
    public RuntimeAnimatorController aHurt;
    private Animator animator;

    private Rigidbody2D rig;

    public float attackTimer = 1;

    public GameObject attackPrefab;


    
    void Start() {
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    void CheckState() {
        if(attackCheck()) {
            currentState = state.Attack;
        } else {
            if(checkPlayerClose()) {
                currentState = state.Chase;
            } else {
                currentState = state.Idle;
            }
        }
        switch(currentState) {
            case state.Attack:
                DoAttack();
                break;
            case state.Chase:
                DoChase();
                break;
            case state.Idle:
                DoIdle();
                break;
            case state.Hurt:
                DoHurt();
                break;
        }
    }

    void DoIdle() {
        animator.runtimeAnimatorController = aIdle;
        rig.velocity = Vector2.zero;
    }

    public float chaseDis;
    public float attackDis;
    public LayerMask mask;

    public bool checkPlayerClose() {        
        Vector2 dir = player.transform.position - transform.position;
        dir.Normalize();
        RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, chaseDis, mask);
        if(ray.collider) {
            Debug.Log(ray.collider.gameObject.name);
            if(ray.collider.transform == player)
                return true;
        }
        return false;   
    }

    public bool isAttacking = false;

    public IEnumerator attack() {
        isAttacking = true;
        GameObject a = (GameObject)Instantiate(attackPrefab, transform);
        Destroy(a, 0.1f);
        yield return new WaitForSeconds(attackTimer);
        isAttacking = false;
    }

    public bool attackCheck() {
        Vector2 dir = player.transform.position - transform.position;
        dir.Normalize();
        RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, attackDis, mask);
        if(ray.collider) {
            Debug.Log(ray.collider.gameObject.name);
            if(ray.collider.transform == player)
                return true;
        }
        return false;   
    }

    public bool isFlipped = false;

    void handleFlip() {
        Vector2 dir = player.transform.position - transform.position;
        dir.Normalize();
        if(dir.x > 0) {
            isFlipped = true;
        } else {
            isFlipped = false;
        }
        GetComponent<SpriteRenderer>().flipX = isFlipped;
    }

    public float moveSpeed;

    void DoChase() {
        animator.runtimeAnimatorController = aChase;
        Vector2 dir = player.transform.position- transform.position;
        dir.Normalize();
        rig.AddForce(new Vector2(moveSpeed * dir.x, rig.velocity.y));

    }

    void DoAttack() {
        rig.velocity = Vector2.zero;
        animator.runtimeAnimatorController = aAttack;
        if(!isAttacking) {
            StartCoroutine(attack());
        }
    }

    void DoHurt() {
        animator.enabled = false;
    }

    IEnumerator hit() {
        isHurt = true;
        yield return new WaitForSeconds(0.4f);
        isHurt = false;
    }

    public bool isHurt = false;

    // Update is called once per frame
    void Update()
    {
        if(isHurt)
            return;
        if(GetComponent<HealthManager>().isHit) {
            GetComponent<HealthManager>().isHit = false;
            StartCoroutine(hit());
        }
        CheckState();
        handleFlip();
    }
}
