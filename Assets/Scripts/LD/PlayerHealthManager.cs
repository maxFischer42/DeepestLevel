using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    
    public int hp = 8;
    public Controller controller;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Sprite hurtSprite;

    public RuntimeAnimatorController death;

    public Color defaultColor;
    public Color hurtColor;

    public AudioClip sound;

    public GameObject[] hpObjects;

    public bool start;

    public void Start() {
        controller = GetComponent<Controller>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        print(PlayerPrefs.GetInt("HP"));
        if(!start) {
            hp = PlayerPrefs.GetInt("HP");
            for(int i = 0; i < PlayerPrefs.GetInt("HP"); i++) {
                hpObjects[i].SetActive(true);
            }
        }
        if(start) {
            PlayerPrefs.SetInt("HP", hp);
        }
    }

    void Update() {
        if(hp <= 0) {
            StartCoroutine(KillPlayer());
        }
        PlayerPrefs.SetInt("HP", hp);
    }

    public bool recovering;

    IEnumerator Recover() {
        recovering = true;
        spriteRenderer.color = hurtColor;
        yield return new WaitForSeconds(1f);
        spriteRenderer.color = defaultColor;
        recovering = false;
    }

    

    public void OnTriggerEnter2D(Collider2D c) {
        if(controller.isAttacking || recovering)
            return;
        if(c.gameObject.tag == "EHit" || c.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            if(controller.isHit)
                return;
                hpObjects[hp - 1].SetActive(false);
            hp--;
            GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(sound);
            PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") - 1);
            spriteRenderer.sprite = hurtSprite;
            StartCoroutine(doHit());
            StartCoroutine(Recover());
        }
    }
    
    public void OnCollisionEnter2D(Collision2D c) {
        if(controller.isAttacking || recovering)
            return;
        if(c.gameObject.tag == "EHit" || c.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            if(controller.isHit)
                return;
            hpObjects[hp - 1].SetActive(false);
            hp--;
            GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(sound);
            PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") - 1);
            spriteRenderer.sprite = hurtSprite;
            StartCoroutine(doHit());
            StartCoroutine(Recover());
        }
    }

    IEnumerator KillPlayer() {
        controller.enabled = false;
        animator.enabled = true;
        animator.runtimeAnimatorController = death;
        yield return new WaitForSeconds(0.7f);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1.5f);
        PlayerPrefs.SetInt("Score", 0);
        SceneManager.LoadScene("Main");
    }

    public IEnumerator doHit() {
        animator.enabled = false;
        controller.isHit = true;
        yield return new WaitForSeconds(1f);
        controller.isHit = false;
        animator.enabled = true;
    }

}
