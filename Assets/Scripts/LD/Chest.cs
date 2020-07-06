using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public Sprite open;

    public GameObject[] treasure;
    public float rate;
    public float vForce;
    public Vector2 rng;

    public bool can = false;

    private SpriteRenderer m_sprite;
    // Start is called before the first frame update
    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();    
    }

    void OnTriggerEnter2D(Collider2D c) {
        if(can)
            return;
        if(c.gameObject.tag == "PHit") {
            can = true;
            m_sprite.sprite = open;
            StartCoroutine(CreateTreasure());
        }
    }

    IEnumerator CreateTreasure() {
        int i = treasure.Length;
        int a = 0;
        do {
            var m = (GameObject)Instantiate(treasure[a], transform);
            var o = Random.Range(rng.x, rng.y);
            m.transform.position = new Vector3(m.transform.position.x, m.transform.position.y, -1);
            m.GetComponent<Rigidbody2D>().AddForce(new Vector2(o, vForce));
            yield return new WaitForSeconds(rate);
            a++;
        }while(i >= a);
        if(a >= i) {
            enabled = false;
        }
    }
}
