using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int mhp;
    private int chp;

    public bool hasHurt;
    public Sprite hurtSprite;

    private Color defColor;
    public Color hurtColor;

    private Animator an;

    void Start() {
        chp = mhp;
        defColor = GetComponent<SpriteRenderer>().color;
        an = GetComponent<Animator>();
    }

    void Update() {
        if(chp <= 0) {
            if(finale) {
                GetComponent<Ravena>().Kill();
                enabled = false;
            } else {
                Destroy(gameObject);
            }
        }

    }

    public Vector2 scale = new Vector2(1,1);

    public float knockbackForce;

    public bool isHit = false;

    void HandleHit() {
        Vector2 d = GameObject.Find("Player").transform.position - transform.position;
        d.Normalize();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(-d.x * knockbackForce, Mathf.Abs(d.y) * knockbackForce));
        StartCoroutine(ChangeHurt());
    }

    public bool finale = false;

    IEnumerator ChangeHurt() {
        an.enabled = false;
        GetComponent<SpriteRenderer>().sprite = hurtSprite;
        yield return new WaitForSeconds(0.3f);
        an.enabled = true;
    }

    IEnumerator ChangeColor() {
        GetComponent<SpriteRenderer>().color = hurtColor;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = defColor;
    }

    public AudioClip sound;

    void OnTriggerEnter2D(Collider2D c) {
        if(c.GetComponent<HitboxData>() && c.gameObject.tag == "PHit") {
            if(isHit)
                return;
            int e = c.GetComponent<HitboxData>().damage * GameObject.Find("Player").GetComponent<Controller>().combo;
            if(1 > e)
                e = 1;
            chp -= e;
            GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(sound);
            if(hasHurt){
                isHit = true;
                HandleHit();
            } else
                StartCoroutine(ChangeColor());
            
            GameObject.Find("Player").GetComponent<Controller>().CreateHitEffect(transform, 0, scale);
            GameObject.Find("Player").GetComponent<Controller>().SpawnCombo();
            if(GameObject.Find("Player").GetComponent<Controller>().previousAttack != GameObject.Find("Player").GetComponent<Controller>().prevTwoAttack) {
                GameObject.Find("Player").GetComponent<Controller>().combo++;
                PlayerPrefs.SetInt("combo", PlayerPrefs.GetInt("combo") + 1);
            }

        }
    }
}
